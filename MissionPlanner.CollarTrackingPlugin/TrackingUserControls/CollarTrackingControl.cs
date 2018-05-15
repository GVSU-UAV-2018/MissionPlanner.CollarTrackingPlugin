using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

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
        public float SelectedCollarFrequency
        { get; set; }

        #endregion

        public CollarTrackingControl()
        {
            InitializeComponent();
        }

        private void CollarTrackingSetFrequencyButton_Click(object sender, EventArgs e)
        {
            this.CollarFrequencyLabel.Text = CollarTrackingFrequencyTextBox.Text + " MHz";
        }

        private void CollarTrackingStartScanButton_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {

        }

        private void Scan()
        {
            for (int i = 0; i < DEGREES; i += DEGREE_INTERVAL)
            {
                MissionPlanner.CollarTrackingPlugin.TrackingControl.TrackingControl.ScanDirection(i);
                CollarScanProgressBar.Value = (int)(((float)i / DEGREES) * 100);
            }
            CollarScanProgressBar.Value = 100;
        }
    }
}
