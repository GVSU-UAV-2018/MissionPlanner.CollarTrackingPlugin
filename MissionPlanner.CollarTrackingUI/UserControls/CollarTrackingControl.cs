using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.CollarTrackingUI
{
    public partial class CollarTrackingControl: UserControl
    {
        public CollarTrackingControl()
        {
            InitializeComponent();
        }

        private void CollarTrackingSetFrequencyButton_Click(object sender, EventArgs e)
        {
            this.CollarTrackingSetFrequencyButton.Text = "Collar Frequency: " + 
                CollarTrackingFrequencyTextBox.Text + " MHz";
        }

        private void CollarTrackingStartScanButton_Click(object sender, EventArgs e)
        {

        }

        private void ColarrTrackingCancelScanButton_Click(object sender, EventArgs e)
        {

        }
    }
}
