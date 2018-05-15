using System;
using System.Windows.Forms;

using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using MissionPlanner;
using MissionPlanner.Utilities;

namespace MissionPlanner.CollarTrackingPlugin
{
    public class TrackingControlPlugin : MissionPlanner.Plugin.Plugin
    {
        Form f = new Form();

        public override string Name => "Collar Tracking Interface Plugin";

        public override string Version => "1.0";

        public override string Author => "2018 UAV Senior Project Team";

        public override bool Init()
        {
            f.Text = "Test";
            f.Size = new System.Drawing.Size(500, 500);
            CollarTrackingControl ctc = new CollarTrackingControl();
            f.Controls.Add(ctc);
            ctc.Dock = DockStyle.Fill;

            f.Show();
            return true;
        }

        public override bool Loaded()
        {
            return true;
        }

        public override bool Exit()
        {
            f.Hide();
            return true;
        }

        public override bool Loop()
        {
            return true;
        }

        public override bool SetupUI(int gui = 0, object data = null)
        {
            return true;
        }
    }
}
