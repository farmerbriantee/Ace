
#include "Eth_UDP.h"
#include <NativeEthernet.h>
#include <NativeEthernetUdp.h>

#include <ADC.h>
#include <ADC_util.h>

#include "BNO_RVC.h"

#include "zNMEAParser.h"

HardwareSerial* SerialIMU = &Serial1;   //IMU BNO-085

//Status LED's
constexpr auto GGAReceivedLED = 13;         //Teensy onboard LED

//timers
elapsedMillis gpsLostTimer;
elapsedMillis LEDTimer;
elapsedMillis imuDelayTimer;

bool isGGA_Updated = false;

// position of imu
bool isSwapXY = true;

//Roomba Vac mode for BNO085 and data
BNO_rvc rvc = BNO_rvc();
BNO_rvcData bnoData;

//All ethernet
Eth_UDP udp = Eth_UDP();

//16x oversampling medium speed 12 bit A/D object
ADC* adcWAS = new ADC();

//Used to set CPU speed
extern "C" uint32_t set_arm_clock(uint32_t frequency); // required prototype

/* A parser is declared with 3 handlers at most */
NMEAParser<2> parser;

// Setup procedure ------------------------
void setup()
{
    delay(100);                         //Small delay so serial can monitor start up, 100 works why wait
    //set_arm_clock(600000000);           //Set CPU speed to 600mhz, 450mhz is also a good choice.
    
    //Create the serial ports
    SerialSetup();
    Serial.println("SerialAOG, SerialRTK, SerialGPS and SerialNED initialized");

    Serial.print("CPU speed set to: ");
    Serial.println(F_CPU_ACTUAL);

    pinMode(GGAReceivedLED, OUTPUT);

    // the dash means wildcard
    parser.setErrorHandler(errorHandler);
    parser.addHandler("G-GGA", GGA_Handler);
    parser.addHandler("G-VTG", VTG_Handler);

    Serial.println("Start setup");

    Serial.println("\r\nStarting BNO-085...");
    rvc.begin(SerialIMU);

    Serial.println("\r\nStarting AutoSteer...");
    ADC_Setup();

    Serial.println("\r\nStarting Ethernet...\r\n");
    udp.Start();

    Serial.println("\r\nEnd setup, waiting for GPS...\r\n"); 
    delay(100);
}

void loop()
{
    //check if new bnoData and if true, send WAS value
    if (rvc.read(&bnoData))
    {
        SampleWAS();
    }

    //wait 40 msec then update imu data for next PANDA sentence
    if (isGGA_Updated && imuDelayTimer > 40)
    {
        imuHandler();
        isGGA_Updated = false;
    }

    //read any NMEA sent via udp 5120
    udpNMEA();

    //check for AgIO Sending 8888 
    //ReceiveUDP_PGN(); *** We don't need all that other data and pgns ***

    //check for Hello Sent 7777
    ReceiveUDP_Hello();

    if (LEDTimer > 2000)
    {
        LEDTimer = 0;

        //Do the LED Routine
        LEDRoutine();
    }

    //GGA timeout, turn off GPS LED's etc
    if (gpsLostTimer > 10000) //GGA age over 10sec
    {
        //digitalWrite(GPSRED_LED, LOW);
        //digitalWrite(GPSGREEN_LED, LOW);
    }

}//End Loop
//**************************************************************************
