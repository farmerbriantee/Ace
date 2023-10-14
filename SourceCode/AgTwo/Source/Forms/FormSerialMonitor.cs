﻿using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormSerialMonitor : Form
    {
        public static int baudRate = 115200;

        public static string portName = "***";

        public string recvSentence = "GPS";

        public SerialPort sp = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);

        //class variables
        private readonly FormLoop mf = null;
        private bool logOn = false;

        public FormSerialMonitor(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxRcv.Text = "";
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            ClosePort();
            if (sp.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }
        }

        private void btnFileSave_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("zAgIO_SerialMon_log.txt", false))
            {
                writer.Write(textBoxRcv.Text);
            }

            mf.TimedMessageBox(2000, "File Saved", "To zAgIO_SerialMon_Log.Txt");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(gStr.gsSerialMonHelp);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            logOn = !logOn;
            if (logOn)
            {
                btnLog.BackColor = Color.LightGreen;
                mf.isLogMonitorOn = true;
                timer1.Enabled = true;
            }
            else
            {
                btnLog.BackColor = Color.Transparent;
                mf.isLogMonitorOn = false;
                timer1.Enabled = false;
            }
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            OpenPort();
            if (sp.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
                lblCurrentBaud.Text = sp.BaudRate.ToString();
                lblCurrentPort.Text = sp.PortName;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
                MessageBox.Show("Unable to connect to Port");
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            mf.isLogMonitorOn = false;
            Close();
        }

        private void cboxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            sp.BaudRate = Convert.ToInt32(cboxBaud.Text);
            baudRate = Convert.ToInt32(cboxBaud.Text);
        }

        private void cboxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            sp.PortName = cboxPort.Text;
            portName = cboxPort.Text;
        }

        private void FormSerialMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isLogMonitorOn = false;
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            cboxPort.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
            }
        }

        public void ClosePort()
        {
            if (sp.IsOpen)
            {
                sp.DataReceived -= sp_DataReceived;
                try
                {
                    sp.Close();
                }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing Machine Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                sp.Dispose();
            }
        }

        //open the Arduino serial port
        public void OpenPort()
        {
            if (!sp.IsOpen)
            {
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataReceived += sp_DataReceived;
                sp.DtrEnable = true;
                sp.RtsEnable = true;
            }

            try { sp.Open(); }
            catch (Exception)
            {
                //WriteErrorLog("Opening Machine Port" + e.ToString());

                //MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");

                //Properties.Settings.Default.setPort_wasConnected = false;
                //Properties.Settings.Default.Save();
                //wasConnectedLastRun = false;
            }

            if (sp.IsOpen)
            {
                //short delay for the use of mega2560, it is working in debugmode with breakpoint
                System.Threading.Thread.Sleep(500); // 500 was not enough

                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
            }
        }

        //Send machine info out to machine board
        public void SendPort(byte[] items, int numItems)
        {
            //Tell Arduino to turn section on or off accordingly
            //if (sp.IsOpen)
            //{
            //    try
            //    {
            //        sp.Write(items, 0, numItems);
            //    }
            //    catch (Exception)
            //    {
            //        ClosePort();
            //    }
            //}
        }

        private void ReceivePort(string sentence)
        {
            textBoxRcv.AppendText(sentence);
        }
        private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (sp.IsOpen)
            {
                try
                {
                    string sentence = sp.ReadExisting();
                    BeginInvoke((MethodInvoker)(() => ReceivePort(sentence)));
                }
                catch (Exception)
                {
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBoxRcv.AppendText(mf.logMonitorSentence.ToString());
            mf.logMonitorSentence.Clear();
        }
    }
}