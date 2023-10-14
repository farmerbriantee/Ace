
namespace AgTwo
{
    partial class FormLoop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoop));
            this.oneSecondLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCurentLon = new System.Windows.Forms.Label();
            this.lblCurrentLat = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripUDPMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSerialMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripEthernet = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSteerAngle = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblWASCounts = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSwitchStatus = new System.Windows.Forms.Label();
            this.lblWorkSwitchStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl1To8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl9To16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSkipCounter = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblToGPS = new System.Windows.Forms.Label();
            this.lblHDOP = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnResetTimer = new System.Windows.Forms.Button();
            this.cboxIsSteerModule = new System.Windows.Forms.CheckBox();
            this.cboxIsNavModule = new System.Windows.Forms.CheckBox();
            this.cboxIsMachineModule = new System.Windows.Forms.CheckBox();
            this.btnNav = new System.Windows.Forms.Button();
            this.btnSteer = new System.Windows.Forms.Button();
            this.btnMachine = new System.Windows.Forms.Button();
            this.btnGPS = new System.Windows.Forms.Button();
            this.btnWindowsShutDown = new System.Windows.Forms.Button();
            this.btnSlide = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cboxLogNMEA = new System.Windows.Forms.CheckBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRunAOG = new System.Windows.Forms.Button();
            this.btnRelayTest = new System.Windows.Forms.Button();
            this.btnUDP = new System.Windows.Forms.Button();
            this.btnGPSData = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // oneSecondLoopTimer
            // 
            this.oneSecondLoopTimer.Interval = 1500;
            this.oneSecondLoopTimer.Tick += new System.EventHandler(this.oneSecondLoopTimer_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(6, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 151;
            this.label6.Text = "Lat";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 31);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 16);
            this.label8.TabIndex = 152;
            this.label8.Text = "Lon";
            // 
            // lblCurentLon
            // 
            this.lblCurentLon.AutoSize = true;
            this.lblCurentLon.BackColor = System.Drawing.Color.Transparent;
            this.lblCurentLon.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurentLon.ForeColor = System.Drawing.Color.White;
            this.lblCurentLon.Location = new System.Drawing.Point(30, 30);
            this.lblCurentLon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurentLon.Name = "lblCurentLon";
            this.lblCurentLon.Size = new System.Drawing.Size(98, 18);
            this.lblCurentLon.TabIndex = 154;
            this.lblCurentLon.Text = "-888.8888888";
            // 
            // lblCurrentLat
            // 
            this.lblCurrentLat.AutoSize = true;
            this.lblCurrentLat.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentLat.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLat.ForeColor = System.Drawing.Color.White;
            this.lblCurrentLat.Location = new System.Drawing.Point(30, 8);
            this.lblCurrentLat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentLat.Name = "lblCurrentLat";
            this.lblCurrentLat.Size = new System.Drawing.Size(90, 18);
            this.lblCurrentLat.TabIndex = 153;
            this.lblCurrentLat.Text = "-53.1234567";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.AutoSize = true;
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.Color.White;
            this.lblFromGPS.Location = new System.Drawing.Point(280, 275);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(26, 18);
            this.lblFromGPS.TabIndex = 130;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(86, 410);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(188, 70);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 149;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripUDPMonitor,
            this.toolStripSerialMonitor,
            this.toolStripEthernet,
            this.deviceManagerToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::AgTwo.Properties.Resources.Settings48;
            this.toolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(90, 68);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.Image = global::AgTwo.Properties.Resources.VehFileSave;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(296, 70);
            this.toolStripMenuItem1.Text = "Save";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem2.Image = global::AgTwo.Properties.Resources.VehFileLoad;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(296, 70);
            this.toolStripMenuItem2.Text = "Load";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripUDPMonitor
            // 
            this.toolStripUDPMonitor.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripUDPMonitor.Image = global::AgTwo.Properties.Resources.ScanNetwork;
            this.toolStripUDPMonitor.Name = "toolStripUDPMonitor";
            this.toolStripUDPMonitor.Size = new System.Drawing.Size(296, 70);
            this.toolStripUDPMonitor.Text = "UDP Monitor";
            this.toolStripUDPMonitor.Click += new System.EventHandler(this.toolStripUDPMonitor_Click);
            // 
            // toolStripSerialMonitor
            // 
            this.toolStripSerialMonitor.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSerialMonitor.Image = global::AgTwo.Properties.Resources.SerialMonitor;
            this.toolStripSerialMonitor.Name = "toolStripSerialMonitor";
            this.toolStripSerialMonitor.Size = new System.Drawing.Size(296, 70);
            this.toolStripSerialMonitor.Text = "Serial Monitor";
            this.toolStripSerialMonitor.Click += new System.EventHandler(this.toolStripSerialMonitor_Click);
            // 
            // toolStripEthernet
            // 
            this.toolStripEthernet.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripEthernet.Image = global::AgTwo.Properties.Resources.EthernetSetup;
            this.toolStripEthernet.Name = "toolStripEthernet";
            this.toolStripEthernet.Size = new System.Drawing.Size(296, 70);
            this.toolStripEthernet.Text = "Ethernet Setup";
            this.toolStripEthernet.Click += new System.EventHandler(this.toolStripEthernet_Click);
            // 
            // deviceManagerToolStripMenuItem
            // 
            this.deviceManagerToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceManagerToolStripMenuItem.Image = global::AgTwo.Properties.Resources.DeviceManager;
            this.deviceManagerToolStripMenuItem.Name = "deviceManagerToolStripMenuItem";
            this.deviceManagerToolStripMenuItem.Size = new System.Drawing.Size(296, 70);
            this.deviceManagerToolStripMenuItem.Text = "Device Manager";
            this.deviceManagerToolStripMenuItem.Click += new System.EventHandler(this.deviceManagerToolStripMenuItem_Click);
            // 
            // lblSteerAngle
            // 
            this.lblSteerAngle.AutoSize = true;
            this.lblSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngle.ForeColor = System.Drawing.Color.White;
            this.lblSteerAngle.Location = new System.Drawing.Point(523, 85);
            this.lblSteerAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSteerAngle.Name = "lblSteerAngle";
            this.lblSteerAngle.Size = new System.Drawing.Size(40, 18);
            this.lblSteerAngle.TabIndex = 473;
            this.lblSteerAngle.Text = "UDP";
            // 
            // lblIP
            // 
            this.lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIP.BackColor = System.Drawing.Color.Transparent;
            this.lblIP.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.ForeColor = System.Drawing.Color.White;
            this.lblIP.Location = new System.Drawing.Point(6, 221);
            this.lblIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(144, 102);
            this.lblIP.TabIndex = 464;
            this.lblIP.Text = "288.288.288.288";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblIP.Click += new System.EventHandler(this.lblIP_Click);
            // 
            // lblWASCounts
            // 
            this.lblWASCounts.AutoSize = true;
            this.lblWASCounts.BackColor = System.Drawing.Color.Transparent;
            this.lblWASCounts.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWASCounts.ForeColor = System.Drawing.Color.White;
            this.lblWASCounts.Location = new System.Drawing.Point(523, 108);
            this.lblWASCounts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWASCounts.Name = "lblWASCounts";
            this.lblWASCounts.Size = new System.Drawing.Size(43, 18);
            this.lblWASCounts.TabIndex = 476;
            this.lblWASCounts.Text = "Only";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(470, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 19);
            this.label3.TabIndex = 477;
            this.label3.Text = "Angle:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(462, 107);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 19);
            this.label4.TabIndex = 478;
            this.label4.Text = "Counts:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(462, 142);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 481;
            this.label2.Text = "Switch:";
            // 
            // lblSwitchStatus
            // 
            this.lblSwitchStatus.AutoSize = true;
            this.lblSwitchStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblSwitchStatus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSwitchStatus.ForeColor = System.Drawing.Color.White;
            this.lblSwitchStatus.Location = new System.Drawing.Point(523, 143);
            this.lblSwitchStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSwitchStatus.Name = "lblSwitchStatus";
            this.lblSwitchStatus.Size = new System.Drawing.Size(18, 18);
            this.lblSwitchStatus.TabIndex = 482;
            this.lblSwitchStatus.Text = "*";
            // 
            // lblWorkSwitchStatus
            // 
            this.lblWorkSwitchStatus.AutoSize = true;
            this.lblWorkSwitchStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkSwitchStatus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkSwitchStatus.ForeColor = System.Drawing.Color.White;
            this.lblWorkSwitchStatus.Location = new System.Drawing.Point(523, 166);
            this.lblWorkSwitchStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkSwitchStatus.Name = "lblWorkSwitchStatus";
            this.lblWorkSwitchStatus.Size = new System.Drawing.Size(18, 18);
            this.lblWorkSwitchStatus.TabIndex = 484;
            this.lblWorkSwitchStatus.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(471, 165);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 19);
            this.label9.TabIndex = 483;
            this.label9.Text = "Work:";
            // 
            // lbl1To8
            // 
            this.lbl1To8.AutoSize = true;
            this.lbl1To8.BackColor = System.Drawing.Color.Transparent;
            this.lbl1To8.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1To8.ForeColor = System.Drawing.Color.White;
            this.lbl1To8.Location = new System.Drawing.Point(460, 228);
            this.lbl1To8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl1To8.Name = "lbl1To8";
            this.lbl1To8.Size = new System.Drawing.Size(106, 23);
            this.lbl1To8.TabIndex = 500;
            this.lbl1To8.Text = "00000000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(460, 208);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 19);
            this.label10.TabIndex = 499;
            this.label10.Text = "8     <<      1";
            // 
            // lbl9To16
            // 
            this.lbl9To16.AutoSize = true;
            this.lbl9To16.BackColor = System.Drawing.Color.Transparent;
            this.lbl9To16.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl9To16.ForeColor = System.Drawing.Color.White;
            this.lbl9To16.Location = new System.Drawing.Point(460, 280);
            this.lbl9To16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl9To16.Name = "lbl9To16";
            this.lbl9To16.Size = new System.Drawing.Size(106, 23);
            this.lbl9To16.TabIndex = 502;
            this.lbl9To16.Text = "00000000";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(460, 259);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 19);
            this.label12.TabIndex = 501;
            this.label12.Text = "16    <<     9";
            // 
            // lblSkipCounter
            // 
            this.lblSkipCounter.AutoSize = true;
            this.lblSkipCounter.BackColor = System.Drawing.Color.Transparent;
            this.lblSkipCounter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkipCounter.ForeColor = System.Drawing.Color.White;
            this.lblSkipCounter.Location = new System.Drawing.Point(30, 53);
            this.lblSkipCounter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkipCounter.Name = "lblSkipCounter";
            this.lblSkipCounter.Size = new System.Drawing.Size(28, 16);
            this.lblSkipCounter.TabIndex = 509;
            this.lblSkipCounter.Text = "285";
            this.lblSkipCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(3, 53);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 16);
            this.label7.TabIndex = 511;
            this.label7.Text = "Min";
            // 
            // lblToGPS
            // 
            this.lblToGPS.AutoSize = true;
            this.lblToGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblToGPS.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToGPS.ForeColor = System.Drawing.Color.White;
            this.lblToGPS.Location = new System.Drawing.Point(280, 248);
            this.lblToGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblToGPS.Name = "lblToGPS";
            this.lblToGPS.Size = new System.Drawing.Size(26, 18);
            this.lblToGPS.TabIndex = 512;
            this.lblToGPS.Text = "---";
            this.lblToGPS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHDOP
            // 
            this.lblHDOP.BackColor = System.Drawing.Color.Transparent;
            this.lblHDOP.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHDOP.ForeColor = System.Drawing.Color.White;
            this.lblHDOP.Location = new System.Drawing.Point(151, 260);
            this.lblHDOP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHDOP.Name = "lblHDOP";
            this.lblHDOP.Size = new System.Drawing.Size(48, 18);
            this.lblHDOP.TabIndex = 523;
            this.lblHDOP.Text = "-";
            this.lblHDOP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnHelp
            // 
            this.btnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnHelp.Image = global::AgTwo.Properties.Resources.Help;
            this.btnHelp.Location = new System.Drawing.Point(222, 14);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(43, 32);
            this.btnHelp.TabIndex = 522;
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnResetTimer
            // 
            this.btnResetTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetTimer.BackColor = System.Drawing.Color.Transparent;
            this.btnResetTimer.BackgroundImage = global::AgTwo.Properties.Resources.ResetTimer;
            this.btnResetTimer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnResetTimer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnResetTimer.FlatAppearance.BorderSize = 0;
            this.btnResetTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetTimer.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetTimer.ForeColor = System.Drawing.Color.White;
            this.btnResetTimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetTimer.Location = new System.Drawing.Point(402, 415);
            this.btnResetTimer.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetTimer.Name = "btnResetTimer";
            this.btnResetTimer.Size = new System.Drawing.Size(63, 58);
            this.btnResetTimer.TabIndex = 504;
            this.btnResetTimer.Text = "179";
            this.btnResetTimer.UseVisualStyleBackColor = false;
            this.btnResetTimer.Click += new System.EventHandler(this.btnResetTimer_Click);
            // 
            // cboxIsSteerModule
            // 
            this.cboxIsSteerModule.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSteerModule.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsSteerModule.BackgroundImage = global::AgTwo.Properties.Resources.AddNew;
            this.cboxIsSteerModule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxIsSteerModule.Checked = true;
            this.cboxIsSteerModule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsSteerModule.FlatAppearance.BorderSize = 0;
            this.cboxIsSteerModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboxIsSteerModule.Location = new System.Drawing.Point(355, 170);
            this.cboxIsSteerModule.Name = "cboxIsSteerModule";
            this.cboxIsSteerModule.Size = new System.Drawing.Size(26, 27);
            this.cboxIsSteerModule.TabIndex = 498;
            this.cboxIsSteerModule.UseVisualStyleBackColor = false;
            this.cboxIsSteerModule.Click += new System.EventHandler(this.cboxIsSteerModule_Click);
            // 
            // cboxIsNavModule
            // 
            this.cboxIsNavModule.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsNavModule.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsNavModule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cboxIsNavModule.BackgroundImage")));
            this.cboxIsNavModule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxIsNavModule.Checked = true;
            this.cboxIsNavModule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsNavModule.FlatAppearance.BorderSize = 0;
            this.cboxIsNavModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboxIsNavModule.Location = new System.Drawing.Point(355, 88);
            this.cboxIsNavModule.Name = "cboxIsNavModule";
            this.cboxIsNavModule.Size = new System.Drawing.Size(26, 27);
            this.cboxIsNavModule.TabIndex = 496;
            this.cboxIsNavModule.UseVisualStyleBackColor = false;
            this.cboxIsNavModule.Click += new System.EventHandler(this.cboxIsNavModule_Click);
            // 
            // cboxIsMachineModule
            // 
            this.cboxIsMachineModule.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsMachineModule.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsMachineModule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cboxIsMachineModule.BackgroundImage")));
            this.cboxIsMachineModule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxIsMachineModule.Checked = true;
            this.cboxIsMachineModule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsMachineModule.FlatAppearance.BorderSize = 0;
            this.cboxIsMachineModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboxIsMachineModule.Location = new System.Drawing.Point(355, 343);
            this.cboxIsMachineModule.Name = "cboxIsMachineModule";
            this.cboxIsMachineModule.Size = new System.Drawing.Size(26, 27);
            this.cboxIsMachineModule.TabIndex = 495;
            this.cboxIsMachineModule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cboxIsMachineModule.UseVisualStyleBackColor = false;
            this.cboxIsMachineModule.Click += new System.EventHandler(this.cboxIsMachineModule_Click);
            // 
            // btnNav
            // 
            this.btnNav.BackColor = System.Drawing.Color.Transparent;
            this.btnNav.BackgroundImage = global::AgTwo.Properties.Resources.Nav;
            this.btnNav.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNav.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnNav.FlatAppearance.BorderSize = 0;
            this.btnNav.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNav.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNav.ForeColor = System.Drawing.Color.White;
            this.btnNav.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNav.Location = new System.Drawing.Point(197, 76);
            this.btnNav.Margin = new System.Windows.Forms.Padding(4);
            this.btnNav.Name = "btnNav";
            this.btnNav.Size = new System.Drawing.Size(75, 50);
            this.btnNav.TabIndex = 185;
            this.btnNav.UseVisualStyleBackColor = false;
            // 
            // btnSteer
            // 
            this.btnSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnSteer.BackgroundImage = global::AgTwo.Properties.Resources.B_Autosteer;
            this.btnSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSteer.FlatAppearance.BorderSize = 0;
            this.btnSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteer.ForeColor = System.Drawing.Color.White;
            this.btnSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSteer.Location = new System.Drawing.Point(197, 161);
            this.btnSteer.Margin = new System.Windows.Forms.Padding(4);
            this.btnSteer.Name = "btnSteer";
            this.btnSteer.Size = new System.Drawing.Size(75, 50);
            this.btnSteer.TabIndex = 189;
            this.btnSteer.UseVisualStyleBackColor = false;
            // 
            // btnMachine
            // 
            this.btnMachine.BackColor = System.Drawing.Color.Transparent;
            this.btnMachine.BackgroundImage = global::AgTwo.Properties.Resources.B_Machine;
            this.btnMachine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMachine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMachine.FlatAppearance.BorderSize = 0;
            this.btnMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMachine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachine.ForeColor = System.Drawing.Color.White;
            this.btnMachine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMachine.Location = new System.Drawing.Point(197, 331);
            this.btnMachine.Margin = new System.Windows.Forms.Padding(4);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(75, 50);
            this.btnMachine.TabIndex = 188;
            this.btnMachine.UseVisualStyleBackColor = false;
            // 
            // btnGPS
            // 
            this.btnGPS.BackColor = System.Drawing.Color.Transparent;
            this.btnGPS.BackgroundImage = global::AgTwo.Properties.Resources.B_GPS;
            this.btnGPS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGPS.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnGPS.FlatAppearance.BorderSize = 0;
            this.btnGPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPS.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPS.ForeColor = System.Drawing.Color.White;
            this.btnGPS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPS.Location = new System.Drawing.Point(197, 246);
            this.btnGPS.Margin = new System.Windows.Forms.Padding(4);
            this.btnGPS.Name = "btnGPS";
            this.btnGPS.Size = new System.Drawing.Size(75, 50);
            this.btnGPS.TabIndex = 187;
            this.btnGPS.UseVisualStyleBackColor = false;
            // 
            // btnWindowsShutDown
            // 
            this.btnWindowsShutDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWindowsShutDown.BackColor = System.Drawing.Color.Transparent;
            this.btnWindowsShutDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnWindowsShutDown.FlatAppearance.BorderSize = 0;
            this.btnWindowsShutDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWindowsShutDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWindowsShutDown.ForeColor = System.Drawing.Color.White;
            this.btnWindowsShutDown.Image = global::AgTwo.Properties.Resources.WindowsShutDown;
            this.btnWindowsShutDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnWindowsShutDown.Location = new System.Drawing.Point(665, 415);
            this.btnWindowsShutDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnWindowsShutDown.Name = "btnWindowsShutDown";
            this.btnWindowsShutDown.Size = new System.Drawing.Size(63, 58);
            this.btnWindowsShutDown.TabIndex = 486;
            this.btnWindowsShutDown.UseVisualStyleBackColor = false;
            this.btnWindowsShutDown.Click += new System.EventHandler(this.btnWindowsShutDown_Click);
            // 
            // btnSlide
            // 
            this.btnSlide.BackColor = System.Drawing.Color.Transparent;
            this.btnSlide.BackgroundImage = global::AgTwo.Properties.Resources.ArrowGrnRight;
            this.btnSlide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSlide.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSlide.FlatAppearance.BorderSize = 0;
            this.btnSlide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSlide.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSlide.ForeColor = System.Drawing.Color.White;
            this.btnSlide.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSlide.Location = new System.Drawing.Point(289, 6);
            this.btnSlide.Margin = new System.Windows.Forms.Padding(4);
            this.btnSlide.Name = "btnSlide";
            this.btnSlide.Size = new System.Drawing.Size(56, 39);
            this.btnSlide.TabIndex = 475;
            this.btnSlide.UseVisualStyleBackColor = false;
            this.btnSlide.Click += new System.EventHandler(this.btnSlide_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::AgTwo.Properties.Resources.AgIO_First;
            this.pictureBox1.Location = new System.Drawing.Point(583, 360);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 182;
            this.pictureBox1.TabStop = false;
            // 
            // cboxLogNMEA
            // 
            this.cboxLogNMEA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxLogNMEA.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxLogNMEA.BackColor = System.Drawing.Color.Transparent;
            this.cboxLogNMEA.BackgroundImage = global::AgTwo.Properties.Resources.LogNMEA;
            this.cboxLogNMEA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cboxLogNMEA.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxLogNMEA.FlatAppearance.BorderSize = 0;
            this.cboxLogNMEA.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.cboxLogNMEA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxLogNMEA.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxLogNMEA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxLogNMEA.Location = new System.Drawing.Point(562, 415);
            this.cboxLogNMEA.Name = "cboxLogNMEA";
            this.cboxLogNMEA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxLogNMEA.Size = new System.Drawing.Size(63, 58);
            this.cboxLogNMEA.TabIndex = 461;
            this.cboxLogNMEA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxLogNMEA.UseVisualStyleBackColor = false;
            this.cboxLogNMEA.CheckedChanged += new System.EventHandler(this.cboxLogNMEA_CheckedChanged);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::AgTwo.Properties.Resources.SwitchOff;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(3, 415);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 58);
            this.btnExit.TabIndex = 192;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRunAOG
            // 
            this.btnRunAOG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunAOG.BackColor = System.Drawing.Color.Transparent;
            this.btnRunAOG.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnRunAOG.FlatAppearance.BorderSize = 0;
            this.btnRunAOG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAOG.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAOG.ForeColor = System.Drawing.Color.White;
            this.btnRunAOG.Image = global::AgTwo.Properties.Resources.AgIOBtn;
            this.btnRunAOG.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRunAOG.Location = new System.Drawing.Point(287, 415);
            this.btnRunAOG.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunAOG.Name = "btnRunAOG";
            this.btnRunAOG.Size = new System.Drawing.Size(63, 58);
            this.btnRunAOG.TabIndex = 190;
            this.btnRunAOG.UseVisualStyleBackColor = false;
            this.btnRunAOG.Click += new System.EventHandler(this.btnRunAOG_Click);
            // 
            // btnRelayTest
            // 
            this.btnRelayTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRelayTest.BackColor = System.Drawing.Color.Transparent;
            this.btnRelayTest.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnRelayTest.FlatAppearance.BorderSize = 0;
            this.btnRelayTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelayTest.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelayTest.ForeColor = System.Drawing.Color.White;
            this.btnRelayTest.Image = global::AgTwo.Properties.Resources.TestRelays;
            this.btnRelayTest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRelayTest.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRelayTest.Location = new System.Drawing.Point(484, 315);
            this.btnRelayTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnRelayTest.Name = "btnRelayTest";
            this.btnRelayTest.Size = new System.Drawing.Size(61, 73);
            this.btnRelayTest.TabIndex = 190;
            this.btnRelayTest.Text = "Test";
            this.btnRelayTest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRelayTest.UseVisualStyleBackColor = false;
            this.btnRelayTest.Click += new System.EventHandler(this.btnRelayTest_Click);
            // 
            // btnUDP
            // 
            this.btnUDP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUDP.BackColor = System.Drawing.Color.Transparent;
            this.btnUDP.BackgroundImage = global::AgTwo.Properties.Resources.B_UDP;
            this.btnUDP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUDP.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnUDP.FlatAppearance.BorderSize = 0;
            this.btnUDP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUDP.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUDP.ForeColor = System.Drawing.Color.White;
            this.btnUDP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUDP.Location = new System.Drawing.Point(42, 331);
            this.btnUDP.Margin = new System.Windows.Forms.Padding(4);
            this.btnUDP.Name = "btnUDP";
            this.btnUDP.Size = new System.Drawing.Size(72, 51);
            this.btnUDP.TabIndex = 184;
            this.btnUDP.UseVisualStyleBackColor = false;
            this.btnUDP.Click += new System.EventHandler(this.btnUDP_Click);
            // 
            // btnGPSData
            // 
            this.btnGPSData.BackColor = System.Drawing.Color.Transparent;
            this.btnGPSData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGPSData.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnGPSData.FlatAppearance.BorderSize = 0;
            this.btnGPSData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPSData.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPSData.ForeColor = System.Drawing.Color.Black;
            this.btnGPSData.Image = global::AgTwo.Properties.Resources.Nmea;
            this.btnGPSData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPSData.Location = new System.Drawing.Point(128, 6);
            this.btnGPSData.Margin = new System.Windows.Forms.Padding(4);
            this.btnGPSData.Name = "btnGPSData";
            this.btnGPSData.Size = new System.Drawing.Size(70, 49);
            this.btnGPSData.TabIndex = 513;
            this.btnGPSData.UseVisualStyleBackColor = false;
            this.btnGPSData.Click += new System.EventHandler(this.btnGPSData_Click);
            // 
            // FormLoop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(733, 475);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblSkipCounter);
            this.Controls.Add(this.btnResetTimer);
            this.Controls.Add(this.lbl9To16);
            this.Controls.Add(this.lbl1To8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cboxIsSteerModule);
            this.Controls.Add(this.cboxIsNavModule);
            this.Controls.Add(this.cboxIsMachineModule);
            this.Controls.Add(this.btnNav);
            this.Controls.Add(this.btnSteer);
            this.Controls.Add(this.btnMachine);
            this.Controls.Add(this.btnGPS);
            this.Controls.Add(this.btnWindowsShutDown);
            this.Controls.Add(this.lblWorkSwitchStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblSwitchStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWASCounts);
            this.Controls.Add(this.lblSteerAngle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSlide);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboxLogNMEA);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRunAOG);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblCurentLon);
            this.Controls.Add(this.lblCurrentLat);
            this.Controls.Add(this.btnRelayTest);
            this.Controls.Add(this.btnUDP);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblToGPS);
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.btnGPSData);
            this.Controls.Add(this.lblHDOP);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "FormLoop";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "AgTwo UDP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLoop_FormClosing);
            this.Load += new System.EventHandler(this.FormLoop_Load);
            this.Resize += new System.EventHandler(this.FormLoop_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer oneSecondLoopTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCurentLon;
        private System.Windows.Forms.Label lblCurrentLat;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnUDP;
        private System.Windows.Forms.Button btnRunAOG;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cboxLogNMEA;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblSteerAngle;
        private System.Windows.Forms.Button btnSlide;
        private System.Windows.Forms.Label lblWASCounts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSwitchStatus;
        private System.Windows.Forms.Label lblWorkSwitchStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnWindowsShutDown;
        private System.Windows.Forms.CheckBox cboxIsMachineModule;
        private System.Windows.Forms.CheckBox cboxIsNavModule;
        private System.Windows.Forms.CheckBox cboxIsSteerModule;
        private System.Windows.Forms.Label lbl1To8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl9To16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnResetTimer;
        private System.Windows.Forms.Label lblSkipCounter;
        private System.Windows.Forms.Button btnRelayTest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblToGPS;
        private System.Windows.Forms.Button btnGPSData;
        public System.Windows.Forms.Button btnNav;
        public System.Windows.Forms.Button btnGPS;
        public System.Windows.Forms.Button btnMachine;
        public System.Windows.Forms.Button btnSteer;
        private System.Windows.Forms.ToolStripMenuItem toolStripEthernet;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripSerialMonitor;
        private System.Windows.Forms.ToolStripMenuItem toolStripUDPMonitor;
        private System.Windows.Forms.Label lblHDOP;
    }
}

