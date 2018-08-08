using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.CollarTrackingPlugin.RadiationPatternMatching
{
    static class VectorAveraging
    {
        public static int direction = 0;
        public static float magnitude = 0.0F;
        public static float confidence = 0.0F;

        public static float threshold = 250.0F;

        public static bool CalculateResult()
        {
            float x_sum_dir = 0.0F;
            float y_sum_dir = 0.0F;
            int count = 0;

            foreach(KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                if (kvp.Value >= threshold)
                {
                    float angle = (float)(Math.PI * kvp.Key / 180.0);

                    x_sum_dir += (float)Math.Cos(angle) * Math.Abs(kvp.Value);
                    y_sum_dir += (float)Math.Sin(angle) * Math.Abs(kvp.Value);
                    count++;
                }
            }
            x_sum_dir = x_sum_dir / count;
            y_sum_dir = y_sum_dir / count;

            float rad = (float)Math.Atan2(y_sum_dir, x_sum_dir);
            direction = (int)(rad * (180.0 / Math.PI));
            magnitude = (float)Math.Sqrt((x_sum_dir * x_sum_dir) + (y_sum_dir * y_sum_dir));
            if (direction < 0)
                direction += 360;

            return true;
        }
    }
}
