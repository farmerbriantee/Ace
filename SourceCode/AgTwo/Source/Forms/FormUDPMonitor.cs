using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AgTwo
{
    public partial class FormUDPMonitor : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormUDPMonitor(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            mf.isUDPMonitorOn = true;
            mf.isLogNMEA = false;
            mf.isNTRIPLogOn = false;

            timer1.Enabled = true;
        }


        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            mf.isUDPMonitorOn = false;
            mf.isLogNMEA = false;
            mf.isNTRIPLogOn = false;
            timer1.Enabled = false;

            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBoxRcv.AppendText(mf.logUDPSentence.ToString());
            mf.logUDPSentence.Clear();
        }

        private void btnFileSave_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("zAgIO_UDP_log.txt", false))
            {
                writer.Write(textBoxRcv.Text);
            }

            mf.TimedMessageBox(2000, "File Saved", "To zAgIO_UDP_Log.Txt");
        }

        private void FormUDPMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isUDPMonitorOn = false;
            mf.isLogNMEA = false;
            mf.isNTRIPLogOn = false;

            timer1.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxRcv.Text = "";
        }

        private void btnLogNMEA_Click(object sender, EventArgs e)
        {

            mf.isGPSLogOn = !mf.isGPSLogOn;

            if (mf.isGPSLogOn) btnLogNMEA.BackColor = Color.LightGreen;
            else btnLogNMEA.BackColor = Color.Transparent;

            DoLogTimerUpdate();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            mf.isUDPMonitorOn = !mf.isUDPMonitorOn;
            if (mf.isUDPMonitorOn)
            {
                btnLog.BackColor = Color.LightGreen;
            }
            else
            {
                btnLog.BackColor = Color.Transparent;
            }

            DoLogTimerUpdate();
        }

        private void DoLogTimerUpdate()
        {
            if (mf.isGPSLogOn || mf.isUDPMonitorOn || mf.isNTRIPLogOn)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
                
                    }

        private void btnLogNTRIP_Click(object sender, EventArgs e)
        {
            mf.isNTRIPLogOn = !mf.isNTRIPLogOn;

            if (mf.isNTRIPLogOn) btnLogNTRIP.BackColor = Color.LightGreen;
            else btnLogNTRIP.BackColor = Color.Transparent;

            DoLogTimerUpdate();
        }

        private void lblPGNGuide_Click(object sender, EventArgs e)
        {
            var form = new FormPGN();
                form.Show(this);

        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                btnPause.BackgroundImage = Properties.Resources.Play;
            }
            else
            {
                timer1.Enabled = true;
                btnPause.BackgroundImage = Properties.Resources.Pause;
            }
        }
    }
}
