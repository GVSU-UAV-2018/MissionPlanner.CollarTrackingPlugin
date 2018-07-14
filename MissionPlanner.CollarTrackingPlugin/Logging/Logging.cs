using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.CollarTrackingPlugin.Logging
{
    class Logging
    {
        public string _File = "";

        public Logging(string location)
        {
            _File = location + @"\log_" +  
                DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".csv";
            System.IO.File.Create(_File);
        }

        public void AddData()
        {
            System.IO.StreamWriter w = new System.IO.StreamWriter(_File);
            foreach (KeyValuePair<int, float> kvp in MavLinkRDFCommunication.MavLinkRDFCommunication.RDFData)
            {
                w.WriteLine(kvp.Key + ", " + kvp.Value);
            }
            w.Close();
        }
    }
}
