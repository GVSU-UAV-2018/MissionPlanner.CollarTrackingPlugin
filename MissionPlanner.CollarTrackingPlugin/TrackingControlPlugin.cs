using System;
using System.Windows.Forms;

using MissionPlanner;

namespace MissionPlanner.CollarTrackingPlugin
{
    public class TrackingControlPlugin : MissionPlanner.Plugin.Plugin
    {
        TabPage p;

        public override string Name => "Collar Tracking Interface Plugin";

        public override string Version => "1.0";

        public override string Author => "2018 UAV Senior Project Team";

        public override bool Init()
        {
            CollarTrackingControl ctc = new CollarTrackingControl();
            ctc.Dock = DockStyle.Fill;
            p = new TabPage("Collar Tracking");
            p.Controls.Add(ctc);
            MissionPlanner.GCSViews.FlightData.instance.tabControlactions.TabPages.Add(p);

            return true;
        }

        public override bool Loaded()
        {
            /*CollarTrackingControl ctc = new CollarTrackingControl();
            ctc.Dock = DockStyle.Fill;
            //TabPage p = new TabPage("Collar Tracking");
            //p.Controls.Add(ctc);
            Form f = new Form();
            f.Controls.Add(ctc);
            f.Show();*/
            //MissionPlanner.GCSViews.FlightData.instance.tabControlactions.TabPages.Add(p);
            return true;
        }

        public override bool Exit()
        {
            return true;
        }

        public override bool Loop()
        {
            if(!MissionPlanner.GCSViews.FlightData.instance.tabControlactions.TabPages.Contains(p))
                MissionPlanner.GCSViews.FlightData.instance.tabControlactions.TabPages.Add(p);
            return true;
        }

        public override bool SetupUI(int gui = 0, object data = null)
        {
            return true;
        }
    }
}
