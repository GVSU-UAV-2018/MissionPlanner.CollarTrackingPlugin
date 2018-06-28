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
        private int DEGREES = 360;
        private int DEGREE_INTERVAL = 5;
        private string LOG_LOCATION = @"C:\";

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

            UnlockButtons(false);
            //Clear all data contents before re-using
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Clear();

            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(true);
            MavLinkRDFCommunication.MavLinkRDFCommunication.ResetFlightPlan(); //Reset flight plan
            MavLinkRDFCommunication.MavLinkRDFCommunication.LoiterDrone(false); //Let drone fly
        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
            MavLinkRDFCommunication.MavLinkRDFCommunication.LoiterDrone(true); //Hold drone position
        }

        private void RDFData_Received(object o, EventArgs e)
        {
            //Whatever the numberof values is divided by the number of intervals to perform
            this.CollarScanProgressBar.Value = MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Count / (DEGREES / DEGREE_INTERVAL);

            //rinse and repeat
            //0 index for series because only one series is used
            //for our needs
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
                LogScan();
                MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
                MavLinkRDFCommunication.MavLinkRDFCommunication.LoiterDrone(true); //Hold drone position
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
            string appendedLine = CollarTrackingFrequencyTextBox.Text + "," +
                RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth + "," +
                RadiationPatternMatching.RadiationPatternMatching.Confidence + "," +
                DateTime.Now.ToString();

            if (!File.Exists(LOG_LOCATION + @"\" + FILE_NAME))
                File.WriteAllText(LOG_LOCATION + @"\" + FILE_NAME,
                    "Frequency,Degrees from North,Confidence,Date/Time");

            File.AppendAllText(LOG_LOCATION + @"\" + FILE_NAME, appendedLine);
        }

        private void ReadConfigFile()
        {
            const string CONFIG_FILE_LOCATION = @"C:\Program Files (x86)\Mission Planner\plugins\MissionPlanner.CollarTrackingPlugin.Settings.txt";
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(CONFIG_FILE_LOCATION);
            while ((line = file.ReadLine()) != null)
            {
                if (line.ToLower().Contains("degrees="))
                {
                    try
                    {
                        DEGREES = Convert.ToInt32(line.Replace("degrees=", ""));
                    }
                    catch (FormatException fex)
                    {

                    }
                }
                else if (line.ToLower().Contains("degree_interval="))
                {
                    try
                    {
                        DEGREE_INTERVAL = Convert.ToInt32(line.Replace("degree_interval=", ""));
                    }
                    catch (FormatException fex)
                    {

                    }
                }
                else if (line.ToLower().Contains("degrees="))
                {
                    LOG_LOCATION = line.Replace("log_location=", "");
                }
            }
        }
    }
}
