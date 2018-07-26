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
        /// Timer for timeout during a button click for Pi paramter.
        /// </summary>
        static System.Timers.Timer CommandTimeoutTimer = new System.Timers.Timer(1000);

        //Timeout global variable
        static bool command_timeout = false;

        //State variables that report if Pi succesfully changed param values
        private static bool vhf_freq_state_changed = false;
        private static bool if_gain_state_changed = false;
        private static bool mixer_gain_state_changed = false;
        private static bool lna_gain_state_changed = false;

        private static uint prev_msg_id;

        /// <summary>
        /// Constructor
        /// </summary>
        static MavLinkRDFCommunication()
        {
            MavLinkCom.OnPacketReceived += MavLinkPacketReceived_Handler;
            CommandTimeoutTimer.Elapsed += CommandTimeoutTimer_Tick;
            CommandTimeoutTimer.Enabled = false;
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

            CommandTimeoutTimer.Enabled = true;
            while (!vhf_freq_state_changed &&
                !command_timeout);

            bool retVal;

            if (vhf_freq_state_changed)
                retVal = true;
            else
                retVal = false;

            CommandTimeoutTimer.Enabled = false;
            command_timeout = false;
            vhf_freq_state_changed = false;

            return retVal;
        }

        public static bool SendMavLinkIFGain(int gain)
        {
            //Setting IF Gain ID
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'I';
            paramid[1] = (byte)'F';
            paramid[2] = (byte)'_';
            paramid[3] = (byte)'G';
            paramid[4] = (byte)'A';
            paramid[5] = (byte)'I';
            paramid[6] = (byte)'N';
            paramid[7] = (byte)'\0';

            MAVLink.mavlink_param_set_t setIFGain = new MAVLink.mavlink_param_set_t();
            setIFGain.target_system = (byte)system_id; //Drone
            setIFGain.target_component = (byte)comp_id; //Pi
            setIFGain.param_id = paramid;
            setIFGain.param_value = gain;
            setIFGain.param_type = (byte)MAVLink.MAV_PARAM_TYPE.INT32;

            MavLinkCom.sendPacket(setIFGain, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);

            CommandTimeoutTimer.Enabled = true;
            while (!if_gain_state_changed &&
                !command_timeout) ;

            bool retVal;

            if (if_gain_state_changed)
                retVal = true;
            else
                retVal = false;

            CommandTimeoutTimer.Enabled = false;
            command_timeout = false;
            if_gain_state_changed = false;

            return retVal;
        }

        public static bool SendMavLinkMixerGain(int gain)
        {
            //Setting IF Gain ID
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'M';
            paramid[1] = (byte)'I';
            paramid[2] = (byte)'X';
            paramid[3] = (byte)'_';
            paramid[4] = (byte)'G';
            paramid[5] = (byte)'A';
            paramid[6] = (byte)'I';
            paramid[7] = (byte)'N';
            paramid[8] = (byte)'\0';

            MAVLink.mavlink_param_set_t setMixerGain = new MAVLink.mavlink_param_set_t();
            setMixerGain.target_system = (byte)system_id; //Drone
            setMixerGain.target_component = (byte)comp_id; //Pi
            setMixerGain.param_id = paramid;
            setMixerGain.param_value = gain;
            setMixerGain.param_type = (byte)MAVLink.MAV_PARAM_TYPE.INT32;

            MavLinkCom.sendPacket(setMixerGain, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);

            CommandTimeoutTimer.Enabled = true;
            while (!mixer_gain_state_changed &&
                !command_timeout) ;

            bool retVal;

            if (mixer_gain_state_changed)
                retVal = true;
            else
                retVal = false;

            CommandTimeoutTimer.Enabled = false;
            command_timeout = false;
            mixer_gain_state_changed = false;

            return retVal;
        }

        public static bool SendMavLinkLNAGain(int gain)
        {
            //Setting IF Gain ID
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'L';
            paramid[1] = (byte)'N';
            paramid[2] = (byte)'A';
            paramid[3] = (byte)'_';
            paramid[4] = (byte)'G';
            paramid[5] = (byte)'A';
            paramid[6] = (byte)'I';
            paramid[7] = (byte)'N';
            paramid[8] = (byte)'\0';

            MAVLink.mavlink_param_set_t setLNAGain = new MAVLink.mavlink_param_set_t();
            setLNAGain.target_system = (byte)system_id; //Drone
            setLNAGain.target_component = (byte)comp_id; //Pi
            setLNAGain.param_id = paramid;
            setLNAGain.param_value = gain;
            setLNAGain.param_type = (byte)MAVLink.MAV_PARAM_TYPE.INT32;

            MavLinkCom.sendPacket(setLNAGain, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);

            CommandTimeoutTimer.Enabled = true;
            while (!lna_gain_state_changed &&
                !command_timeout) ;

            bool retVal;

            if (lna_gain_state_changed)
                retVal = true;
            else
                retVal = false;

            CommandTimeoutTimer.Enabled = false;
            command_timeout = false;
            lna_gain_state_changed = false;

            return retVal;
        }

        /// <summary>
        /// Sends a param_value to pi as an acknowledgement.
        /// </summary>
        /// <param name="value"></param>
        private static void SendVHF_SNRPiAcknowledge(float value)
        {
            MAVLink.mavlink_param_value_t ack = new MAVLink.mavlink_param_value_t();
            //Struct for sending collar frequency to RDF system
            //This should be good to go based on prev groups design
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'V';
            paramid[1] = (byte)'H';
            paramid[2] = (byte)'F';
            paramid[3] = (byte)'_';
            paramid[4] = (byte)'S';
            paramid[5] = (byte)'N';
            paramid[6] = (byte)'R';
            paramid[7] = (byte)'\0';

            ack.param_id = paramid;
            ack.param_value = value;
            MavLinkCom.sendPacket(ack, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);
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
        private static void MavLinkPacketReceived_Handler(object o, MAVLink.MAVLinkMessage msg)
        {
            if (msg.sysid == system_id && msg.compid == comp_id) //Get Pi messages
            {
                if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.MEMORY_VECT)
                {
                    MAVLink.mavlink_memory_vect_t mem_vect_msg = (MAVLink.mavlink_memory_vect_t)msg.data;
                    byte[] mem_vect_data = mem_vect_msg.value;
                    ushort mem_vect_addr = mem_vect_msg.address;

                    float SNR = 0;
                    uint msg_id;
                    int direction = 0;

                    if (System.BitConverter.IsLittleEndian)
                    {
                        byte temp = mem_vect_data[0];
                        mem_vect_data[0] = mem_vect_data[3];
                        mem_vect_data[3] = temp;
                        temp = mem_vect_data[1];
                        mem_vect_data[1] = mem_vect_data[2];
                        mem_vect_data[2] = temp;
                    }

                    SNR = System.BitConverter.ToSingle(mem_vect_data, 0);
                    msg_id = mem_vect_addr;

                    SendVHF_SNRPiAcknowledge(SNR);
                    direction = (int)(Math.Round(MavLinkCom.MAV.cs.yaw / 5.0) * 5); //current state of drone

                    //We do not want double SNR data recorded
                    if (prev_msg_id != msg_id)
                    {
                        prev_msg_id = msg_id;
                        RDFData.Add(new KeyValuePair<int, float>(direction, SNR));
                        RDFDataReceived(new object(), new EventArgs());
                    }

                    prev_msg_id = msg_id;
                }
                //Received values from Pi
                /*if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.PARAM_SET)
                {
                    MAVLink.mavlink_param_set_t param_set_msg = (MAVLink.mavlink_param_set_t)msg.data;
                    string param_id = System.Text.Encoding.Default.GetString(param_set_msg.param_id).Trim().ToUpper();

                    if (param_id[0] == 'V' && param_id[1] == 'H' 
                        && param_id[2] == 'F')
                    {
                        int direction;
                        float SNR;
                        direction = (int)MavLinkCom.MAV.cs.yaw; //current state of drone
                        SNR = (float)Convert.ToDouble(param_set_msg.param_value); //this may need further conversion if payload is more than just SNR

                        //We do not want double SNR data recorded
                        if (prev_msg_id != msg.msgid)
                        {
                            RDFData.Add(new KeyValuePair<int, float>(direction, SNR));
                            RDFDataReceived(new object(), new EventArgs());

                            System.Windows.Forms.MessageBox.Show("Pi SNR: " + SNR);
                        }

                        SendVHF_SNRPiAcknowledge(SNR);
                    }
                }*/
                else if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.PARAM_VALUE)
                {
                    MAVLink.mavlink_param_value_t param_value_msg = (MAVLink.mavlink_param_value_t)msg.data;
   
                    string param_id = System.Text.Encoding.Default.GetString(param_value_msg.param_id).Trim().ToUpper();
                    if (param_id[0] == 'V' && param_id[1] == 'H' 
                        && param_id[2] == 'F')
                    {
                        vhf_freq_state_changed = true;
                    }
                    else if (param_id[0] == 'I' && param_id[1] == 'F')
                    {
                        if_gain_state_changed = true;
                    }
                    else if (param_id[0] == 'M' && param_id[1] == 'I'
                        && param_id[2] == 'X')
                    {
                        mixer_gain_state_changed = true;
                    }
                    else if (param_id[0] == 'L' && param_id[1] == 'N'
                        && param_id[2] == 'A')
                    {
                        lna_gain_state_changed = true;
                    }
                }
            }

            if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT)
            {
                MAVLink.mavlink_mission_current_t mission_current_msg = (MAVLink.mavlink_mission_current_t)msg.data;
                current_wp = mission_current_msg.seq;
            }
        }

        /// <summary>
        /// Sets timeout to true when a button is clicked
        /// but no acknowledge is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CommandTimeoutTimer_Tick(object sender, EventArgs e)
        {
            command_timeout = true;
        }
    }
}
