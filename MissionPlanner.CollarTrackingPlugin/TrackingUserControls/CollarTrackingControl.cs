using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MissionPlanner.CollarTrackingPlugin
{
    public partial class CollarTrackingControl : UserControl
    {
        private const int DEGREES = 360;
        private const int DEGREE_INTERVAL = 5;

        #region Properties
        /// <summary>
        /// Gets/Sets the desired collar frequency
        /// to search for.
        /// </summary>
        [Description("The selected collar frequency to be scanned."),Category("Data")] 
        public double SelectedCollarFrequency
        { get; set; }

        #endregion

        public CollarTrackingControl()
        {
            InitializeComponent();
        }

        private void CollarTrackingSetFrequencyButton_Click(object sender, EventArgs e)
        {
            if (VerifyFrequency())
            {
                CollarTrackingFrequencyTextBox.BackColor = Color.Green;
                this.CollarFrequencyLabel.Text = CollarTrackingFrequencyTextBox.Text + " MHz";
                SelectedCollarFrequency = Convert.ToDouble(this.CollarTrackingFrequencyTextBox.Text);
                this.CollarTrackingStartScanButton.Enabled = true;
            }
            else
            {
                CollarTrackingFrequencyTextBox.BackColor = Color.Red;
                this.CollarTrackingStartScanButton.Enabled = false;
            }
        }

        private void CollarTrackingStartScanButton_Click(object sender, EventArgs e)
        {
            if (SelectedCollarFrequency == 0 && VerifyFrequency())
                return;

            Scanning(false);
            Scan();
            Scanning(true);
        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            Scanning(true);
        }

        private void CollarTrackingFrequencyTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Scan()
        {
            for (int i = 0; i < DEGREES; i += DEGREE_INTERVAL)
            {
                MissionPlanner.CollarTrackingPlugin.TrackingControl.TrackingControl.ScanDirection(i);
                this.CollarScanProgressBar.Value = (int)(((float)i / DEGREES) * 100);
            }
            this.CollarScanProgressBar.Value = 100;
        }

        private void Scanning(bool isComplete)
        {
            this.CollarTrackingSetFrequencyButton.Enabled = isComplete;
            this.CollarTrackingStartScanButton.Enabled = isComplete;
            this.CollarTrackingCancelScanButton.Enabled = !isComplete;
        }

        private bool VerifyFrequency()
        {
            bool isValidFrequency = false;
            Regex r = new Regex("[1][4-5][0-9][.][0-9][0-9][0-9]");
            if (r.IsMatch(CollarTrackingFrequencyTextBox.Text))
                isValidFrequency = true;
            return isValidFrequency;
        }
    }
}
