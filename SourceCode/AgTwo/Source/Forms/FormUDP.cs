﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormUDP : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        //used to send communication check pgn= C8 or 200
        private byte[] sendIPToModules = { 0x80, 0x81, 0x7F, 201, 5, 201, 201, 192, 168, 5, 0x47 };
        private byte[] ipCurrent = { 192, 168, 5 };
        private byte[] ipNew = { 192, 168, 5 };

        public FormUDP(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();

            nudFirstIP.Controls[0].Enabled = false;
            nudSecndIP.Controls[0].Enabled = false;
            nudThirdIP.Controls[0].Enabled = false;
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            mf.ipAutoSet[0] = 99;
            mf.ipAutoSet[1] = 99;
            mf.ipAutoSet[2] = 99;

            lblHostname.Text = Dns.GetHostName(); // Retrieve the Name of HOST

            lblNetworkHelp.Text =
                Properties.Settings.Default.etIP_SubnetOne.ToString() + " . " +
                Properties.Settings.Default.etIP_SubnetTwo.ToString() + " . " +
                Properties.Settings.Default.etIP_SubnetThree.ToString();

            nudFirstIP.Value = ipNew[0] = ipCurrent[0] = Properties.Settings.Default.etIP_SubnetOne;
            nudSecndIP.Value = ipNew[1] = ipCurrent[1] = Properties.Settings.Default.etIP_SubnetTwo;
            nudThirdIP.Value = ipNew[2] = ipCurrent[2] = Properties.Settings.Default.etIP_SubnetThree;

            ScanNetwork();
        }

        int tickCounter = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(!mf.scanReply.isNewData)
            {
                mf.ipAutoSet[0] = 99;
                mf.ipAutoSet[1] = 99;
                mf.ipAutoSet[2] = 99;
                btnAutoSet.Enabled = false;
            }
            else
            {
                btnAutoSet.Enabled = true;
            }

            if (mf.scanReply.isNewSteer)
            {
                lblSteerIP.Text = mf.scanReply.steerIP;
                mf.scanReply.isNewSteer = false;
                lblNewSubnet.Text = mf.scanReply.subnetStr;
            }

            if (mf.scanReply.isNewMachine)
            {
                lblMachineIP.Text = mf.scanReply.machineIP;
                mf.scanReply.isNewMachine = false;
                lblNewSubnet.Text = mf.scanReply.subnetStr;
            }

            if (mf.scanReply.isNewNav)
            {
                lblNav_IP.Text = mf.scanReply.Nav_IP;
                mf.scanReply.isNewNav = false;
                lblNewSubnet.Text = mf.scanReply.subnetStr;
            }

            if (mf.scanReply.isNewGPS)
            {
                lblGPSIP.Text = mf.scanReply.GPS_IP;
                mf.scanReply.isNewGPS = false;
                lblNewSubnet.Text = mf.scanReply.subnetStr;
            }

            if (tickCounter == 4)
            {                
                if (mf.btnSteer.BackColor == Color.LimeGreen) lblBtnSteer.BackColor = Color.LimeGreen;
                else lblBtnSteer.BackColor = Color.Red;

                if (mf.btnMachine.BackColor == Color.LimeGreen) lblBtnMachine.BackColor = Color.LimeGreen;
                else lblBtnMachine.BackColor = Color.Red;

                if (mf.btnGPS.BackColor == Color.LimeGreen) lblBtnGPS.BackColor = Color.LimeGreen;
                else lblBtnGPS.BackColor = Color.Red;

                if (mf.btnNav.BackColor == Color.LimeGreen) lblBtnNav.BackColor = Color.LimeGreen;
                else lblBtnNav.BackColor = Color.Red;
            }

            if (tickCounter > 5)
            {
                ScanNetwork();
                tickCounter = 0;
                lblSubTimer.Text = "Scanning";
            }
            else
            {
                 lblSubTimer.Text = "-";
            }
            tickCounter++;
        }

        private void ScanNetwork()
        {
            tboxNets.Text = "";

            lblSteerIP.Text = lblMachineIP.Text = lblGPSIP.Text = lblNav_IP.Text = lblNewSubnet.Text = "";
            mf.scanReply.isNewData = false;

            bool isSubnetMatchCard = false;

            byte[] scanModules = { 0x80, 0x81, 0x7F, 202, 3, 202, 202, 5, 0x47 };

            //Send out 255x4 to each installed network interface
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Supports(NetworkInterfaceComponent.IPv4) )
                {
                    foreach (var info in nic.GetIPProperties().UnicastAddresses)
                    {
                        // Only InterNetwork and not loopback which have a subnetmask
                        if (info.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(info.Address))
                        {
                            Socket scanSocket;
                            try
                            {
                                //create list of interface properties
                                if ((cboxUp.Checked && nic.OperationalStatus == OperationalStatus.Up) || !cboxUp.Checked)
                                {
                                    var properties = nic.GetIPStatistics();
                                    tboxNets.Text +=
                                            info.Address + "  - " + nic.OperationalStatus + "\r\n";

                                    tboxNets.Text += nic.Name.ToString() + "\r\n";
                                    tboxNets.Text += "Sent: " + (properties.NonUnicastPacketsSent
                                        + properties.UnicastPacketsSent).ToString()
                                        + "   Recd: " + (properties.NonUnicastPacketsReceived
                                        + properties.UnicastPacketsReceived).ToString() + "\r\n\r\n";
                                }

                                if ( nic.OperationalStatus == OperationalStatus.Up 
                                    && info.IPv4Mask != null)
                                {
                                    byte[] data = info.Address.GetAddressBytes();
                                    if (data[0] == ipCurrent[0] && data[1] == ipCurrent[1] && data[2] == ipCurrent[2])
                                    {
                                        isSubnetMatchCard = true;   
                                    }

                                    //send scan reply out each network interface
                                    scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                    scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                                    scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                    scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, true);

                                    try
                                    {
                                        scanSocket.Bind(new IPEndPoint(info.Address, 9999));
                                        scanSocket.SendTo(scanModules, 0, scanModules.Length, SocketFlags.None, mf.epModuleSet);
                                        if (mf.isUDPMonitorOn)
                                            mf.logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + mf.epModuleSet.ToString() + "\t" + " > " + data[3].ToString() + "\r\n");
                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write("Bind Error = ");
                                        Console.WriteLine(ex.ToString());
                                    }

                                    scanSocket.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write("nic Loop = ");
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }

            if (isSubnetMatchCard)
            {
                lblNetworkHelp.BackColor = System.Drawing.Color.LightGreen;
                lblNoAdapter.Visible = false;
            }
            else
            {
                lblNetworkHelp.BackColor = System.Drawing.Color.Salmon;
                lblNoAdapter.Visible = true;
            }
        }

        private void btnSendSubnet_Click(object sender, EventArgs e)
        {
            {
                sendIPToModules[7] = ipNew[0];
                sendIPToModules[8] = ipNew[1];
                sendIPToModules[9] = ipNew[2];

                //loop thru all interfaces
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.Supports(NetworkInterfaceComponent.IPv4) && nic.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (var info in nic.GetIPProperties().UnicastAddresses)
                        {
                            // Only InterNetwork and not loopback which have a subnetmask
                            if (info.Address.AddressFamily == AddressFamily.InterNetwork &&
                                !IPAddress.IsLoopback(info.Address) &&
                                info.IPv4Mask != null)
                            {
                                Socket scanSocket;
                                try
                                {
                                    if (nic.OperationalStatus == OperationalStatus.Up
                                        && info.IPv4Mask != null)
                                    {
                                        scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                        scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                                        scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                        scanSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, true);

                                        try
                                        {
                                            scanSocket.Bind(new IPEndPoint(info.Address, 9999));
                                            scanSocket.SendTo(sendIPToModules, 0, sendIPToModules.Length, SocketFlags.None, mf.epModuleSet);
                                            if (mf.isUDPMonitorOn)
                                                mf.logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + mf.epModuleSet.ToString() + "\t" + " > " + sendIPToModules[3].ToString() + "\r\n");

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.Write("Bind Error = ");
                                            Console.WriteLine(ex.ToString());
                                        }

                                        scanSocket.Dispose();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write("nic Loop = ");
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                        }
                    }
                }

                Properties.Settings.Default.etIP_SubnetOne = ipCurrent[0] = ipNew[0];
                Properties.Settings.Default.etIP_SubnetTwo = ipCurrent[1] = ipNew[1];
                Properties.Settings.Default.etIP_SubnetThree = ipCurrent[2] = ipNew[2];

                Properties.Settings.Default.Save();

                mf.epModule = new IPEndPoint(IPAddress.Parse(
                    Properties.Settings.Default.etIP_SubnetOne.ToString() + "." +
                    Properties.Settings.Default.etIP_SubnetTwo.ToString() + "." +
                    Properties.Settings.Default.etIP_SubnetThree.ToString() + ".255"), 8888);

                lblNetworkHelp.Text =
                    Properties.Settings.Default.etIP_SubnetOne.ToString() + " . " +
                    Properties.Settings.Default.etIP_SubnetTwo.ToString() + " . " +
                    Properties.Settings.Default.etIP_SubnetThree.ToString();
            }

            pboxSendSteer.Visible = false;
            btnSerialCancel.Image = Properties.Resources.back_button;
        }

        private void btnAutoSet_Click(object sender, EventArgs e)
        {
                nudFirstIP.Value = mf.scanReply.subnet[0];
                nudSecndIP.Value = mf.scanReply.subnet[1];
                nudThirdIP.Value = mf.scanReply.subnet[2];
                ipNew[0] = mf.scanReply.subnet[0];
                ipNew[1] = mf.scanReply.subnet[1];
                ipNew[2] = mf.scanReply.subnet[2];
                btnSerialCancel.Image = Properties.Resources.Cancel64;
                pboxSendSteer.Visible = true;
        }

        private void nudFirstIP_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            ipNew[0] = (byte)nudFirstIP.Value;
            ipNew[1] = (byte)nudSecndIP.Value;
            ipNew[2] = (byte)nudThirdIP.Value;
            btnSendSubnet.Enabled = true;
            pboxSendSteer.Visible = true;
            btnSerialCancel.Image = Properties.Resources.Cancel64;
        }

        private void cboxUp_Click(object sender, EventArgs e)
        {
            if(cboxUp.Checked)
            {
                cboxUp.Text = "Up";
            }
            else
            {
                cboxUp.Text = "Up + Down";
            }
        }

        private void btnNetworkCPL_Click(object sender, EventArgs e)
        {
            Process.Start("ncpa.cpl");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(gStr.gsEthernetHelp);

        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSerialMonitor_Click(object sender, EventArgs e)
        {
            mf.ShowSerialMonitor();
        }

        ////get the ipv4 address only
        //public void GetIP4AddressList()
        //{
        //    tboxNets.Text = "";
        //    foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
        //    {
        //        if (IPA.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            tboxNets.Text += IPA.ToString() + "\r\n";
        //        }
        //    }
        //}

        //public void IsValidNetworkFound()
        //{
        //    foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
        //    {
        //        if (IPA.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            byte[] data = IPA.GetAddressBytes();
        //            //  Split string by ".", check that array length is 3
        //            if (data[0] == 192 && data[1] == 168 && data[2] == 1)
        //            {
        //                if (data[3] < 255 && data[3] > 1)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
