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
        private static byte DRONE_SYS_ID = 255;
        private static byte DRONE_COMP_ID = 0;
        private static byte RDF_SYS_ID = 255;
        private static byte RDF_COMP_ID = 0;

        public static List<KeyValuePair<int, float>> RDFData = new List<KeyValuePair<int, float>>();
        public static event EventHandler RDFDataReceived;

        /// <summary>
        /// Return instance of current MavLink comPort.
        /// Previous group created new instance, not sure
        /// if that is necessary yet.
        /// </summary>
        private static MissionPlanner.MAVLinkInterface MavLinkCom
        {
            get { return MainV2.comPort; }
        }

        static MavLinkRDFCommunication()
        {
            //Subscribe to MEMORY_VECT packet in construtor
            MavLinkCom.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.MEMORY_VECT, RetreiveBearingStatus_Handler, false);
        }

        public static bool SendMavLinkFrequency(double frequency)
        {
            //Struct for sending collar frequency to RDF system
            //This should be good to go based on prev groups design
            MAVLink.mavlink_param_ext_set_t setFreq = new MAVLink.mavlink_param_ext_set_t();
            setFreq.target_system = RDF_SYS_ID;
            setFreq.target_component = RDF_COMP_ID;
            setFreq.param_id = System.Text.Encoding.Unicode.GetBytes("VHF_FREQ");
            setFreq.param_value = BitConverter.GetBytes(frequency);
            setFreq.param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32;

            MavLinkCom.sendPacket(setFreq, RDF_SYS_ID, RDF_COMP_ID);

            return true;
        }

        public static bool ResetFlightPlan()
        {
            MavLinkCom.setWPCurrent(0); // set nav to 0
            return true;
        }

        //Loiter and unloiter
        public static bool LoiterDrone(bool doLoiter)
        {
            MAVLink.mavlink_command_long_t loiter = new MAVLink.mavlink_command_long_t();
            loiter.target_system = DRONE_SYS_ID;
            loiter.target_component = DRONE_COMP_ID;
            loiter.param1 = 0;
            loiter.param2 = 0;
            loiter.param3 = 0;
            loiter.param4 = 0;

            if(doLoiter)
                loiter.command = (ushort)MAVLink.MAV_GOTO.DO_HOLD; //loiter
            else
                loiter.command = (ushort)MAVLink.MAV_GOTO.DO_CONTINUE; //stop loiter
            MavLinkCom.sendPacket(loiter, DRONE_SYS_ID, DRONE_COMP_ID);

            return true;
        }

        /// <summary>
        /// Event handler for retreiving gain in each
        /// direction.
        /// </summary>
        /// <param name="data">A MAVLink message of the data.</param>
        /// <returns></returns>
        private static bool RetreiveBearingStatus_Handler(MAVLink.MAVLinkMessage data)
        {
            if(data.compid == RDF_COMP_ID)
            {
                int direction;
                float SNR;
                direction = (int)MavLinkCom.GetParam("CONDITION_YAW"); //this is a guess for now
                SNR = (float)Convert.ToDouble(data.data); //this may need further conversion if payload is more than just SNR
                RDFData.Add(new KeyValuePair<int, float>(direction, SNR));

                if (RDFDataReceived != null)
                    RDFDataReceived(new object(), new EventArgs());

                return true;
            }
            return false;
        }
    }
}
