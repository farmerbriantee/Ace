﻿
namespace AgOpenGPS
{
    public class CAHRS
    {
        //private readonly FormGPS mf;

        //Roll and heading from the IMU
        public double imuHeading = 99999, prevIMUHeading = 0, imuRoll = 0, imuPitch=0, imuYawRate = 0;

        public System.Int16 angVel;

        //actual value in degrees
        public double rollZero;

        //Roll Filter Value
        public double rollFilter;

        //is the auto steer in auto turn on mode or not
        public bool isAutoSteerAuto, isRollInvert, isDualAsIMU, isReverseOn;

        //the factor for fusion of GPS and IMU
        public double forwardComp, reverseComp, fusionWeight;

        //constructor
        public CAHRS()
        {
            rollZero = Properties.Settings.Default.setIMU_rollZero;

            rollFilter = Properties.Settings.Default.setIMU_rollFilter;

            fusionWeight = Properties.Settings.Default.setIMU_fusionWeight;

            isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn;

            forwardComp = Properties.Settings.Default.setGPS_forwardComp;

            reverseComp = Properties.Settings.Default.setGPS_reverseComp;

            isRollInvert = Properties.Settings.Default.setIMU_invertRoll;

            isDualAsIMU = Properties.Settings.Default.setIMU_isDualAsIMU;

            isReverseOn = Properties.Settings.Default.setIMU_isReverseOn;
        }
    }
}

