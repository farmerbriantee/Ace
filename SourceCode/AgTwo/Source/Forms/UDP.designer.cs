﻿using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgTwo
{
    public class CTraffic
    {     
        public int cntrGPSIn = 0;
        public int cntrGPSInBytes = 0;
        public int cntrGPSOut = 0;

        public uint helloFromMachine = 99, helloFromAutoSteer = 99, helloFromNav = 99;
    }

    public class CScanReply
    {
        public string steerIP =   "";
        public string machineIP = "";
        public string GPS_IP =    "";
        public string Nav_IP =    "";
        public string subnetStr = "";

        public byte[] subnet = { 0, 0, 0 };

        public bool isNewSteer, isNewMachine, isNewGPS, isNewNav;

        public bool isNewData = false;
    }

    public partial class FormLoop
    {
        // loopback Socket
        private Socket loopBackSocket;
        private EndPoint endPointLoopBack = new IPEndPoint(IPAddress.Loopback, 0);

        //2 endpoints for local and 2 udp

        private IPEndPoint epAgOpen = new IPEndPoint(IPAddress.Parse(
            Properties.Settings.Default.eth_loopOne.ToString() + "." +
            Properties.Settings.Default.eth_loopTwo.ToString() + "." +
            Properties.Settings.Default.eth_loopThree.ToString() + "." +
            Properties.Settings.Default.eth_loopFour.ToString()), 25555);

        // UDP Sockets
        public Socket UDPSocket;
        private EndPoint endPointUDP = new IPEndPoint(IPAddress.Any, 0);

        public bool isUDPNetworkConnected;

        //UDP Endpoints
        public IPEndPoint epModule = new IPEndPoint(IPAddress.Parse(
                Properties.Settings.Default.etIP_SubnetOne.ToString() + "." +
                Properties.Settings.Default.etIP_SubnetTwo.ToString() + "." +
                Properties.Settings.Default.etIP_SubnetThree.ToString() + ".255"), 28888);

        public IPEndPoint epHello = new IPEndPoint(IPAddress.Parse(
                Properties.Settings.Default.etIP_SubnetOne.ToString() + "." +
                Properties.Settings.Default.etIP_SubnetTwo.ToString() + "." +
                Properties.Settings.Default.etIP_SubnetThree.ToString() + ".255"), 27777);

        public IPEndPoint epModuleSet = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 27777);

        public byte[] ipAutoSet = { 192, 168, 5 };

        private IPEndPoint epNtrip;

        //class for counting bytes
        public CTraffic traffic = new CTraffic();
        public CScanReply scanReply = new CScanReply();

        //scan results placed here
        public string scanReturn = "Scanning...";
        
        // Data stream
        private byte[] buffer = new byte[1024];

        //used to send communication check pgn= C8 or 200
        private byte[] helloFromAgIO = { 0x80, 0x81, 0x7F, 200, 3, 56, 0, 0, 0x47 };

        public IPAddress ipCurrent;
        //initialize loopback and udp network
        public void LoadUDPNetwork()
        {
            helloFromAgIO[5] = 56;

            lblIP.Text = "";
            try //udp network
            {
                string bob = Dns.GetHostName();
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string data = IPA.ToString();
                        lblIP.Text += IPA.ToString().Trim() + "\r\n";
                    }
                }

                // Initialise the socket
                UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                UDPSocket.Bind(new IPEndPoint(IPAddress.Any, 29999));
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointUDP,
                    new AsyncCallback(ReceiveDataUDPAsync), null);

                isUDPNetworkConnected = true;
                btnUDP.BackColor = Color.LimeGreen;

                //if (!isFound)
                //{
                //    MessageBox.Show("Network Address of Modules -> " + Properties.Settings.Default.setIP_localAOG+"[2 - 254] May not exist. \r\n"
                //    + "Are you sure ethernet is connected?\r\n" + "Go to UDP Settings to fix.\r\n\r\n", "Network Connection Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //btnUDP.BackColor = Color.Red;
                //    lblIP.Text = "Not Connected";
                //}
            }
            catch (Exception e)
            {
                //WriteErrorLog("UDP Server" + e);
                MessageBox.Show(e.Message, "Serious Network Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnUDP.BackColor = Color.Red;
                lblIP.Text = "Error";
            }
        }

        private void LoadLoopback()
        { 
            try //loopback
            {
                loopBackSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                loopBackSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                loopBackSocket.Bind(new IPEndPoint(IPAddress.Loopback, 27777));
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointLoopBack, 
                    new AsyncCallback(ReceiveDataLoopAsync), null);
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error";
                MessageBox.Show("Load Error: " + ex.Message, "Loopback Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Send LoopBack

        private void SendToLoopBackMessageAOG(byte[] byteData)
        {
            SendDataToLoopBack(byteData, epAgOpen);
        }

        private void SendDataToLoopBack(byte[] byteData, IPEndPoint endPoint)
        {
            try
            {
                if (byteData.Length != 0)
                {
                    // Send packet to AgVR
                    loopBackSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, endPoint,
                        new AsyncCallback(SendDataLoopAsync), null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                loopBackSocket.EndSend(asyncResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Receive LoopBack

        private void ReceiveFromLoopBack(byte[] data)
        {
            //Send out to udp network
            SendUDPMessage(data, epModule);
        }

        private void ReceiveDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = loopBackSocket.EndReceiveFrom(asyncResult, ref endPointLoopBack);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                loopBackSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointLoopBack, 
                    new AsyncCallback(ReceiveDataLoopAsync), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromLoopBack(localMsg)));
            }
            catch (Exception)
            {
                //MessageBox.Show("ReceiveData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Send UDP

        public void SendUDPMessage(byte[] byteData, IPEndPoint endPoint)
        {
            if (isUDPNetworkConnected)
            {
                try
                {
                    // Send packet to the zero
                    if (byteData.Length != 0)
                    {
                        UDPSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None,
                           endPoint, new AsyncCallback(SendDataUDPAsync), null);
                    }
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK,
                    //MessageBoxIcon.Error);
                }

                if (isUDPMonitorOn)
                {
                    //it isn't ntrip
                    if (epNtrip == null)
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPoint.ToString() + "\t" + " > " + byteData[3].ToString() + "\r\n");
                    }
                    else
                    {
                        //make sure not ntrip
                        if (endPoint != epNtrip)
                        {
                            logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPoint.ToString() + "\t" + " > " + byteData[3].ToString() + "\r\n");
                        }
                    }
                }
            }
        }

        private void SendDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                UDPSocket.EndSend(asyncResult);
            }
            catch (Exception)
            {
                //WriteErrorLog(" UDP Send Data" + e.ToString());
                //MessageBox.Show("SendData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Receive UDP

        private void ReceiveDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = UDPSocket.EndReceiveFrom(asyncResult, ref endPointUDP);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                UDPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPointUDP, 
                    new AsyncCallback(ReceiveDataUDPAsync), null);

                BeginInvoke((MethodInvoker)(() => ReceiveFromUDP(localMsg)));

            }
            catch (Exception)
            {
                //WriteErrorLog("UDP Recv data " + e.ToString());
                //MessageBox.Show("ReceiveData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK,
                //MessageBoxIcon.Error);
            }
        }

        private void ReceiveFromUDP(byte[] data)
        {
            try
            {
                //Hello and scan reply
                if (data[0] == 0x80 && data[1] == 0x81)
                {
                    switch (data[3])
                    {
                        case 126:
                            {
                                traffic.helloFromAutoSteer = 0;
                                if (isViewAdvanced)
                                {
                                    double actualSteerAngle = (Int16)((data[6] << 8) + data[5]);
                                    lblSteerAngle.Text = (actualSteerAngle * 0.01).ToString("N1");
                                    lblWASCounts.Text = ((Int16)((data[8] << 8) + data[7])).ToString();

                                    lblSwitchStatus.Text = ((data[9] & 2) == 2).ToString();
                                    lblWorkSwitchStatus.Text = ((data[9] & 1) == 1).ToString();
                                }
                                break;
                            }
                        case 123:
                            {
                                traffic.helloFromMachine = 0;

                                if (isViewAdvanced)
                                {
                                    lbl1To8.Text = Convert.ToString(data[5], 2).PadLeft(8, '0');
                                    lbl9To16.Text = Convert.ToString(data[6], 2).PadLeft(8, '0');
                                }
                                break;
                            }

                        case 121:
                            {
                                traffic.helloFromNav = 0;
                                break;
                            }

                        case 203:
                            {
                                if (data[2] == 126)  //steer module
                                {
                                    scanReply.steerIP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                                    scanReply.subnet[0] = data[09];
                                    scanReply.subnet[1] = data[10];
                                    scanReply.subnet[2] = data[11];

                                    scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                                    scanReply.isNewData = true;
                                    scanReply.isNewSteer = true;
                                }
                                //
                                else if (data[2] == 123)   //machine module
                                {
                                    scanReply.machineIP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                                    scanReply.subnet[0] = data[09];
                                    scanReply.subnet[1] = data[10];
                                    scanReply.subnet[2] = data[11];

                                    scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                                    scanReply.isNewData = true;
                                    scanReply.isNewMachine = true;

                                }
                                else if (data[2] == 121)   //Nav Module
                                {
                                    scanReply.Nav_IP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                                    scanReply.subnet[0] = data[09];
                                    scanReply.subnet[1] = data[10];
                                    scanReply.subnet[2] = data[11];

                                    scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                                    scanReply.isNewData = true;
                                    scanReply.isNewNav = true;
                                }

                                else if (data[2] == 120)    //GPS module
                                {
                                    scanReply.GPS_IP = data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString() + "." + data[8].ToString();

                                    scanReply.subnet[0] = data[09];
                                    scanReply.subnet[1] = data[10];
                                    scanReply.subnet[2] = data[11];

                                    scanReply.subnetStr = data[9].ToString() + "." + data[10].ToString() + "." + data[11].ToString();

                                    scanReply.isNewData = true;
                                    scanReply.isNewGPS = true;
                                }

                                break;
                            }

                        default:
                            {
                                //module return via udp sent to AOG
                                SendToLoopBackMessageAOG(data);

                                break;
                            }
                    }

                    if (isUDPMonitorOn)
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + endPointUDP.ToString() + "\t" + " < " + data[3].ToString() + "\r\n");
                    }

                } // end of pgns

                //GPS sentences
                else if (data[0] == 36 && (data[1] == 71 || data[1] == 80 || data[1] == 75))
                {
                    traffic.cntrGPSOut += data.Length;
                    rawBuffer += Encoding.ASCII.GetString(data);
                    ParseNMEA(ref rawBuffer);

                    if (isGPSLogOn)
                    {
                        logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + System.Text.Encoding.ASCII.GetString(data));
                    }
                }
            }
            catch
            {

            }
        }

        #endregion
    }
}
