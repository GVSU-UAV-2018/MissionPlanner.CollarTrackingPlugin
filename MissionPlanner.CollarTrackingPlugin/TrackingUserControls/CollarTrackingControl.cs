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
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived += RDFData_Received;
        }

        private void CollarTrackingSetFrequencyButton_Click(object sender, EventArgs e)
        {
            if (VerifyFrequency())
            {
                CollarTrackingFrequencyTextBox.BackColor = Color.Green;
                this.CollarFrequencyLabel.Text = CollarTrackingFrequencyTextBox.Text + " MHz";
                SelectedCollarFrequency = Convert.ToDouble(this.CollarTrackingFrequencyTextBox.Text);
                TrackingControl.TrackingControl.Frequency = SelectedCollarFrequency;
                //MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkFrequency(SelectedCollarFrequency); Unlock for testing
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

            UnlockButtons(false);
            //Clear all data contents before re-using
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Clear();

            //this.CollarTrackingScanBackgroundWorker.RunWorkerAsync();//The big dance

        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            //this.CollarTrackingScanBackgroundWorker.CancelAsync();
        }

        private void CollarTrackingFrequencyTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CollarTrackingScanBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            /*for (int i = 0; i < DEGREES; i += DEGREE_INTERVAL)
            {
                if (this.CollarTrackingScanBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                MissionPlanner.CollarTrackingPlugin.TrackingControl.TrackingControl.ScanDirection(i);
                this.CollarTrackingScanBackgroundWorker.ReportProgress((int)(((float)i / DEGREES) * 100));
            }*/
        }

        private void CollarTrackingScanBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.CollarScanProgressBar.Value = e.ProgressPercentage;
        }

        private void CollarTrackingScanBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.CollarScanProgressBar.Value = 100;
            //UnlockButtons(true);
            // TODO: Do stuff on scan completion
        }

        private void RDFData_Received(object o, EventArgs e)
        {
            //Whatever the numberof values is divided by the number of intervals to perform
            this.CollarScanProgressBar.Value = MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Count / (DEGREES / DEGREE_INTERVAL);

            //rinse and repeat
            CollarTrackingPolarChart.Series[0].Points.Clear();
            foreach(KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                CollarTrackingPolarChart.Series[0].Points.AddXY(kvp.Key, kvp.Value);
            }

            //Complete
            if(MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Count == DEGREES / DEGREE_INTERVAL)
            {
                this.CollarScanProgressBar.Value = 100;
                UnlockButtons(true);
            }
        }

        private void UnlockButtons(bool unlock)
        {
            this.CollarTrackingSetFrequencyButton.Enabled = unlock;
            this.CollarTrackingStartScanButton.Enabled = unlock;
            this.CollarTrackingCancelScanButton.Enabled = !unlock;
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
