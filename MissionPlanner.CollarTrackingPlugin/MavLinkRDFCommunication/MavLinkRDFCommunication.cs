using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MissionPlanner;

namespace MissionPlanner.CollarTrackingPlugin.MavLinkRDFCommunication
{
    static class MavLinkRDFCommunication
    {
        private static MAVLinkInterface MavLinkCom = MainV2.comPort;

        public static List<KeyValuePair<int, float>> RDFData = new List<KeyValuePair<int, float>>();
        public static event EventHandler RDFDataReceived;

        static MavLinkRDFCommunication()
        {

        }

        public static int GetCurrentWP()
        {
            return MavLinkCom.getRequestedWPNo(); //again i think this is correct
        }

        public static int GetWPCount()
        {
            return MavLinkCom.getWPCount();
        }

        public static void CaptureRDFData(bool doCapture)
        {
            if(doCapture)
                MavLinkCom.OnPacketReceived += RetreiveBearingStatus_Handler;
            else
                MavLinkCom.OnPacketReceived -= RetreiveBearingStatus_Handler;
        }

        public static void GoToNextWayPoint()
        {
            MavLinkCom.setWPCurrent((ushort)(MavLinkCom.getRequestedWPNo() + 1)); //I think this is correct way to get curr wp
        }

        public static bool ResetFlightPlan()
        {
            MavLinkCom.setWPCurrent(0); // set nav to 0. Might be a better way to do this
            return true;
        }

        /// <summary>
        /// Works
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public static bool SendMavLinkFrequency(float frequency)
        {
            //Struct for sending collar frequency to RDF system
            //This should be good to go based on prev groups design
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'V';
            paramid[1] = (byte)'H';
            paramid[2] = (byte)'F';
            paramid[3] = (byte)'_';
            paramid[4] = (byte)'F';
            paramid[5] = (byte)'R';
            paramid[6] = (byte)'E';
            paramid[7] = (byte)'Q';
            paramid[8] = (byte)'\0';

            MAVLink.mavlink_param_set_t setFreq = new MAVLink.mavlink_param_set_t();
            setFreq.target_system = (byte)1; //Drone
            setFreq.target_component = (byte)177; //Pi
            setFreq.param_id = paramid;
            setFreq.param_value = frequency;
            setFreq.param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32;

            MavLinkCom.sendPacket(setFreq, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);

            return true;
        }

        public static void SendMavLinkCmdLongUser_1()
        {
            //TO DO: Check that drone is loitering first before RDF scan
            MavLinkCom.doCommand(1, 177, MAVLink.MAV_CMD.USER_1, 0, 0, 0, 0, 0, 0, 0, false);
        }

        /// <summary>
        /// Event handler for retreiving gain in each
        /// direction. Works
        /// </summary>
        /// <param name="data">A MAVLink message of the data.</param>
        /// <returns></returns>
        private static void RetreiveBearingStatus_Handler(object o, MAVLink.MAVLinkMessage msg)
        {
            if (msg.sysid == 1 && msg.compid == 177) //Find enum for this and make ids configurable
            {
                //Received values from Pi
                if(msg.msgid == 23)
                {
                    MAVLink.mavlink_param_set_t param_set_msg = (MAVLink.mavlink_param_set_t)msg.data;
                    //if (String.Equals(System.Text.Encoding.Default.GetString(param_set_msg.param_id).Trim(), "VHF_SNR"))
                    //{
                        int direction;
                        float SNR;
                        direction = (int)MavLinkCom.MAV.cs.yaw; //current state of drone
                        SNR = (float)Convert.ToDouble(param_set_msg.param_value); //this may need further conversion if payload is more than just SNR
                        
                        RDFData.Add(new KeyValuePair<int, float>(direction, SNR));

                        RDFDataReceived(new object(), new EventArgs());
                    //}
                }
            }
        }
    }
}
