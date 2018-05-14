using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.CollarTrackingUI
{
    public partial class CollarTrackingControl : UserControl
    {
        /// <summary>
        /// Gets/Sets the desired collar frequency
        /// to search for.
        /// </summary>
        [Description("The selected collar frequency to be scanned."),Category("Data")] 
        public float SelectedCollarFrequency
        { get; set; }

        public CollarTrackingControl()
        {
            InitializeComponent();
        }

        private void CollarTrackingSetFrequencyButton_Click(object sender, EventArgs e)
        {
            this.CollarFrequencyLabel.Text = "Collar Freq: " + 
                CollarTrackingFrequencyTextBox.Text + " MHz";
        }

        private void CollarTrackingStartScanButton_Click(object sender, EventArgs e)
        {

        }

        private void CollarTrackingCancelScanButton_Click(object sender, EventArgs e)
        {

        }
    }
}
