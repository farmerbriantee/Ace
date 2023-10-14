﻿using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBoundaryPlayer : Form
    {
        //properties
        private readonly FormGPS mf = null;

        private bool isClosing;

        //constructor
        public FormBoundaryPlayer(Form callingForm)
        {
            mf = callingForm as FormGPS;


            InitializeComponent();

            //btnStop.Text = gStr.gsDone;
            //btnPausePlay.Text = gStr.gsRecord;
            label1.Text = gStr.gsArea + ":";
            this.Text = gStr.gsStopRecordPauseBoundary;
            nudOffset.Controls[0].Enabled = false;
        }
        private void FormBoundaryPlayer_Load(object sender, EventArgs e)
        {
            //mf.bnd.isOkToAddPoints = false;
            if (mf.isMetric)
            {
                nudOffset.Maximum = 4999;
                nudOffset.Value = (decimal)(mf.tool.width * 0.5 * 100);
                lblMetersInches.Text = gStr.gsCentimeters;
            }
            else
            {
                nudOffset.Maximum = 1968;
                nudOffset.Value = (decimal)(mf.tool.width * 0.5 * 39.3701);
                double ftInches = (double)nudOffset.Value;
                lblMetersInches.Text = ((int)(ftInches / 12)).ToString() + "' " + ((int)(ftInches % 12)).ToString() + '"';
                //lblMetersInches.Text = gStr.gsInches;
            }
            btnPausePlay.Image = Properties.Resources.BoundaryRecord;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.bnd.createBndOffset = (mf.tool.width * 0.5);
            mf.bnd.isBndBeingMade = true;
            mf.Focus();
        }

        private void FormBoundaryPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        private void nudOffset_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnPausePlay.Focus();
            if (mf.isMetric)
            {
                mf.bnd.createBndOffset = (double)nudOffset.Value*0.01;
            }
            else
            {
                mf.bnd.createBndOffset = (double)nudOffset.Value/39.3701;
                double ftInches = (double)nudOffset.Value;
                lblMetersInches.Text = ((int)(ftInches / 12)).ToString() + "' " + (ftInches % 12).ToString("N1") + '"';
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.Count;
            double area = 0;

            if (ptCount > 0)
            {
                int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                for (int i = 0; i < ptCount; j = i++)
                {
                    area += (mf.bnd.bndBeingMadePts[j].easting + mf.bnd.bndBeingMadePts[i].easting) * (mf.bnd.bndBeingMadePts[j].northing - mf.bnd.bndBeingMadePts[i].northing);
                }
                area = Math.Abs(area / 2);
            }
            if (mf.isMetric)
            {
                lblArea.Text = Math.Round(area * 0.0001, 2).ToString();
            }
            else
            {
                lblArea.Text = Math.Round(area * 0.000247105, 2).ToString();
            }
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndBeingMadePts.Count > 2)
            {
                CBoundaryList New = new CBoundaryList();

                for (int i = 0; i < mf.bnd.bndBeingMadePts.Count; i++)
                {
                    New.fenceLine.Add(mf.bnd.bndBeingMadePts[i]);
                }

                New.CalculateFenceArea(mf.bnd.bndList.Count);
                New.FixFenceLine(mf.bnd.bndList.Count);

                mf.bnd.bndList.Add(New);
                mf.fd.UpdateFieldBoundaryGUIAreas();

                //turn lines made from boundaries
                mf.CalculateMinMax();
                mf.FileSaveBoundary();
                mf.bnd.BuildTurnLines();
                //mf.hd.BuildSingleSpaceHeadLines();
                mf.btnABDraw.Visible = true;
            }

            //stop it all for adding
            mf.bnd.isOkToAddPoints = false;
            mf.bnd.isBndBeingMade = false;
            mf.bnd.bndBeingMadePts.Clear();
            
            //close window
            isClosing = true;
            Close();
        }

        //actually the record button
        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            if (mf.bnd.isOkToAddPoints)
            {
                mf.bnd.isOkToAddPoints = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                //btnPausePlay.Text = gStr.gsRecord;
                btnAddPoint.Enabled = true;
                btnDeleteLast.Enabled = true;
            }
            else
            {
                mf.bnd.isOkToAddPoints = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.gsPause;
                btnAddPoint.Enabled = false;
                btnDeleteLast.Enabled = false;
            }
            mf.Focus();
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {

            mf.bnd.isOkToAddPoints = true;
            mf.AddBoundaryPoint();
            mf.bnd.isOkToAddPoints = false;
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();

            mf.Focus();
        }

        private void btnDeleteLast_Click(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.Count;
            if (ptCount > 0)
                mf.bnd.bndBeingMadePts.RemoveAt(ptCount - 1);
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();
            mf.Focus();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.gsCompletelyDeleteBoundary,
                                    gStr.gsDeleteForSure,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.bnd.bndBeingMadePts?.Clear();
                lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();
            }
            mf.Focus();
        }

        private void btnLeftRight_Click(object sender, EventArgs e)
        {
            mf.bnd.isDrawRightSide = !mf.bnd.isDrawRightSide;
            btnLeftRight.Image = mf.bnd.isDrawRightSide ? Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
        }

        #region Help
        private void nudOffset_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_nudOffset, gStr.gsHelp);
        }

        private void btnLeftRight_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnLeftRight, gStr.gsHelp);
        }

        private void btnDeleteLast_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnDeleteLast, gStr.gsHelp);
        }

        private void btnAddPoint_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnAddPoint, gStr.gsHelp);
        }

        private void btnRestart_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnRestart, gStr.gsHelp);
        }

        private void btnPausePlay_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnPausePlay, gStr.gsHelp);
        }

        private void btnStop_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            MessageBox.Show(gStr.hb_btnStop, gStr.gsHelp);
        }

        #endregion
    }
}

/*
            
            MessageBox.Show(gStr, gStr.gsHelp);

            DialogResult result2 = MessageBox.Show(gStr, gStr.gsHelp,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result2 == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rsJMRZrcuX4");
            }

*/
