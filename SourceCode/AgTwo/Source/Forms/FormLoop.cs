using AgTwo.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormLoop : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //key event to restore window
        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;

        //Stringbuilder
        public StringBuilder logNMEASentence = new StringBuilder();
        public StringBuilder logMonitorSentence = new StringBuilder();
        public StringBuilder logUDPSentence = new StringBuilder();
        public bool isLogNMEA, isLogMonitorOn, isUDPMonitorOn, isGPSLogOn, isNTRIPLogOn;

        private StringBuilder sbRTCM = new StringBuilder();

        public bool isKeyboardOn = true;

        public bool isNTRIPToggle = true, isSendToUDP = false;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        //timer variables
        public double secondsSinceStart, twoSecondTimer, tenSecondTimer, threeMinuteTimer;

        public string lastSentence;

        public bool isPluginUsed;

        //usually 256 - send ntrip to serial in chunks
        public int packetSizeNTRIP;

        public bool lastHelloGPS, lastHelloAutoSteer, lastHelloMachine, lastHelloNav;
        public bool isConnectedNav, isConnectedSteer, isConnectedMachine;

        //is the fly out displayed
        public bool isViewAdvanced = false;

        //used to hide the window and not update text fields and most counters
        public bool isAppInFocus = true, isLostFocus;
        public int focusSkipCounter = 121;

        //The base directory where Drive will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of Comm storage
        public string commDirectory, commFileName = "";

        public FormLoop()
        {
            InitializeComponent();
        }

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            if (Settings.Default.setF_workingDirectory == "Default")
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else baseDirectory = Settings.Default.setF_workingDirectory + "\\AgOpenGPS\\";

            //get the fields directory, if not exist, create
            commDirectory = baseDirectory + "AgTwo\\";
            string dir = Path.GetDirectoryName(commDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            if (Settings.Default.setUDP_isOn)
            {
                LoadUDPNetwork();
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label9.Visible = false;

                lblSteerAngle.Visible = false;
                lblWASCounts.Visible = false;
                lblSwitchStatus.Visible = false;
                lblWorkSwitchStatus.Visible = false;

                label10.Visible = false;
                label12.Visible = false;
                lbl1To8.Visible = false;
                lbl9To16.Visible = false;

                btnRelayTest.Visible = false;

                btnUDP.BackColor = Color.Gainsboro;
                lblIP.Text = "Off";
            }

            //small view
            this.Width = 370;

            LoadLoopback();

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;
            isPluginUsed = Properties.Settings.Default.setUDP_isUsePluginApp;

            isConnectedNav = cboxIsNavModule.Checked = Properties.Settings.Default.setMod_isNavConnected;
            isConnectedSteer = cboxIsSteerModule.Checked = Properties.Settings.Default.setMod_isSteerConnected;
            isConnectedMachine = cboxIsMachineModule.Checked = Properties.Settings.Default.setMod_isMachineConnected;
            
            SetModulesOnOff();

            oneSecondLoopTimer.Interval = 1000;
            oneSecondLoopTimer.Enabled = true;

            //On or off the module rows
            SetModulesOnOff();
        }

        public void SetModulesOnOff()
        {
            if (isConnectedNav)
            {
                btnNav.Visible = true; 
                cboxIsNavModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnNav.Visible = false;
                cboxIsNavModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedMachine)
            {
                btnMachine.Visible = true;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnMachine.Visible = false;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedSteer)
            {
                btnSteer.Visible = true;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnSteer.Visible = false;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.AddNew;
            }

            Properties.Settings.Default.setMod_isNavConnected = isConnectedNav;
            Properties.Settings.Default.setMod_isSteerConnected = isConnectedSteer;
            Properties.Settings.Default.setMod_isMachineConnected = isConnectedMachine;

            Properties.Settings.Default.Save();
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();

            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { loopBackSocket.Close(); }
            }

            if (UDPSocket != null)
            {
                try
                {
                    UDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { UDPSocket.Close(); }
            }
        }

        private void oneSecondLoopTimer_Tick(object sender, EventArgs e)
        {
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            #region Sleep

            //is this the active window
            isAppInFocus = FormLoop.ActiveForm != null;

            //start counting down to minimize
            if (!isAppInFocus && !isLostFocus)
            {
                focusSkipCounter = 121;
                isLostFocus = true;
            }

            // Is active window again
            if (isAppInFocus && isLostFocus)
            {
                isLostFocus = false;
                focusSkipCounter = int.MaxValue;
            }

            if (isLostFocus && focusSkipCounter !=0)
            {
                if (focusSkipCounter == 1)
                {
                    WindowState = FormWindowState.Minimized;
                }

                focusSkipCounter-- ;
            }

            if (focusSkipCounter != 0)
            {
                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }            

            #endregion

            //every couple or so seconds
            if ((secondsSinceStart - twoSecondTimer) > 1.5)
            {
                TwoSecondLoop();
                twoSecondTimer = secondsSinceStart;
            }

            //every 10 seconds
            if ((secondsSinceStart - tenSecondTimer) > 9.5)
            {
                TenSecondLoop();
                tenSecondTimer = secondsSinceStart;
            }

            //3 minute egg timer
            if ((secondsSinceStart - threeMinuteTimer) > 180)
            {
                ThreeMinuteLoop();
                threeMinuteTimer = secondsSinceStart;
            }

            // 1 Second Loop Part2 
            if (isViewAdvanced)
            {
                btnResetTimer.Text = ((int)(180 - (secondsSinceStart - threeMinuteTimer))).ToString();
            }
        }

        private void TwoSecondLoop()
        {
            //Hello Alarm logic
            DoHelloAlarmLogic();

            DoTraffic();

            //send a hello to modules
            SendUDPMessage(helloFromAgIO, epHello);


            if (isLogNMEA)
            {
                using (StreamWriter writer = new StreamWriter("zAgIO_log.txt", true))
                {
                    writer.Write(logNMEASentence.ToString());
                }
                logNMEASentence.Clear();
            }

            if (focusSkipCounter != 0)
            {
                if (focusSkipCounter < 121) lblSkipCounter.Text = focusSkipCounter.ToString();
                else lblSkipCounter.Text = "On";

                lblHDOP.Text = hdopData.ToString("N1");
            }
        }

        private void TenSecondLoop()
        {
            if (focusSkipCounter != 0 && WindowState == FormWindowState.Minimized)
            {
                focusSkipCounter = 0;
                isLostFocus = true;
            }

            if (focusSkipCounter != 0)
            {
                //update connections
                lblIP.Text = "";
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _ = IPA.ToString();
                        lblIP.Text += IPA.ToString() + "\r\n";
                    }
                }

            }
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (this.Width < 600)
            {
                this.Width = 750;
                isViewAdvanced = true;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnLeft;
                sbRTCM.Clear();
                threeMinuteTimer = secondsSinceStart;
            }
            else
            {
                this.Width = 370;
                isViewAdvanced = false;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnRight;
            }
        }

        private void ThreeMinuteLoop()
        {
            if (isViewAdvanced)
            {
                btnSlide.PerformClick();
            }
        }

        private void DoHelloAlarmLogic()
        {
            bool currentHello;

            if (isConnectedMachine)
            {
                currentHello = traffic.helloFromMachine < 3;

                if (currentHello != lastHelloMachine)
                {
                    if (currentHello) btnMachine.BackColor = Color.LimeGreen;
                    else btnMachine.BackColor = Color.Red;
                    lastHelloMachine = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedSteer)
            {
                currentHello = traffic.helloFromAutoSteer < 3;

                if (currentHello != lastHelloAutoSteer)
                {
                    if (currentHello) btnSteer.BackColor = Color.LimeGreen;
                    else btnSteer.BackColor = Color.Red;
                    lastHelloAutoSteer = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedNav)
            {
                currentHello = traffic.helloFromNav < 3;

                if (currentHello != lastHelloNav)
                {
                    if (currentHello) btnNav.BackColor = Color.LimeGreen;
                    else btnNav.BackColor = Color.Red;
                    lastHelloNav = currentHello;
                    ShowAgIO();
                }
            }

            currentHello = traffic.cntrGPSOut != 0;

            if (currentHello != lastHelloGPS)
            {
                if (currentHello) btnGPS.BackColor = Color.LimeGreen;
                else btnGPS.BackColor = Color.Red;
                lastHelloGPS = currentHello;
                ShowAgIO();
            }
        }

        private void FormLoop_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                if (isViewAdvanced) btnSlide.PerformClick();
                isLostFocus = true;
                focusSkipCounter = 0;
            }
        }

        private void ShowAgIO()
        {
            Process[] processName = Process.GetProcessesByName("AgTwo");
            
            if (processName.Length != 0)
            {
                // Guard: check if window already has focus.
                if (processName[0].MainWindowHandle == GetForegroundWindow()) return;

                // Show window maximized.
                ShowWindow(processName[0].MainWindowHandle, 9);

                // Simulate an "ALT" key press.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

                // Simulate an "ALT" key release.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

                // Show window in forground.
                SetForegroundWindow(processName[0].MainWindowHandle);
            }  
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;
            traffic.helloFromNav++;

            if (focusSkipCounter != 0)
            {
                lblFromGPS.Text = traffic.cntrGPSOut == 0 ? "---" : ((traffic.cntrGPSOut>>1)).ToString();

                //reset all counters
                traffic.cntrGPSOut = 0;

                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }
        }

        // Buttons, Checkboxes and Clicks  ***************************************************

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void cboxIsSteerModule_Click(object sender, EventArgs e)
        {
            isConnectedSteer = cboxIsSteerModule.Checked;
            SetModulesOnOff();  
        }

        private void cboxIsMachineModule_Click(object sender, EventArgs e)
        {
            isConnectedMachine = cboxIsMachineModule.Checked;
            SetModulesOnOff();
        }

        private void lblMessages_Click(object sender, EventArgs e)
        {
            sbRTCM.Clear();
            sbRTCM.Append("Reset..");
        }

        private void btnResetTimer_Click(object sender, EventArgs e)
        {
            threeMinuteTimer = secondsSinceStart;
        }

        private void btnRelayTest_Click(object sender, EventArgs e)
        {
                helloFromAgIO[7] = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void lblIP_Click(object sender, EventArgs e)
        {
            lblIP.Text = "";
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ = IPA.ToString();
                    lblIP.Text += IPA.ToString() + "\r\n";
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Save curent Settngs
            using (var form = new FormCommSaver(this))
            {
                form.ShowDialog(this);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Load new settings
            using (var form = new FormCommPicker(this))
            {
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.OK)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
            }
        }

        private void btnGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void toolStripEthernet_Click(object sender, EventArgs e)
        {
            SettingsEthernet();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(gStr.gsAgIOHelp);
        }

        private void cboxIsNavModule_Click(object sender, EventArgs e)
        {
            isConnectedNav = cboxIsNavModule.Checked;
            SetModulesOnOff();
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            if (!Settings.Default.setUDP_isOn) SettingsEthernet();
            else SettingsUDP();
        }

        private void btnRunAOG_Click(object sender, EventArgs e)
        {
            StartAOG();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void btnWindowsShutDown_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Shutdown Windows For Realz ?",
                "For Sure For Sure ?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private void toolStripGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void cboxLogNMEA_CheckedChanged(object sender, EventArgs e)
        {
            isLogNMEA = cboxLogNMEA.Checked;
        }

    }
}

