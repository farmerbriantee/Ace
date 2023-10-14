using System;
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
    public partial class FormEthernet : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormEthernet(Form callingForm)
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

            cboxIsUDPOn.Checked = Properties.Settings.Default.setUDP_isOn;
            cboxIsSendNMEAToUDP.Checked = Properties.Settings.Default.setUDP_isSendNMEAToUDP;

            nudFirstIP.Value = Properties.Settings.Default.eth_loopOne;
            nudSecndIP.Value = Properties.Settings.Default.eth_loopTwo;
            nudThirdIP.Value = Properties.Settings.Default.eth_loopThree;
            nudFourthIP.Value= Properties.Settings.Default.eth_loopFour;

            if (!cboxIsUDPOn.Checked) cboxIsUDPOn.BackColor = System.Drawing.Color.Salmon;
        }

        private void nudFirstIP_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.eth_loopOne = (byte)nudFirstIP.Value;
            Properties.Settings.Default.eth_loopTwo = (byte)nudSecndIP.Value;
            Properties.Settings.Default.eth_loopThree = (byte)nudThirdIP.Value;
            Properties.Settings.Default.eth_loopFour = (byte)nudFourthIP.Value;

            Properties.Settings.Default.setUDP_isOn = cboxIsUDPOn.Checked;
            Properties.Settings.Default.setUDP_isSendNMEAToUDP = cboxIsSendNMEAToUDP.Checked;

            Properties.Settings.Default.Save();

            mf.YesMessageBox("AgTwo will Restart to Enable UDP Networking Features");

            Application.Restart();
            Environment.Exit(0);
            Close();

        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(gStr.gsEthernetHelp);
        }
    }
}
