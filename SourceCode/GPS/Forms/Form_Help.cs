﻿using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class Form_Help : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public Form_Help(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void linkLabelGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkLabelCombineForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            // Add a link to the LinkLabel.
            LinkLabel.Link link = new LinkLabel.Link { LinkData = "https://github.com/farmerbriantee/AgOpenGPS" };
            linkLabelGit.Links.Add(link);

            // Add a link to the LinkLabel.
            LinkLabel.Link linkCf = new LinkLabel.Link
            {
                LinkData = "https://discourse.agopengps.com/"
            };
            linkLabelCombineForum.Links.Add(linkCf);
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gStr.v_AboutIntro))
                System.Diagnostics.Process.Start(gStr.v_AboutIntro);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            bool notFound = false;
            try
            {
                switch (Properties.Settings.Default.setF_culture)
                {
                    case "en":
                        System.Diagnostics.Process.Start("Manual.pdf");
                        break;

                    case "ru":
                        System.Diagnostics.Process.Start("Manual.ru.pdf");
                        break;

                    case "da":
                        System.Diagnostics.Process.Start("Manual.da.pdf");
                        break;

                    case "de":
                        System.Diagnostics.Process.Start("Manual.de.pdf");
                        break;

                    case "nl":
                        System.Diagnostics.Process.Start("Manual.nl.pdf");
                        break;

                    case "it":
                        System.Diagnostics.Process.Start("Manual.it.pdf");
                        break;

                    case "es":
                        System.Diagnostics.Process.Start("Manual.es.pdf");
                        break;

                    case "fr":
                        System.Diagnostics.Process.Start("Manual.fr.pdf");
                        break;

                    case "uk":
                        System.Diagnostics.Process.Start("Manual.uk.pdf");
                        break;

                    case "sk":
                        System.Diagnostics.Process.Start("Manual.sk.pdf");
                        break;

                    case "pl":
                        System.Diagnostics.Process.Start("Manual.pl.pdf");
                        break;

                    case "af":
                        System.Diagnostics.Process.Start("Manual.af.pdf");
                        break;

                    default:
                        System.Diagnostics.Process.Start("Manual.pdf");
                        break;
                }
            }
            catch
            {
                notFound = true;
            }

            if (notFound)
            {
                try
                {
                    System.Diagnostics.Process.Start("Manual.pdf");
                }
                catch
                {
                    //mf.TimedMessageBox(2000, "No File Found", "Can't Find Manual.pdf");
                }

            }

            Close();
        }
    }
}