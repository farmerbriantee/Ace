﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;

namespace AgIO
{
    public partial class FormLoop
    {
        //for the NTRIP CLient counting
        private int ntripCounter = 10;

        private Socket clientSocket;                      // Server connection
        private byte[] casterRecBuffer = new byte[2800];    // Recieved data buffer

        //Send GGA back timer
        Timer tmr;

        private string mount;
        private string username;
        private string password;

        private string broadCasterIP;
        private int broadCasterPort;

        private int sendGGAInterval = 0;
        private string GGASentence;

        public uint tripBytes = 0, tripCounts = 0;
        private int toUDP_Port = 0;
        private int NTRIP_Watchdog = 100;

        public bool isNTRIP_RequiredOn = false;
        public bool isNTRIP_Connected = false;
        public bool isNTRIP_Starting = false;
        public bool isNTRIP_Connecting = false;
        public bool isNTRIP_Sending = false;
        public bool isRunGGAInterval = false;

        List<int> rList = new List<int>();
        List<int> aList = new List<int>();

        //set up connection to Caster
        private void DoNTRIPSecondRoutine()
        {
            //count up the ntrip clock only if everything is alive
            if (isNTRIP_RequiredOn)
            {
                IncrementNTRIPWatchDog();
            }

            //Have we NTRIP connection
            if (isNTRIP_RequiredOn && !isNTRIP_Connected && !isNTRIP_Connecting)
            {
                if (!isNTRIP_Starting && ntripCounter > 20)
                {
                    StartNTRIP();
                }
            }

            if (isNTRIP_Connecting)
            {
                if (ntripCounter > 29)
                {
                    TimedMessageBox(1500, "Connection Problem", "Not Connecting To Caster");
                    ReconnectRequest();
                }
                if (clientSocket != null && clientSocket.Connected)
                {
                    SendAuthorization();
                }
            }

            if (focusSkipCounter != 0 && isNTRIP_RequiredOn)
            {
                //pbarNtripMenu.Value = unchecked((byte)(tripBytes * 0.02));
                lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

                //update byte counter and up counter
                if (ntripCounter > 59) btnStartStopNtrip.Text = (ntripCounter >> 6) + " Min";
                else if (ntripCounter < 60 && ntripCounter > 22) btnStartStopNtrip.Text = ntripCounter + " Secs";
                else btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 22)) + " secs";

                //watchdog for Ntrip
                if (isNTRIP_Connecting)
                {
                    lblWatch.Text = gStr.gsAuthourizing;
                }
                else
                {
                    if (isNTRIP_RequiredOn && NTRIP_Watchdog > 10)
                    {
                        lblWatch.Text = gStr.gsWaiting;
                    }
                    else
                    {
                        lblWatch.Text = gStr.gsListening;

                        if (isNTRIP_RequiredOn)
                        {
                            lblWatch.Text += " NTRIP";
                        }
                    }
                }

                if (sendGGAInterval > 0 && isNTRIP_Sending)
                {
                    lblWatch.Text = "Send GGA";
                    isNTRIP_Sending = false;
                }
            }
        }

        public void ConfigureNTRIP()
        {
            lblWatch.Text = "Wait GPS";
            lblMessages.Text = "Reading...";
            lblNTRIP_IP.Text = "";
            lblMount.Text = "";

            aList.Clear();
            rList.Clear();
            lblMessages.Text = "Reading....";

            //start NTRIP if required
            isNTRIP_RequiredOn = Properties.Settings.Default.setNTRIP_isOn;

            if (isNTRIP_RequiredOn)
            {
                btnStartStopNtrip.Visible = true;
                btnStartStopNtrip.Visible = true;
                lblWatch.Visible = true;
                lblNTRIPBytes.Visible = true;
                lblToGPS.Visible = true;
                lblMount.Visible = true;
                lblNTRIP_IP.Visible = true;
            }
            else
            {
                btnStartStopNtrip.Visible = false;
                btnStartStopNtrip.Visible = false;
                lblWatch.Visible = false;
                lblNTRIPBytes.Visible = false;
                lblToGPS.Visible = false;
                lblMount.Visible = false;
                lblNTRIP_IP.Visible = false;
            }

            btnStartStopNtrip.Text = "Off";
        }

        public void StartNTRIP()
        {
            if (isNTRIP_RequiredOn)
            {
                if (isLostFocus) ShowAgIO();                
                
                broadCasterIP = null;
                string actualIP = Properties.Settings.Default.setNTRIP_casterURL.Trim();

                try
                {
                    IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                    foreach (IPAddress address in addresslist)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            broadCasterIP = address.ToString().Trim();
                            break;
                        }
                    }

                    if (broadCasterIP == null) throw new NullReferenceException();
                }
                catch (Exception)
                {
                    TimedMessageBox(1500, "IP Not Located, Network Down?", "Cannot Find: " + Properties.Settings.Default.setNTRIP_casterURL);
                    //if we had a timer already, kill it
                    if (tmr != null)
                    {
                        tmr.Dispose();
                    }

                    // Close the socket if it is still open
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(100);
                        clientSocket.Close();
                    }

                    //TimedMessageBox(2000, "NTRIP Not Connected", " Reconnect Request");
                    ntripCounter = 15;
                    isNTRIP_Connected = false;
                    isNTRIP_Starting = false;
                    isNTRIP_Connecting = false;
                    return;
                }

                broadCasterPort = Properties.Settings.Default.setNTRIP_casterPort; //Select correct port (usually 80 or 2101)
                mount = Properties.Settings.Default.setNTRIP_mount; //Insert the correct mount
                username = Properties.Settings.Default.setNTRIP_userName; //Insert your username!
                password = Properties.Settings.Default.setNTRIP_userPassword; //Insert your password!
                toUDP_Port = Properties.Settings.Default.setNTRIP_sendToUDPPort; //send rtcm to which udp port
                sendGGAInterval = Properties.Settings.Default.setNTRIP_sendGGAInterval; //how often to send fixes

                //if we had a timer already, kill it
                if (tmr != null)
                {
                    tmr.Dispose();
                }

                //create new timer at fast rate to start
                if (sendGGAInterval > 0)
                {
                    this.tmr = new System.Windows.Forms.Timer();
                    this.tmr.Interval = 5000;
                    this.tmr.Tick += new EventHandler(NTRIPtick);
                }

                try
                {
                    // Close the socket if it is still open
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(100);
                        clientSocket.Close();
                    }

                    //NTRIP endpoint
                    epNtrip = new IPEndPoint(IPAddress.Parse(
                        Properties.Settings.Default.etIP_SubnetOne.ToString() + "." +
                        Properties.Settings.Default.etIP_SubnetTwo.ToString() + "." +
                        Properties.Settings.Default.etIP_SubnetThree.ToString() + ".255"), toUDP_Port);

                    // Create the socket object
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // Connect to server non-Blocking method
                    clientSocket.Blocking = false;
                    clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(broadCasterIP), broadCasterPort), new AsyncCallback(OnConnect), null);
                }
                catch (Exception)
                {
                    ReconnectRequest();
                    return;
                }

                isNTRIP_Connecting = true;
                lblNTRIP_IP.Text = broadCasterIP;
                lblMount.Text = mount;
            }
        }

        private void ReconnectRequest()
        {
            //TimedMessageBox(2000, "NTRIP Not Connected", " Reconnect Request");
            ntripCounter = 15;
            isNTRIP_Connected = false;
            isNTRIP_Starting = false;
            isNTRIP_Connecting = false;

            //if we had a timer already, kill it
            if (tmr != null)
            {
                tmr.Dispose();
            }

            ShowAgIO();
        }

        private void IncrementNTRIPWatchDog()
        {
            //increment once every second
            ntripCounter++;

            //Thinks is connected but not receiving anything
            if (NTRIP_Watchdog++ > 20 && isNTRIP_Connected) 
                ReconnectRequest();

            //Once all connected set the timer GGA to NTRIP Settings
            if (sendGGAInterval > 0 && ntripCounter == 40) tmr.Interval = sendGGAInterval * 1000;
        }

        private void SendAuthorization()
        {
            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                //TimedMessageBox(2000, gStr.gsNTRIPNotConnected, " At the StartNTRIP() ");
                ReconnectRequest();
                return;
            }

            // Read the message from settings and send it
            try
            {
                if (!Properties.Settings.Default.setNTRIP_isTCP)
                {
                    //encode user and password
                    string auth = ToBase64(username + ":" + password);

                    //grab location sentence
                    BuildGGA();
                    GGASentence = sbGGA.ToString();

                    string htt;
                    if (Properties.Settings.Default.setNTRIP_isHTTP10) htt = "1.0";
                    else htt = "1.1";

                    //Build authorization string
                    string str = "GET /" + mount + " HTTP/" + htt + "\r\n";
                    str += "User-Agent: NTRIP AgOpenGPSClient/20221020\r\n";
                    str += "Authorization: Basic " + auth + "\r\n"; //This line can be removed if no authorization is needed
                                                                    //str += GGASentence; //this line can be removed if no position feedback is needed
                    str += "Accept: */*\r\nConnection: close\r\n";
                    str += "\r\n";

                    // Convert to byte array and send.
                    Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                    clientSocket.Send(byteDateLine, byteDateLine.Length, 0);

                    //enable to periodically send GGA sentence to server.
                    if (sendGGAInterval > 0) tmr.Enabled = true;
                }
                //say its connected
                isNTRIP_Connected = true;
                isNTRIP_Starting = false;
                isNTRIP_Connecting = false;
            }
            catch (Exception)
            {
                ReconnectRequest();
            }
        }

        //intial connection only
        public void OnConnect(IAsyncResult ar)
        {
            // Check if we were sucessfull
            try
            {
                if (clientSocket.Connected)
                    clientSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceivedData), null);
            }
            catch (Exception)
            {
                ReconnectRequest();
            }
        }

        //called when data received
        public void OnReceivedData(IAsyncResult ar)
        {
            // Check if we got any data
            try
            {
                int nBytesRec = clientSocket.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    byte[] localMsg = new byte[nBytesRec];
                    Array.Copy(casterRecBuffer, localMsg, nBytesRec);

                    BeginInvoke((MethodInvoker)(() => OnAddMessage(localMsg)));
                    clientSocket.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceivedData), null);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    //Console.WriteLine("Client {0}, disconnected", clientSocket.RemoteEndPoint);
                    //clientSocket.Shutdown(SocketShutdown.Both);
                    //clientSocket.Close();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show( this, ex.Message, "Unusual error druing Recieve!" );
            }
        }

        public void OnAddMessage(byte[] data)
        {
            //send it
            SendUDPMessage(data, epNtrip);

            //update gui with stats
            tripBytes += (uint)data.Length;
            
            if (isViewAdvanced && isNTRIP_RequiredOn)
            {
                int mess = 0;
                //lblPacketSize.Text = data.Length.ToString();

                try
                {
                    lblStationID.Text = (((data[4] & 15) << 8) + (data[5])).ToString();

                    for (int i = 0; i < data.Length - 5; i++)
                    {

                        if (data[i] == 211 && (data[i + 1] >> 2) == 0)
                        {
                            mess = ((data[i + 3] << 4) + (data[i + 4] >> 4));
                            if (mess > 1000 && mess < 1231)
                            {
                                rList.Add(mess);
                                i += (data[i + 1] << 6) + (data[i + 2]) + 5;
                                if (data[i + 1] != 211)
                                {
                                    //rList.Clear();
                                    //break;
                                }
                            }
                            else
                            {
                                rList.Clear();
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    //MessageBox.Show("Error");
                }
            }

            //reset watchdog since we have updated data
            NTRIP_Watchdog = 0;

            if (focusSkipCounter != 0)
            {
                tripCounts += (uint)data.Length;
                if (tripCounts > 9999) tripCounts -= 10000;

                lblToGPS.Text = tripCounts.ToString();

                if (isNTRIPLogOn && epNtrip != null)
                {
                    logUDPSentence.Append(DateTime.Now.ToString("ss.fff\t") + epNtrip.ToString() + "\t" + " > NTRIP\r\n");
                }
            }
        }

        public void SendGGA()
        {
            //timer may have brought us here so return if not connected
            if (!isNTRIP_Connected)
                return;
            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                ReconnectRequest();
                return;
            }

            // Read the message from the text box and send it
            try
            {
                isNTRIP_Sending = true;
                BuildGGA();
                string str = sbGGA.ToString();

                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                clientSocket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception)
            {
                ReconnectRequest();
            }
        }
        private void NTRIPtick(object o, EventArgs e)
        {
            SendGGA();
        }

        private string ToBase64(string str)
        {
            Encoding asciiEncoding = Encoding.ASCII;
            byte[] byteArray = new byte[asciiEncoding.GetByteCount(str)];
            byteArray = asciiEncoding.GetBytes(str);
            return Convert.ToBase64String(byteArray, 0, byteArray.Length);
        }

        private void ShutDownNTRIP()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                //shut it down
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                System.Threading.Thread.Sleep(500);

                //start it up again
                ReconnectRequest();

                //Also stop the requests now
                isNTRIP_RequiredOn = false;
            }
        }

        private void SettingsShutDownNTRIP()
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                System.Threading.Thread.Sleep(500);
                ReconnectRequest();
            }
        }

        //calculate the NMEA checksum to stuff at the end
        public string CalculateChecksum(string Sentence)
        {
            int sum = 0, inx;
            char[] sentence_chars = Sentence.ToCharArray();
            char tmp;

            // All character xor:ed results in the trailing hex checksum
            // The checksum calc starts after '$' and ends before '*'
            for (inx = 1; ; inx++)
            {
                tmp = sentence_chars[inx];

                // Indicates end of data and start of checksum
                if (tmp == '*')
                    break;
                sum ^= tmp;    // Build checksum
            }

            // Calculated checksum converted to a 2 digit hex string
            return String.Format("{0:X2}", sum);
        }

        private readonly StringBuilder sbGGA = new StringBuilder();

        private void BuildGGA()
        {
            double latitude = 0;
            double longitude = 0;

            if (Properties.Settings.Default.setNTRIP_isGGAManual)
            {
                latitude = Properties.Settings.Default.setNTRIP_manualLat;
                longitude = Properties.Settings.Default.setNTRIP_manualLon;
            }
            else
            {
                latitude = this.latitude;
                longitude = this.longitude;
            }

            //convert to DMS from Degrees
            double latMinu = latitude;
            double longMinu = longitude;

            double latDeg = (int)latitude;
            double longDeg = (int)longitude;

            latMinu -= latDeg;
            longMinu -= longDeg;

            latMinu = Math.Round(latMinu * 60.0, 7);
            longMinu = Math.Round(longMinu * 60.0, 7);

            latDeg *= 100.0;
            longDeg *= 100.0;

            double latNMEA = latMinu + latDeg;
            double longNMEA = longMinu + longDeg;

            char NS = 'W';
            char EW = 'N';
            if (latitude >= 0) NS = 'N';
            else NS = 'S';
            if (longitude >= 0) EW = 'E';
            else EW = 'W';

            //sbGGA.Clear();
            //sbGGA.Append("$GPGGA,");
            //sbGGA.Append(DateTime.Now.ToString("HHmmss.00,", CultureInfo.InvariantCulture));
            //sbGGA.Append(Math.Abs(latNMEA).ToString("0000.000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            //sbGGA.Append(Math.Abs(longNMEA).ToString("00000.000", CultureInfo.InvariantCulture)).Append(',').Append(EW);
            //sbGGA.Append(",1,10,1,43.4,M,46.4,M,5,0*");

            //sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
            //sbGGA.Append("\r\n");
            sbGGA.Clear();
            sbGGA.Append("$GPGGA,");
            sbGGA.Append(DateTime.Now.ToString("HHmmss.00,", CultureInfo.InvariantCulture));
            sbGGA.Append(Math.Abs(latNMEA).ToString("0000.000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbGGA.Append(Math.Abs(longNMEA).ToString("00000.000", CultureInfo.InvariantCulture)).Append(',').Append(EW);
            sbGGA.Append(',').Append(fixQualityData.ToString()).Append(',');
            sbGGA.Append(satellitesData.ToString()).Append(',');

            if (hdopData > 0) sbGGA.Append(hdopData.ToString("0.##", CultureInfo.InvariantCulture)).Append(',');

            else sbGGA.Append("1,");

            sbGGA.Append(altitudeData.ToString("#.###", CultureInfo.InvariantCulture)).Append(',');
            sbGGA.Append("M,");
            sbGGA.Append("46.4,M,");  //udulation
            sbGGA.Append(ageData.ToString("0.#", CultureInfo.InvariantCulture)).Append(','); //age
            sbGGA.Append("0*");

            sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
            sbGGA.Append("\r\n");
            /*
        $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,5,0*47
           0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
                Time      Lat       Lon     FixSatsOP Alt */
        }
    }
}