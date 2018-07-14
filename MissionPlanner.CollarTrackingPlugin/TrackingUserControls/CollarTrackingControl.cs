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

        Logging.Logging logs;
        int SCAN_TIMEOUT = 12;

        public CollarTrackingControl()
        {
            InitializeComponent();
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived += RDFData_Received;
            ReadConfigFile();
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

            CollarTrackingScanInfoLabel.Text = "Scan Status:";
            UnlockButtons(false);
            //Clear all data contents before re-using
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Clear();

            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(true);
            MavLinkRDFCommunication.MavLinkRDFCommunication.ResetFlightPlan(); //Reset flight plan assuming it remains at altitude
            MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1(); //Kick off the scanning
            CollarTrackingTimeoutTimer.Enabled = true;

            logs = new Logging.Logging(@"C:\Drone_Debugging");
        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            CollarTrackingTimeoutTimer.Enabled = false;
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
            float min = 1000;
            foreach(KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                CollarTrackingPolarChart.Series[0].Points.AddXY(kvp.Key, kvp.Value);
                if (kvp.Value < min)
                    min = kvp.Value;
            }
            logs.AddData();
            CollarTrackingPolarChart.ChartAreas[0].AxisY.Minimum = (int)(min - 1);

            //Complete
            if (MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP() 
                == MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount())
            {
                this.CollarScanProgressBar.Value = 100;
                RadiationPatternMatching.RadiationPatternMatching.PerformPatternMatchingAnalysis();

                CollarTrackingScanInfoLabel.Text = "D: " +
                    RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth +
                    "° from N | C: " +
                    (RadiationPatternMatching.RadiationPatternMatching.Confidence * 100).ToString("0.0") + 
                    "%";

                UnlockButtons(true);
                LogScan();
                MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
                CollarTrackingTimeoutTimer.Enabled = false;
            }
            else
            {
                //The next WP should be YAW
                MavLinkRDFCommunication.MavLinkRDFCommunication.GoToNextWayPoint();
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1();
                CollarTrackingTimeoutTimer.Stop();
                CollarTrackingTimeoutTimer.Start();
            }
        }

        private void CollarTrackingTimeoutTimer_Tick(object sender, EventArgs e)
        {
            //Dont post data, just go to next position
            RDFData_Received(new object(), new EventArgs());
        }

        private void CollarTrackingConnectionTimer_Tick(object sender, EventArgs e)
        {

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

        private void ReadConfigFile()
        {
            const string CONFIG_FILE_LOCATION = @"C:\Program Files (x86)\Mission Planner\plugins\MissionPlanner.CollarTrackingPlugin.settings.ini";
            string line = "";
            System.IO.StreamReader file;

            try
            {
                file = new System.IO.StreamReader(CONFIG_FILE_LOCATION);
            }
            catch (FileNotFoundException fex)
            {
                return;
            }

            while ((line = file.ReadLine()) != null)
            {
                if (line.ToLower().Contains("SCAN_TIMEOUT="))
                {
                    try
                    {
                        CollarTrackingTimeoutTimer.Interval = Convert.ToInt32(line.Replace("SCAN_TIMEOUT=", ""));
                    }
                    catch (FormatException fex)
                    {

                    }
                }
                else if (line.ToLower().Contains("COMP_ID="))
                {
                    try
                    {
                        MavLinkRDFCommunication.MavLinkRDFCommunication.comp_id 
                            = Convert.ToInt32(line.Replace("COMP_ID=", ""));
                    }
                    catch (FormatException fex)
                    {

                    }
                }
                else if(line.ToLower().Contains("RAD_PATTERN_FILE="))
                {
                    try
                    {
                        RadiationPatternMatching.RadiationPatternMatching.AntennaPatternFile = line.Replace("COMP_ID=", "");
                        File.ReadLines(RadiationPatternMatching.RadiationPatternMatching.AntennaPatternFile);
                    }
                    catch (FileNotFoundException fex)
                    {

                    }
                }
            }
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
