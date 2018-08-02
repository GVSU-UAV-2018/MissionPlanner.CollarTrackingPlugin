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
        /// The resolution for the drone to do a scan in.
        /// </summary>
        static float direction_resolution = 5.0f;

        /// <summary>
        /// The current turn the system is executing.
        /// </summary>
        static int current_turn = 0;

        /// <summary>
        /// Timer for timeout during a button click for Pi paramter.
        /// </summary>
        static System.Timers.Timer CommandTimeoutTimer = new System.Timers.Timer(1000);

        //Timeout global variable
        static bool command_timeout = false;

        static byte scan_completion_status = (byte)MAVLink.MAV_RESULT.UNSUPPORTED;

        static byte scan_sequence = 1;

        //State variables that report if Pi succesfully changed param values
        private static bool vhf_snr_state_changed = false;
        private static bool vhf_freq_state_changed = false;
        private static bool if_gain_state_changed = false;
        private static bool mixer_gain_state_changed = false;
        private static bool lna_gain_state_changed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        static MavLinkRDFCommunication()
        {
            MavLinkCom.OnPacketReceived += MavLinkPacketReceived_Handler;
            CommandTimeoutTimer.Elapsed += CommandTimeoutTimer_Tick;
            CommandTimeoutTimer.Enabled = false;
        }

        public static void SetDirectionResolution(float degrees)
        {
            direction_resolution = degrees;
        }

        public static float GetDirectionResolution()
        {
            return direction_resolution;
        }

        /// <summary>
        /// Gets the current turn that the drone is 
        /// executing.
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentTurn()
        {
            return current_turn;
        }

        /// <summary>
        /// Gets the total amount of turns the
        /// drone will perform.
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfTurns()
        {
            return (int)(360.0f / GetDirectionResolution());
        }

        /// <summary>
        /// Commands the drone to turn to the specified
        /// turn, a.k.a turn * degree interval.
        /// </summary>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static bool GoToTurn(int turn)
        {
            if(turn < 0 || turn >= GetNumberOfTurns())
            {
                return false;
            }
            // THIS WILL BLOCK! hopefully OK
            if(MavLinkCom.doCommand(
                system_id,
                0,
                MAVLink.MAV_CMD.CONDITION_YAW,
                turn * GetDirectionResolution(), // yaw angle
                10,                           // yaw rate
                1,                            // direction (1 is clockwise)
                0,                            // reference frame (0 is absolute)
                0, 0, 0,                      // unused
                false))                        // require ack
            {
                current_turn = turn;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the drone to perform the next turn.
        /// </summary>
        public static bool GoToNextTurn()
        {
            return GoToTurn(GetCurrentTurn() + 1);
        }

        /// <summary>
        /// Resets the flight plan to the first waypoint.
        /// </summary>
        /// <returns></returns>
        public static bool ResetFlight()
        {
            return GoToTurn(0);
        }

        /// <summary>
        /// Sends frequency to the Pi fro processing.
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public static bool SendMavLinkFrequency(float frequency)
        {
            //Struct for sending collar frequency to RDF system
            //Has to be exactly 16 bytes
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

            CommandTimeoutTimer.Interval = 1000;
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

            CommandTimeoutTimer.Interval = 1000;
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

            CommandTimeoutTimer.Interval = 1000;
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

            CommandTimeoutTimer.Interval = 1000;
            CommandTimeoutTimer.Enabled = true;
            while (!lna_gain_state_changed &&
                !command_timeout);

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
        /// Tell the Pi to begin a scan.
        /// </summary>
        public static void DoMavLinkSNRScan(bool new_scan)
        {
            if(new_scan)
                scan_sequence = (byte)(((int)scan_sequence + 1) % 256);

            //Setting ack to false. Ok in this scenario because we don't move on
            //until MAV_IN_PROGRESS is MAV_SUCCESS
            MavLinkCom.doCommand((byte)system_id, (byte)comp_id, MAVLink.MAV_CMD.USER_1,
                scan_sequence, 0, 0, 0, 0, 0, 0, false);
        }

        /// <summary>
        /// Request the value of a data scan from the Pi.
        /// </summary>
        /// <returns></returns>
        public static void ReadMavLinkSNR()
        {
            //Setting IF Gain ID
            byte[] paramid = new byte[16];
            paramid[0] = (byte)'V';
            paramid[1] = (byte)'H';
            paramid[2] = (byte)'F';
            paramid[3] = (byte)'_';
            paramid[4] = (byte)'S';
            paramid[5] = (byte)'N';
            paramid[6] = (byte)'R';
            paramid[7] = (byte)'\0';

            MAVLink.mavlink_param_request_read_t getSNR = new MAVLink.mavlink_param_request_read_t();
            getSNR.target_system = (byte)system_id; //Drone
            getSNR.target_component = (byte)comp_id; //Pi
            getSNR.param_index = (byte)4;
            getSNR.param_id = paramid;

            MavLinkCom.sendPacket(getSNR, MavLinkCom.sysidcurrent, MavLinkCom.compidcurrent);
        }

        /// <summary>
        /// Poll the Pi to determine if the scan has completed.
        /// Poll the Pi SNR value until a value is received.
        /// </summary>
        /// <returns></returns>
        public static bool GetScanStatus()
        {
            if (scan_completion_status == (byte)MAVLink.MAV_RESULT.ACCEPTED)
            {
                ReadMavLinkSNR();
                return true;
            }
            else
            {
                DoMavLinkSNRScan(false);
                return false;
            }
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
                if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.COMMAND_ACK)
                {
                    MAVLink.mavlink_command_ack_t cmd_ack_msg = (MAVLink.mavlink_command_ack_t)msg.data;
                    scan_completion_status = cmd_ack_msg.result;
                }
                if(msg.msgid == (int)MAVLink.MAVLINK_MSG_ID.PARAM_VALUE)
                {
                    MAVLink.mavlink_param_value_t param_value_msg = (MAVLink.mavlink_param_value_t)msg.data;
                    string param_id = System.Text.Encoding.Default.GetString(param_value_msg.param_id).Trim().ToUpper();

                    if (param_id[0] == 'V' && param_id[1] == 'H'
                        && param_id[2] == 'F' && param_id[3] == '_'
                        && param_id[4] == 'S' && param_id[5] == 'N'
                        && param_id[6] == 'R')
                    {
                        float SNR = param_value_msg.param_value;
                        int direction = (int)(Math.Round(MavLinkCom.MAV.cs.yaw / 5.0) * 5);
                        RDFData.Add(new KeyValuePair<int, float>(direction, SNR));
                        RDFDataReceived(new object(), new EventArgs());
                        //Set to unsupported so that a retrigger of a new packet 
                        //does not happen. 
                        scan_completion_status = (byte)MAVLink.MAV_RESULT.UNSUPPORTED;
                        vhf_snr_state_changed = true;
                    }
                    else if (param_id[0] == 'V' && param_id[1] == 'H' 
                        && param_id[2] == 'F' && param_id[3] == '_'
                        && param_id[4] == 'F' && param_id[5] == 'R'
                        && param_id[6] == 'E' && param_id[7] == 'Q')
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
