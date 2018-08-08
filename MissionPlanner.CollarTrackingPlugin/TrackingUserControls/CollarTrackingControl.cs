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
        [Description("The selected collar frequency to be scanned."), Category("Data")]
        public float SelectedCollarFrequency
        { get; set; }
        #endregion

        #region Globals
        /// <summary>
        /// Instance for logging data
        /// points for a scan.
        /// </summary>
        Logging.Logging log;

        /// <summary>
        /// Timer for timeout during a RDF scan.
        /// </summary>
        System.Timers.Timer CollarTrackingTimeoutTimer = new System.Timers.Timer(250);

        //Default logging location
        string LOG_LOCATION = @"C:\UAV\Log";

        //Default detection method of 0 is Radiation Pattern Matching.
        //1 uses vector averaging
        int direction_detection_method = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CollarTrackingControl()
        {
            InitializeComponent();
            //Add MavLink RDF Received event handler
            ReadConfigFile();
            CollarTrackingTimeoutTimer.Elapsed += CollarTrackingTimeoutTimer_Tick;
            CollarTrackingTimeoutTimer.Enabled = false;
        }
        #endregion

        #region Handlers
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
                this.CollarFrequencyLabel.Text = CollarTrackingFrequencyTextBox.Text + " MHz";
                SelectedCollarFrequency = (float)Convert.ToDouble(this.CollarTrackingFrequencyTextBox.Text);

                if (MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkFrequency(SelectedCollarFrequency))
                {
                    MessageBox.Show("Frequency set to " + CollarTrackingFrequencyTextBox.Text);
                    this.CollarTrackingStartScanButton.Enabled = true;
                    CollarTrackingFrequencyTextBox.BackColor = Color.Green;
                }
                else
                {
                    MessageBox.Show("Frequency set failed");
                    CollarTrackingFrequencyTextBox.BackColor = Color.Red;
                }
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

            CollarTrackingScanInfoLabel.Text = "Scan Status: ";
            UnlockButtons(false);
            //Clear all data contents before re-using
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Clear();
            polarChartControl1.Clear();
            CollarTrackingConnectionLabel.Text = "";
            CollarTrackingConnectionLabel.BackColor = Color.Black;
            CollarScanProgressBar.Value = 0;

            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived += RDFData_Received;
            MavLinkRDFCommunication.MavLinkRDFCommunication.ResetFlight(); //Reset flight assuming it remains at altitude
            MavLinkRDFCommunication.MavLinkRDFCommunication.DoMavLinkSNRScan(true); //Kick off the scanning
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
            MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived -= RDFData_Received;
            UnlockButtons(true);

            CollarScanProgressBar.Value = 0;
        }

        /// <summary>
        /// Event Handler: Performs all necessary operations
        /// when a data point is received from the Pi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RDFData_Received(object sender, EventArgs e)
        {
            CollarTrackingTimeoutTimer.Enabled = false;
            CollarTrackingConnectionLabel.Text = "Obtained Pi data";
            CollarTrackingConnectionLabel.BackColor = Color.Green;
            CollarTrackingScanInfoLabel.Text = "Scan Status: " + (MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentTurn() + 1) + " out of " +
                MavLinkRDFCommunication.MavLinkRDFCommunication.GetNumberOfTurns() + " turns";

            //Whatever the numberof values is divided by the number of intervals to perform
            this.CollarScanProgressBar.Value = (int)(((double)MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentTurn() / (double)MavLinkRDFCommunication.MavLinkRDFCommunication.GetNumberOfTurns()) * 100);


            log.AddData();
            //rinse and repeat
            //0 index for series because only one series is used
            //for our needs
            KeyValuePair<int, float> kvp = MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData[MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData.Count - 1];
            polarChartControl1.AddPoint(kvp.Key, kvp.Value);

            //Complete
            if (MavLinkRDFCommunication.MavLinkRDFCommunication.GetCurrentTurn()
                == (MavLinkRDFCommunication.MavLinkRDFCommunication.GetNumberOfTurns() - 1))
            {
                this.CollarScanProgressBar.Value = 100;

                if (this.CollarTrackingDetectionMethodCombo.SelectedIndex == 0)
                {
                    if (RadiationPatternMatching.RadiationPatternMatching.PerformPatternMatchingAnalysis())
                    {
                        CollarTrackingScanInfoLabel.Text = "D: " +
                        RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth +
                            "° from N | C: " +
                            (RadiationPatternMatching.RadiationPatternMatching.Confidence * 100).ToString("0.0") +
                            "%";
                    }
                    else
                    {
                        MessageBox.Show("Radiation Pattern error! Is the file already open? Is the name/location correct?");
                    }
                }
                else if (this.CollarTrackingDetectionMethodCombo.SelectedIndex == 1)
                {
                    if(RadiationPatternMatching.VectorAveraging.CalculateResult())
                    {
                        CollarTrackingScanInfoLabel.Text = "D: " +
                            RadiationPatternMatching.VectorAveraging.direction +
                            "° from N | RSSI: " +
                            (RadiationPatternMatching.VectorAveraging.magnitude).ToString("0.0");
                    }
                    else
                    {
                        MessageBox.Show("Vector averaging method failed.");
                    }
                }
                UnlockButtons(true);
                LogScan(true);
                MavLinkRDFCommunication.MavLinkRDFCommunication.RDFDataReceived -= RDFData_Received;
                CollarTrackingTimeoutTimer.Enabled = false;
                CollarTrackingConnectionLabel.Text = "";
                CollarTrackingConnectionLabel.BackColor = Color.Black;
            }
            else
            {
                //The next WP should be YAW
                MavLinkRDFCommunication.MavLinkRDFCommunication.GoToNextTurn();
                MavLinkRDFCommunication.MavLinkRDFCommunication.DoMavLinkSNRScan(true);

                CollarTrackingTimeoutTimer.Enabled = true;
                CollarTrackingTimeoutTimer.Start();
            }
        }

        /// <summary>
        /// Event Handler: Waits for Pi SNR to
        /// be received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollarTrackingTimeoutTimer_Tick(object sender, EventArgs e)
        {
            //Resend frequency since Pi probably restarted
            //Try to do scan in direction again
            CollarTrackingConnectionLabel.Text = "Waiting on Pi...";
            CollarTrackingConnectionLabel.BackColor = Color.Yellow;
            MavLinkRDFCommunication.MavLinkRDFCommunication.GetScanStatus();
        }

        /// <summary>
        /// Event Handler: IF Gain click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IFGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkIFGain(Convert.ToInt32(IFGainTextBox.Text)))
                {
                    MessageBox.Show("IF Gain set to " + IFGainTextBox.Text);
                }
                else
                {
                    MessageBox.Show("IF Gain set failed");
                }

            }
            catch (FormatException ex1)
            {
                MessageBox.Show("IF Gain not valid");
            }
        }

        /// <summary>
        /// Event Handler: Mixer Gain click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MixerGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkMixerGain(Convert.ToInt32(MixerGainTextBox.Text)))
                {
                    MessageBox.Show("Mixer Gain set to " + MixerGainTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Mixer Gain set failed");
                }

            }
            catch (FormatException ex1)
            {
                MessageBox.Show("Mixer Gain not valid");
            }
        }

        /// <summary>
        /// Event Handler: LNA Gain click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LNAGainButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MavLinkRDFCommunication.MavLinkRDFCommunication.SendMavLinkLNAGain(Convert.ToInt32(LNAGainTextBox.Text)))
                {
                    MessageBox.Show("LNA Gain set to " + LNAGainTextBox.Text);
                }
                else
                {
                    MessageBox.Show("LNA Gain set failed");
                }

            }
            catch (FormatException ex1)
            {
                MessageBox.Show("LNA Gain not valid");
            }
        }
        #endregion

        #region Helpers
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
            this.CollarTrackingDetectionMethodCombo.Enabled = unlock;
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
                    //MessageBox.Show("Collar Tracking Control received exception: " + fex.Message);
                    return;
                }

                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToUpper().Contains("COMP_ID="))
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
                    else if(line.ToUpper().Contains("DEGREE_INTERVAL="))
                    {
                        try
                        {
                            MavLinkRDFCommunication.MavLinkRDFCommunication.SetDirectionResolution(Convert.ToInt32(line.Replace("DEGREE_INTERVAL=", "")));
                        }
                        catch(FormatException fex)
                        {

                        }
                    }
                    else if (line.ToUpper().Contains("LOG_DIR="))
                    {
                        LOG_LOCATION = line.Replace("LOG_DIR=", "");
                    }
                    else if(line.ToUpper().Contains("DIR_DETECTION_METHOD="))
                    {
                        try
                        {
                            this.CollarTrackingDetectionMethodCombo.SelectedIndex = Convert.ToInt32(line.Replace("DIR_DETECTION_METHOD=", ""));
                        }
                        catch (FormatException fex)
                        {

                        }
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
                if(this.CollarTrackingDetectionMethodCombo.SelectedIndex == 0)
                {
                    appendedLine = CollarTrackingFrequencyTextBox.Text + ", Completed, " +
                        RadiationPatternMatching.RadiationPatternMatching.DegreesFromNorth + "," +
                        RadiationPatternMatching.RadiationPatternMatching.Confidence + "," +
                        DateTime.Now.ToString() + "\n";
                }
                else if(this.CollarTrackingDetectionMethodCombo.SelectedIndex == 1)
                {
                    appendedLine = CollarTrackingFrequencyTextBox.Text + ", Completed, " +
                        RadiationPatternMatching.VectorAveraging.direction + "," +
                        RadiationPatternMatching.VectorAveraging.magnitude + "," +
                        DateTime.Now.ToString() + "\n";
                }
            }
            else
            {
                appendedLine = CollarTrackingFrequencyTextBox.Text + ", Cancelled, " +
                0 + "," +
                0 + "," +
                DateTime.Now.ToString() + "\n";
            }

            if (!File.Exists(LOG_LOCATION + @"\" + FILE_NAME))
                File.WriteAllText(LOG_LOCATION + @"\" + FILE_NAME,
                    "Frequency, Completed?, Degrees from North,Confidence,Date/Time\n");

            File.AppendAllText(LOG_LOCATION + @"\" + FILE_NAME, appendedLine);
        }
        #endregion

        #region Debug
        #endregion
    }
}
