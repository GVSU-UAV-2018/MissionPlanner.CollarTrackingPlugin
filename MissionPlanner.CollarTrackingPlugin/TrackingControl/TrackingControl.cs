using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.CollarTrackingPlugin.TrackingControl
{
    static class TrackingControl
    {
        public static double Frequency
        { get; set; }

        public static void ScanDirection(int degree)
        {
            for (long i = 0; i < 1000000000; i++) ;
        }
    }
}
