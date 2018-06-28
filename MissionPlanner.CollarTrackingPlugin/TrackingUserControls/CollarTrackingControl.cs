/*******************************************************
 * Mission Planner UAV Plugin
 * 
 * GVSU Team UAV 2018
 * 
 * User will provide a flight plan that consists of
 * loiter unlimited and yaw commands. This determines
 * how many increments the drone turns in.
 ******************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace MissionPlanner.CollarTrackingPlugin
{
    public partial class CollarTrackingControl : UserControl
    {
        #region Properties
        /// <summary>
        /// Gets/Sets the desired collar frequency
        /// to search for.
        /// </summary>
        [Description("The selected collar frequency to be scanned."),Category("Data")] 
        public float SelectedCollarFrequency
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
                SelectedCollarFrequency = (float)Convert.ToDouble(this.CollarTrackingFrequencyTextBox.Text);
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkFrequency(SelectedCollarFrequency);
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

            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(true);
            MavLinkRDFCommunication.MavLinkRDFCommunication.ResetFlightPlan(); //Reset flight plan assuming it remains at altitude
            MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1(); //Kick off the scanning
        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
            UnlockButtons(true);
        }

        private void RDFData_Received(object o, EventArgs e)
        {
            //Whatever the numberof values is divided by the number of intervals to perform
            this.CollarScanProgressBar.Value = (int)(MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP() / (float)MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount()) * 100;

            //rinse and repeat
            //0 index for series because only one series is used
            //for our needs
            CollarTrackingPolarChart.Series[0].Points.Clear();
            foreach(KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                CollarTrackingPolarChart.Series[0].Points.AddXY(kvp.Key, kvp.Value);
            }

            //Complete
            if(MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP() 
                == MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount())
            {
                this.CollarScanProgressBar.Value = 100;
                UnlockButtons(true);
                LogScan();
                MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
            }
            else
            {
                //The next WP should be YAW
                MavLinkRDFCommunication.MavLinkRDFCommunication.GoToNextWayPoint();
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1();
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

        private void LogScan()
        {
            const string FILE_NAME = "CollarLog.csv";
            const string LOG_LOCATION = @"C:\TrackingLogs";
            string appendedLine = CollarTrackingFrequencyTextBox.Text + "," +
                RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth + "," +
                RadiationPatternMatching.RadiationPatternMatching.Confidence + "," +
                DateTime.Now.ToString();

            if (!File.Exists(LOG_LOCATION + @"\" + FILE_NAME))
                File.WriteAllText(LOG_LOCATION + @"\" + FILE_NAME,
                    "Frequency,Degrees from North,Confidence,Date/Time");

            File.AppendAllText(LOG_LOCATION + @"\" + FILE_NAME, appendedLine);
        }
    }
}
