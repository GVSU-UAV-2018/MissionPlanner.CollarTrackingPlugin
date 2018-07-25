﻿/*******************************************************
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

        Logging.Logging log;

        System.Timers.Timer CollarTrackingTimeoutTimer = new System.Timers.Timer(12000);

        string LOG_LOCATION = @"C:\UAV\Log";

        /// <summary>
        /// Constructor
        /// </summary>
        public CollarTrackingControl()
        {
            InitializeComponent();
            //Add MavLink RDF Received event handler
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived += RDFData_Received;
            ReadConfigFile();
            CollarTrackingTimeoutTimer.Elapsed += CollarTrackingTimeoutTimer_Tick;
            CollarTrackingTimeoutTimer.Enabled = false;
        }

        /// <summary>
        /// Event Handler: Sends the frequency in
        /// the Collar Tracking text box to the
        /// Pi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event Handler: Starts a scan by sending a message
        /// to the Pi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollarTrackingStartScanButton_Click(object sender, EventArgs e)
        {
            if (SelectedCollarFrequency == 0 && VerifyFrequency())
                return;

            CollarTrackingScanInfoLabel.Text = "Scan Status:";
            UnlockButtons(false);
            //Clear all data contents before re-using
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Clear();
            CollarTrackingPolarChart.Series[0].Points.Clear();
            CollarScanProgressBar.Value = 0;

            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(true);
            MavLinkRDFCommunication.MavLinkRDFCommunication.ResetFlightPlan(); //Reset flight plan assuming it remains at altitude
            MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1(); //Kick off the scanning
            CollarTrackingTimeoutTimer.Enabled = true;
            CollarTrackingTimeoutTimer.Start();
            log = new Logging.Logging(LOG_LOCATION);
        }

        /// <summary>
        /// Event Handler: Clears the event handler that
        /// keeps the Pi scanning for data, which
        /// effectively cancels the scan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {
            LogScan(false);
            CollarTrackingTimeoutTimer.Enabled = false;
            MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
            UnlockButtons(true);
        }

        /// <summary>
        /// Event Handler: Performs all necessary operations
        /// when a data point is received from the Pi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RDFData_Received(object sender, EventArgs e)
        {
            CollarTrackingFrequencyTextBox.Text = MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP().ToString() + " | " + MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount();
            CollarTrackingConnectionLabel.Text = "Receiving Pi data";
            CollarTrackingConnectionLabel.BackColor = Color.Green;

            //Whatever the numberof values is divided by the number of intervals to perform
            this.CollarScanProgressBar.Value = (int)(((double)MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP() / (double)MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount()) * 100);

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
            log.AddData();
            CollarTrackingPolarChart.ChartAreas[0].AxisY.Minimum = (int)(min - 1);

            //Complete
            if (MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentWP() 
                >= (MavLinkRDFCommunication.MavLinkRDFCommunication.GetWPCount() - 1))
            {
                this.CollarScanProgressBar.Value = 100;

                RadiationPatternMatching.RadiationPatternMatching.PerformPatternMatchingAnalysis();
                CollarTrackingScanInfoLabel.Text = "D: " +
                RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth +
                    "° from N | C: " +
                    (RadiationPatternMatching.RadiationPatternMatching.Confidence * 100).ToString("0.0") + 
                    "%";
                UnlockButtons(true);
                LogScan(true);
                MavLinkRDFCommunication.MavLinkRDFCommunication.CaptureRDFData(false);
                CollarTrackingTimeoutTimer.Enabled = false;
                CollarTrackingConnectionLabel.Text = "";
                CollarTrackingConnectionLabel.BackColor = Color.Black;
            }
            else
            {
                //The next WP should be YAW
                MavLinkRDFCommunication.MavLinkRDFCommunication.GoToNextWayPoint();
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1();
                CollarTrackingTimeoutTimer.Enabled = false;
                CollarTrackingTimeoutTimer.Enabled = true;
                CollarTrackingTimeoutTimer.Start();
            }
        }

        /// <summary>
        /// Event Handler: Skips waiting for the current data if
        /// the desired interval is reached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollarTrackingTimeoutTimer_Tick(object sender, EventArgs e)
        {
            //Resend frequency since Pi probably restarted
            //Try to do scan in direction again
            CollarTrackingConnectionLabel.Text = "No data rcvd. Retrying...";
            CollarTrackingConnectionLabel.BackColor = Color.Red;
            MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkFrequency(SelectedCollarFrequency);
            System.Threading.Thread.Sleep(250);
            MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkCmdLongUser_1();
        }

        /// <summary>
        /// Places the user interface in mode specific
        /// to the system's current state.
        /// </summary>
        /// <param name="unlock"></param>
        private void UnlockButtons(bool unlock)
        {
            this.CollarTrackingSetFrequencyButton.Enabled = unlock;
            this.CollarTrackingStartScanButton.Enabled = unlock;
            this.CollarTrackingCancelScanButton.Enabled = !unlock;
        }

        /// <summary>
        /// Ensures that the entered frequency is of correct
        /// format.
        /// </summary>
        /// <returns></returns>
        private bool VerifyFrequency()
        {
            bool isValidFrequency = false;
            Regex r = new Regex("[1][4-5][0-9][.][0-9][0-9][0-9]");
            if (r.IsMatch(CollarTrackingFrequencyTextBox.Text))
                isValidFrequency = true;
            return isValidFrequency;
        }

        /// <summary>
        /// Loads the configuration parameters
        /// for the program.
        /// </summary>
        private void ReadConfigFile()
        {
            const string CONFIG_FILE_LOCATION = @"C:\Program Files (x86)\Mission Planner\plugins\MissionPlanner.CollarTrackingPlugin.settings.ini";
            string line = "";
            System.IO.StreamReader file;

            try
            {
                try
                {
                    file = new System.IO.StreamReader(CONFIG_FILE_LOCATION);
                }
                catch (FileNotFoundException fex)
                {
                    MessageBox.Show("Collar Tracking Control received exception: " + fex.Message);
                    return;
                }

                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToUpper().Contains("SCAN_TIMEOUT="))
                    {
                        try
                        {
                            CollarTrackingTimeoutTimer.Interval = Convert.ToInt32(line.Replace("SCAN_TIMEOUT=", "")) * 1000;
                        }
                        catch (FormatException fex)
                        {

                        }
                    }
                    else if (line.ToUpper().Contains("COMP_ID="))
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
                    else if (line.ToUpper().Contains("RAD_PATTERN_FILE="))
                    {
                        try
                        {
                            RadiationPatternMatching.RadiationPatternMatching.AntennaPatternFile = line.Replace("RAD_PATTERN_FILE=", "");
                            File.ReadLines(RadiationPatternMatching.RadiationPatternMatching.AntennaPatternFile);
                        }
                        catch (FileNotFoundException fex)
                        {

                        }
                    }
                    else if (line.ToUpper().Contains("LOG_DIR="))
                    {
                        LOG_LOCATION = line.Replace("LOG_DIR=", "");
                    }
                }
            }

            catch (Exception e)
            {
                //Get any possible user exception
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Saves a log of an RDF scan.
        /// </summary>
        private void LogScan(bool completed)
        {
            const string FILE_NAME = "CollarLog.csv";
            string appendedLine = "";
            if (completed)
            {
                appendedLine = CollarTrackingFrequencyTextBox.Text + ", Completed, " +
                    RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth + "," +
                    RadiationPatternMatching.RadiationPatternMatching.Confidence + "," +
                    DateTime.Now.ToString();
            }
            else
            {
                appendedLine = CollarTrackingFrequencyTextBox.Text + ", Cancelled, " +
                0 + "," +
                0 + "," +
                DateTime.Now.ToString();
            }

            if (!File.Exists(LOG_LOCATION + @"\" + FILE_NAME))
                File.WriteAllText(LOG_LOCATION + @"\" + FILE_NAME,
                    "Frequency, Completed?, Degrees from North,Confidence,Date/Time");

            File.AppendAllText(LOG_LOCATION + @"\" + FILE_NAME, appendedLine);
        }

        private void IFGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkIFGain(Convert.ToInt32(IFGainTextBox.Text));
            }
            catch(FormatException ex1)
            {
                MessageBox.Show("IF Gain not valid");
            }
        }

        private void MixerGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkMixerGain(Convert.ToInt32(MixerGainTextBox.Text));
            }
            catch (FormatException ex1)
            {
                MessageBox.Show("Mixer Gain not valid");
            }
        }

        private void LNAGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkLNAGain(Convert.ToInt32(LNAGainTextBox.Text));
            }
            catch (FormatException ex1)
            {
                MessageBox.Show("LNA Gain not valid");
            }
        }
    }
}
