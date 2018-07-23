/*******************************************************
 * MavLinkRDFCommunication
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

using MissionPlanner;

namespace MissionPlanner.CollarTrackingPlugin.MavLinkRDFCommunication
{
    static class MavLinkRDFCommunication
    {
        /// <summary>
        /// Retreives the comport that is in use with the drone.
        /// </summary>
        private static MAVLinkInterface MavLinkCom = MainV2.comPort;

        /// <summary>
        /// List of SNR data retreived from the Pi.
        /// </summary>
        public static List<KeyValuePair<int, float>> RDFData = new List<KeyValuePair<int, float>>();

        /// <summary>
        /// Event that is triggered when SNR data is received
        /// from the Pi.
        /// </summary>
        public static event EventHandler RDFDataReceived;

        /// <summary>
        /// The id of the drone.
        /// </summary>
        const int system_id = 1;

        /// <summary>
        /// The id of the Pi attached to the drone.
        /// </summary>
        public static int comp_id = 177;

        /// <summary>
        /// The current waypoint the system is executing.
        /// </summary>
        public static int current_wp = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        static MavLinkRDFCommunication()
        {

        }

        /// <summary>
        /// Gets the current waypoint that the drone is 
        /// executing.
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentWP()
        {
            return current_wp;
        }

        /// <summary>
        /// Gets the total amount of waypoints
        /// in the flight plan.
        /// </summary>
        /// <returns></returns>
        public static int GetWPCount()
        {
            return MavLinkCom.getWPCount();
        }

        /// <summary>
        /// Enables or disables the packet reception
        /// event handler.
        /// </summary>
        /// <param name="doCapture"></param>
        public static void CaptureRDFData(bool doCapture)
        {
            if(doCapture)
                MavLinkCom.OnPacketReceived += RetreiveBearingStatus_Handler;
            else
                MavLinkCom.OnPacketReceived -= RetreiveBearingStatus_Handler;
        }

        /// <summary>
        /// Sets the drone to perform the next waypoint.
        /// </summary>
        public static void GoToNextWayPoint()
        {
            MavLinkCom.setWPCurrent((ushort)(GetCurrentWP() + 1)); //I think this is correct way to get curr wp
        }

        /// <summary>
        /// Resets the flight plan to the first waypoint.
        /// </summary>
        /// <returns></returns>
        public static bool ResetFlightPlan()
        {
            MavLinkCom.setWPCurrent(1); //Based on Mission Planner button click event. 1 is yaw to zero
            return true;
        }

        /// <summary>
        /// Sends frequency to the Pi fro processing.
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
            setFreq.target_system = (byte)system_id; //Drone
            setFreq.target_component = (byte)comp_id; //Pi
            setFreq.param_id = paramid;
            setFreq.param_value = frequency;
            setFreq.param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32;

            MavLinkCom.sendPacket(setFreq, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);

            return true;
        }

        /// <summary>
        /// Send signal to drone to perform RDF scan.
        /// </summary>
        public static void SendMavLinkCmdLongUser_1()
        {
            //TO DO: Check that drone is loitering first before RDF scan
            MavLinkCom.doCommand(system_id, (byte)comp_id, MAVLink.MAV_CMD.USER_1, 0, 0, 0, 0, 0, 0, 0, false);
        }

        /// <summary>
        /// Event handler for retreiving gain in each
        /// direction and for retreiving current waypoints
        /// </summary>
        /// <param name="data">A MAVLink message of the data.</param>
        /// <returns></returns>
        private static void RetreiveBearingStatus_Handler(object o, MAVLink.MAVLinkMessage msg)
        {
            if (msg.sysid == system_id && msg.compid == comp_id) //Find enum for this and make ids configurable
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

            if(msg.msgid == 42)
            {
                MAVLink.mavlink_mission_current_t mission_current_msg = (MAVLink.mavlink_mission_current_t)msg.data;
                current_wp = mission_current_msg.seq;
            }
        }
    }
}
