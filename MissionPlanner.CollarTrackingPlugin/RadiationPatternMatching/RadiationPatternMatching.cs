﻿/*******************************************************
 * RadiationPatternMatching
 * 
 * GVSU Team UAV 2018
 * 
 * Implements all communication between the Raspberry
 * Pi and the base station Collar Tracking Control.
 ******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.CollarTrackingPlugin.RadiationPatternMatching
{
    static class RadiationPatternMatching
    {
        /// <summary>
        /// Retreived angle from North.
        /// </summary>
        public static float DegreesFromNorth = 0.0F;

        /// <summary>
        /// Confidence in measurement.
        /// </summary>
        public static float Confidence = 0.0F;

        /// <summary>
        /// File to compare retreived signals wth.
        /// </summary>
        public static string AntennaPatternFile = @"C:\Yagi_3Elm_RadPattern.csv";

        /// <summary>
        /// Constructor
        /// </summary>
        static RadiationPatternMatching()
        {

        }

        /// <summary>
        /// Performs the pattern matching analysis by 
        /// loading the radiation pattern and comparing
        /// it with received SNR signals via a cross
        /// correlation.
        /// </summary>
        /// <returns></returns>
        public static bool PerformPatternMatchingAnalysis()
        {
            //Read radiation pattern file. File should be in increments
            //of 1 from 0 to 359
            System.IO.StreamReader reader = new System.IO.StreamReader(AntennaPatternFile);
            Dictionary<int, float> rad_pattern = new Dictionary<int, float>();
            string line;
            int i = 0;
            while((line = reader.ReadLine()) != null)
            {
                KeyValuePair<int, float> point;
                string[] split = line.Split(',');
                try
                {
                    rad_pattern.Add(
                        Convert.ToInt32(split[0]), 
                        (float)Convert.ToDouble(split[1]));
                }
                catch
                {
                    return false;
                }
            }

            //Two lists are created to contained data lined up by direction
            //received. The allows for cross correlation to be taken.
            Dictionary<int, int> directions = new Dictionary<int, int>();
            List<float> rad_pattern_points = new List<float>();
            List<float> retrieved_points = new List<float>();

            foreach(KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                if(rad_pattern.ContainsKey(kvp.Key))
                {
                    directions.Add(i++, kvp.Key);
                    rad_pattern_points.Add(rad_pattern[kvp.Key]);
                    retrieved_points.Add(kvp.Value);

                }
            }

            //Do CC
            CircularCrossCorrelation(rad_pattern_points, retrieved_points);
            //CC tells how many increments true direction is away from rad
            //pattern direction. Use dictornary to retrieve true direction.
            DegreesFromNorth = directions[(int)DegreesFromNorth];

            return true;
        }

        /// <summary>
        /// Performs a correlation between two lists.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void CircularCrossCorrelation(List<float> x, List<float> y)
        {
            /* Calculate the mean of the two series x[], y[] */
            double mx = 0;
            double my = 0;
            double sx;
            double sy;
            double sxy;
            double denom = 0.0;

            int n = y.Count;
            int i;
            int j;
            int delay;

            double r;
            double max_r = -2;
            int max_d = -1;


            for (i = 0; i < n; i++)
            {
                mx += x[i];
                my += y[i];
            }
            mx /= n;
            my /= n;

            /* Calculate the denominator */
            sx = 0;
            sy = 0;
            for (i = 0; i < n; i++)
            {
                sx += (x[i] - mx) * (x[i] - mx);
                sy += (y[i] - my) * (y[i] - my);
            }
            denom = Math.Sqrt(sx * sy);

            /* Calculate the correlation series */
            for (delay = 0; delay < n; delay++)
            {
                sxy = 0;
                for (i = 0; i < n; i++)
                {
                    j = i + delay;
                    while (j < 0)
                        j += n;
                    j %= n;
                    sxy += (x[i] - mx) * (y[j] - my);
                }
                r = sxy / denom;

                /* r is the correlation coefficient at "delay" */
                if (r > max_r)
                {
                    max_r = r;
                    max_d = delay;
                }
            }

            Confidence = (float)max_r;
            DegreesFromNorth = max_d;
        }
    }
}
