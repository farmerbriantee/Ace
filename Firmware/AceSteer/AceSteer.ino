    /*
    * UDP Autosteer code for ENC28J60 module
    * For AgOpenGPS
    * 4 Feb 2021, Brian Tischler
    * Like all Arduino code - copied from somewhere else :)
    * So don't claim it as your own
    */

    #include <Ethernet.h>
    #include <EthernetUdp.h>
    #include <EEPROM.h> 

    #define WATCHDOG_LIMIT 20

    //Dir1 for Cytron Dir, Both L and R enable for IBT2
    #define DIR1_RL_ENABLE  4  //PD4

    //PWM1 for Cytron PWM, Left PWM for IBT2
    #define PWM1_LPWM  3  //PD3

    //--------------------------- Switch Input Pins ------------------------
    #define STEERSW_PIN 6 //PD6
    #define WORKSW_PIN 7  //PD7
    #define REMOTE_PIN 8  //PB0
 
    //Define sensor pin for current or pressure sensor
    #define ANALOG_SENSOR_PIN A0

    #define CONST_180_DIVIDED_BY_PI 57.2957795130823

    ////////////////// User Settings /////////////////////////  

    //How many degrees before decreasing Max PWM
    #define LOW_HIGH_DEGREES 3.0

    /*  PWM Frequency -> 
    *   490hz (default) = 0
    *   122hz = 1
    *   3921hz = 2
    */
    #define PWM_Frequency 0
  
    // Change this number to reset and reload default parameters To EEPROM
    #define EEP_Ident 0x5417

    struct ConfigIP {
        uint8_t ipOne = 192;
        uint8_t ipTwo = 168;
        uint8_t ipThree = 5;
    };  ConfigIP networkAddress;   //3 bytes

    /////////////////////////////////////////////////////////////////////////////////////////////////
    
    // ethernet interface ip address and host address 126
    static uint8_t myip[] = { 0,0,0,126 };

    //mask
    static uint8_t mask[] = { 255,255,255,0 };

    //this is port of this autosteer module
    uint16_t portMy = 5126; 
  
    //sending back to where and which port
    static uint8_t ipDestination[] = {0,0,0, 255};
    uint16_t portDestination = 9999; //AOG port that listens
  
    // ethernet mac address - must be unique on your network - 126 = 7E
    static uint8_t mymac[] = { 0x00,0x00,0x56,0x00,0x00,0x7E };
    uint8_t udpData[64];

    uint16_t counter = 0;
      
    //loop time variables in microseconds  
    const uint16_t LOOP_TIME = 200;  //40Hz    
    uint32_t lastTime = LOOP_TIME;
    uint32_t currentTime = LOOP_TIME;

    uint8_t watchdogTimer = 50;

    //Heart beat hello AgIO
    uint8_t helloFromAutoSteer[] = { 128, 129, 126, 126, 5, 0, 0, 0, 0, 0, 71 };
    int16_t helloSteerPosition = 0;

    //fromAutoSteerData FD 253 - ActualSteerAngle*100 -5,6, SwitchByte-7, pwmDisplay-8
    uint8_t PGN_253[] = {128, 129, 126, 253, 8, 0, 0, 0, 0, 0,0,0,0, 12 };
    int8_t PGN_253_Size = sizeof(PGN_253) - 1;

    //fromAutoSteerData FD 250 - sensor values etc
    uint8_t PGN_250[] = { 128, 129, 126, 250, 8, 0, 0, 0, 0, 0,0,0,0, 12 };
    int8_t PGN_250_Size = sizeof(PGN_250) - 1;
 
    //EEPROM
    int16_t EEread = 0;

    //Relays
    bool isRelayActiveHigh = true;
    uint8_t relay = 0, relayHi = 0, uTurn = 0;
    uint8_t xte = 0;
    
    //Switches
    uint8_t remoteSwitch = 0, workSwitch = 0, steerSwitch = 1, switchByte = 0;

    uint8_t aog2Count = 0;
    float sensorReading = 0, sensorSample;

    //Steer switch button  ***********************************************************************************************************
    uint8_t currentState = 1, reading, previous = 0;
    uint8_t pulseCount = 0; // Steering Wheel Encoder
    bool encEnable = false; //debounce flag
    uint8_t thisEnc = 0, lastEnc = 0;

    //On Off
    uint8_t guidanceStatus = 0;

    //speed sent as *10
    float gpsSpeed = 0;
  
    //steering variables
    float steerAngleActual = 0;
    float steerAngleSetPoint = 0; //the desired angle from AgOpen
    int16_t steeringPosition = 0; //from steering sensor
    float steerAngleError = 0; //setpoint - actual
  
    //pwm variables
    int16_t pwmDrive = 0, pwmDisplay = 0;
    float pValue = 0;
    float errorAbs = 0;
    float highLowPerDeg = 0; 

    //Variables for settings  
    struct Storage {
        uint8_t Kp = 120;  //proportional gain
        uint8_t lowPWM = 30;  //band of no action
        int16_t wasOffset = 0;
        uint8_t minPWM = 25;
        uint8_t highPWM = 160;//max PWM value
        float steerSensorCounts = 30;        
        float AckermanFix = 1;     //sent as percent
    };  Storage steerSettings;  //11 bytes

    //Variables for settings - 0 is false  
    struct Setup {
        uint8_t InvertWAS = 0;
        uint8_t IsRelayActiveHigh = 0; //if zero, active low (default)
        uint8_t MotorDriveDirection = 0;
        uint8_t SingleInputWAS = 1;
        uint8_t CytronDriver = 1;
        uint8_t SteerSwitch = 0;  //1 if switch selected
        uint8_t SteerButton = 0;  //1 if button selected
        uint8_t ShaftEncoder = 0;
        uint8_t PressureSensor = 0;
        uint8_t CurrentSensor = 0;
        uint8_t PulseCountMax = 5; 
        uint8_t IsDanfoss = 0; 
    };  Setup steerConfig;          //9 bytes

    //reset function
    void(* resetFunc) (void) = 0;

    // An EthernetUDP instance to let us send and receive packets over UDP
    EthernetUDP Udp;
    EthernetUDP Hello;


    void setup()
    {
        //delay(4000);

        //PWM rate settings
        if (PWM_Frequency == 1)
        {
            TCCR2B = TCCR2B & (B11111000 | B00000110);    // set timer 2 to 256 for PWM frequency of   122.55 Hz
            TCCR1B = TCCR1B & (B11111000 | B00000100);    // set timer 1 to 256 for PWM frequency of   122.55 Hz
        }

        else if (PWM_Frequency == 2)
        {
            TCCR1B = TCCR1B & (B11111000 | B00000010);    // set timer 1 to 8 for PWM frequency of  3921.16 Hz
            TCCR2B = TCCR2B & (B11111000 | B00000010);    // set timer 2 to 8 for PWM frequency of  3921.16 Hx
        }

        //keep pulled high and drag low to activate, noise free safe   
        pinMode(WORKSW_PIN, INPUT_PULLUP);
        pinMode(STEERSW_PIN, INPUT_PULLUP);
        pinMode(REMOTE_PIN, INPUT_PULLUP);
        pinMode(DIR1_RL_ENABLE, OUTPUT);

        //set up communication
        Serial.begin(115200);

        EEPROM.get(0, EEread);              // read identifier

        if (EEread != EEP_Ident)   // check on first start and write EEPROM
        {
            EEPROM.put(0, EEP_Ident);
            EEPROM.put(10, steerSettings);
            EEPROM.put(40, steerConfig);
            EEPROM.put(60, networkAddress);
        }
        else
        {
            EEPROM.get(10, steerSettings);     // read the Settings
            EEPROM.get(40, steerConfig);
            EEPROM.get(60, networkAddress);
        }

        // for PWM High to Low interpolator
        highLowPerDeg = ((float)(steerSettings.highPWM - steerSettings.lowPWM)) / LOW_HIGH_DEGREES;

        //grab the ip from EEPROM
        myip[0] = networkAddress.ipOne;
        myip[1] = networkAddress.ipTwo;
        myip[2] = networkAddress.ipThree;

        ipDestination[0] = networkAddress.ipOne;
        ipDestination[1] = networkAddress.ipTwo;
        ipDestination[2] = networkAddress.ipThree;

        // start the Ethernet
        Ethernet.begin(mymac, myip);

        //subnet mask = 24
        Ethernet.setSubnetMask(mask);

        // Check for Ethernet hardware present
        if (Ethernet.hardwareStatus() == EthernetNoHardware) {
            Serial.println("Ethernet shield was not found.  Sorry, can't run without hardware. :(");
            while (true) {
                delay(1); // do nothing, no point running without Ethernet hardware
            }
        }
        if (Ethernet.linkStatus() == LinkOFF) {
            Serial.println("Ethernet cable is not connected.");
        }

        // start UDP listen to module port
        Udp.begin(8888);

        // start UDP listen to hello port
        Hello.begin(7777);

        Serial.println("This IP:");
        Serial.print(myip[0]);
        Serial.print(".");
        Serial.print(myip[1]);
        Serial.print(".");
        Serial.print(myip[2]);
        Serial.println(".126");

        Serial.println("Setup complete, waiting for AgOpenGPS");

    }// End of Setup

    void loop()
    {
        // Loop triggers every 200 msec
        currentTime = millis();

        if (currentTime - lastTime >= LOOP_TIME)
        {
            lastTime = currentTime;

            //reset debounce
            encEnable = true;

            //If connection lost to AgOpenGPS, the watchdog will count up and turn off steering
            if (watchdogTimer < 30) watchdogTimer++;

            //steerSwitch = 0;  //default to on

            //read all the switches
            workSwitch = digitalRead(WORKSW_PIN);  // read work switch

            if (steerConfig.SteerSwitch == 1)         //steer switch on - off
            {
                steerSwitch = digitalRead(STEERSW_PIN); //read auto steer enable switch open = 0n closed = Off
            }
            else if (steerConfig.SteerButton == 1)     //steer Button momentary
            {
                reading = digitalRead(STEERSW_PIN);
                if (reading != previous)
                {
                    if (reading == LOW && previous == HIGH)
                    {
                        //on
                        if (steerSwitch == 0) steerSwitch = 1;
                        else (steerSwitch = 0);
                    }
                    previous = reading;
                }
            }

            //if (steerConfig.ShaftEncoder && pulseCount >= steerConfig.PulseCountMax)
            //{
            //    steerSwitch = 1; // it turned off
            //}

            // //Pressure sensor?
            //if (steerConfig.PressureSensor)
            //{
            //    sensorSample = (float)analogRead(ANALOG_SENSOR_PIN);
            //    sensorSample *= 0.25;
            //    sensorReading = sensorReading * 0.6 + sensorSample * 0.4;
            //    if (sensorReading >= steerConfig.PulseCountMax)
            //    {
            //        steerSwitch = 1; // it turned off
            //    }
            //}

            ////Current sensor?
            //if (steerConfig.CurrentSensor)
            //{
            //    sensorSample = (float)analogRead(ANALOG_SENSOR_PIN);
            //    sensorSample = (abs(512 - sensorSample)) * 0.5;
            //    sensorReading = sensorReading * 0.7 + sensorSample * 0.3;
            //    if (sensorReading >= steerConfig.PulseCountMax)
            //    {
            //        steerSwitch = 1; // it turned off
            //    }
            //}

            remoteSwitch = digitalRead(REMOTE_PIN); //read auto steer enable switch open = 0n closed = Off
            switchByte = 0;
            switchByte |= (remoteSwitch << 2); //put remote in bit 2
            switchByte |= (steerSwitch << 1);   //put steerswitch status in bit 1 position
            switchByte |= workSwitch;

            //check encoder counter
            if (steerConfig.ShaftEncoder)
            {
                if (encEnable)
                {
                    thisEnc = digitalRead(REMOTE_PIN);
                    if (thisEnc != lastEnc)
                    {
                        lastEnc = thisEnc;
                        if (lastEnc) EncoderFunc();
                    }
                }
            }

        } //end of timed loop

        //This runs continuously, outside of the timed loop, keeps checking for new udpData
        CheckUDP();

        CheckHello();


    } // end of main loop

//callback when received packets
    void CheckUDP(void)
    {
        int packetSize = Udp.parsePacket();

        if (packetSize > 4)
        {
            //read into buffer
            Udp.readBytes(udpData, packetSize);

            uint16_t rem_port = Udp.remotePort();

            //Serial.print("This Port:  "); Serial.println(rem_port);

            //coming from IMU_WAS
            //if (rem_port == 5120)
            {
                if (udpData[0] == 128 && udpData[1] == 129 && udpData[2] == 121) //IMU/WAS PGN
                {
                    //Stream from WAS and IMU
                    if (udpData[3] == 249)
                    {
                        counter++;

                        steeringPosition = (udpData[5] | udpData[6] << 8);
                        //helloSteerPosition = steeringPosition - 2048;

                        if (steerConfig.InvertWAS)
                        {
                            steeringPosition = (steeringPosition - 2048 - steerSettings.wasOffset);
                            steerAngleActual = (float)(steeringPosition) / -steerSettings.steerSensorCounts;
                        }
                        else
                        {
                            steeringPosition = (steeringPosition - 2048 + steerSettings.wasOffset);
                            steerAngleActual = (float)(steeringPosition) / steerSettings.steerSensorCounts;
                        }

                        //Ackerman fix
                        if (steerAngleActual < 0) steerAngleActual = (steerAngleActual * steerSettings.AckermanFix);
                        
                        if (watchdogTimer < WATCHDOG_LIMIT)
                        {
                            steerAngleError = steerAngleActual - steerAngleSetPoint;   //calculate the steering error
                            calcSteeringPID();  //do the pid
                            motorDrive();       //out to motors the pwm value
                        }
                        else
                        {
                            //we've lost the comm to AgOpenGPS, or just stop request
                            pwmDrive = 0; //turn off steering motor
                            motorDrive(); //out to motors the pwm value
                            pulseCount = 0;
                        }
                    }
                    return;
                }
            }

            if (rem_port == 9999)
            {
                //Coming from agio
                if (udpData[0] == 128 && udpData[1] == 129 && udpData[2] == 127) //Data
                {
                    if (udpData[3] == 254)
                    {
                        gpsSpeed = ((float)(udpData[5] | udpData[6] << 8)) * 0.1;

                        guidanceStatus = udpData[7];

                        //Bit 8,9    set point steer angle * 100 is sent
                        steerAngleSetPoint = ((float)(udpData[8] | udpData[9] << 8)) * 0.01; //high low bytes

                        //Serial.println(gpsSpeed); 

                        if ((bitRead(guidanceStatus, 0) == 0) || (gpsSpeed < 0.1) || (steerSwitch == 1))
                        {
                            watchdogTimer = WATCHDOG_LIMIT; //turn off steering motor
                        }
                        else          //valid conditions to turn on autosteer
                        {
                            watchdogTimer = 0;  //reset watchdog
                        }

                        //Bit 10 Tram 
                        xte = udpData[10];

                        //Bit 11
                        relay = udpData[11];

                        //Bit 12
                        relayHi = udpData[12];

                        //-------------------------------------------
                        //Send to agopenGPS

                        int16_t sa = (int16_t)(steerAngleActual * 100);

                        PGN_253[5] = (uint8_t)sa;
                        PGN_253[6] = sa >> 8;

                        //heading         
                        PGN_253[7] = (uint8_t)9999;
                        PGN_253[8] = 9999 >> 8;

                        //roll
                        PGN_253[9] = (uint8_t)8888;
                        PGN_253[10] = 8888 >> 8;

                        PGN_253[11] = switchByte;
                        PGN_253[12] = (uint8_t)pwmDisplay;

                        //checksum
                        int16_t CK_A = 0;
                        for (uint8_t i = 2; i < PGN_253_Size; i++)
                            CK_A = (CK_A + PGN_253[i]);

                        PGN_253[PGN_253_Size] = CK_A;

                        //off to AOG
                        Udp.beginPacket(ipDestination, portDestination);
                        Udp.write(PGN_253, sizeof(PGN_253));
                        Udp.endPacket();

                        //Steer Data 2 -------------------------------------------------
                        if (steerConfig.PressureSensor || steerConfig.CurrentSensor)
                        {
                            if (aog2Count++ > 2)
                            {
                                //Send fromAutosteer2
                                PGN_250[5] = (byte)sensorReading;

                                //add the checksum for AOG2
                                CK_A = 0;
                                for (uint8_t i = 2; i < PGN_250_Size; i++)
                                {
                                    CK_A = (CK_A + PGN_250[i]);
                                }
                                PGN_250[PGN_250_Size] = CK_A;

                                //off to AOG
                                Udp.beginPacket(ipDestination, portDestination);
                                Udp.write(PGN_250, sizeof(PGN_250));
                                Udp.endPacket();

                                aog2Count = 0;
                            }
                        }

                        //Serial.println(steerAngleActual); 
                        //--------------------------------------------------------------------------    
                    }

                    //steer settings
                    else if (udpData[3] == 252)
                    {
                        //PID values
                        steerSettings.Kp = udpData[5];   // read Kp from AgOpenGPS

                        steerSettings.highPWM = udpData[6]; // read high pwm

                        steerSettings.lowPWM = udpData[7];   // read lowPWM from AgOpenGPS              

                        steerSettings.minPWM = udpData[8]; //read the minimum amount of PWM for instant on

                        float temp = steerSettings.minPWM;
                        temp *= 1.2;
                        steerSettings.lowPWM = (uint8_t)temp;

                        steerSettings.steerSensorCounts = udpData[9]; //sent as setting displayed in AOG

                        steerSettings.wasOffset = (udpData[10]);  //read was zero offset Lo

                        steerSettings.wasOffset |= (udpData[11] << 8);  //read was zero offset Hi

                        steerSettings.AckermanFix = (float)udpData[12] * 0.01;

                        //crc
                        //udpData[13];

                        //store in EEPROM
                        EEPROM.put(10, steerSettings);

                        // for PWM High to Low interpolator
                        highLowPerDeg = ((float)(steerSettings.highPWM - steerSettings.lowPWM)) / LOW_HIGH_DEGREES;
                    }

                    else if (udpData[3] == 251)  //251 FB - SteerConfig
                    {
                        uint8_t sett = udpData[5]; //setting0

                        if (bitRead(sett, 0)) steerConfig.InvertWAS = 1; else steerConfig.InvertWAS = 0;
                        if (bitRead(sett, 1)) steerConfig.IsRelayActiveHigh = 1; else steerConfig.IsRelayActiveHigh = 0;
                        if (bitRead(sett, 2)) steerConfig.MotorDriveDirection = 1; else steerConfig.MotorDriveDirection = 0;
                        if (bitRead(sett, 3)) steerConfig.SingleInputWAS = 1; else steerConfig.SingleInputWAS = 0;
                        if (bitRead(sett, 4)) steerConfig.CytronDriver = 1; else steerConfig.CytronDriver = 0;
                        if (bitRead(sett, 5)) steerConfig.SteerSwitch = 1; else steerConfig.SteerSwitch = 0;
                        if (bitRead(sett, 6)) steerConfig.SteerButton = 1; else steerConfig.SteerButton = 0;
                        if (bitRead(sett, 7)) steerConfig.ShaftEncoder = 1; else steerConfig.ShaftEncoder = 0;

                        steerConfig.PulseCountMax = udpData[6];

                        //was speed
                        //udpData[7]; 

                        sett = udpData[8]; //setting1 - Danfoss valve etc

                        if (bitRead(sett, 0)) steerConfig.IsDanfoss = 1; else steerConfig.IsDanfoss = 0;
                        if (bitRead(sett, 1)) steerConfig.PressureSensor = 1; else steerConfig.PressureSensor = 0;
                        if (bitRead(sett, 2)) steerConfig.CurrentSensor = 1; else steerConfig.CurrentSensor = 0;

                        //crc
                        //udpData[13];        

                        EEPROM.put(40, steerConfig);

                        //reset the arduino
                        resetFunc();
                    }

                } //end if 80 81 7F 
            }
        }

    } //end udp callback

//callback when received packets
    void CheckHello(void)
    {
        int packetSize = Hello.parsePacket();

        if (packetSize > 4)
        {
            //read into buffer
            Hello.readBytes(udpData, packetSize);

            uint16_t rem_port = Hello.remotePort();

            if (rem_port == 9999)
            {
                //Coming from agio
                if (udpData[0] == 128 && udpData[1] == 129 && udpData[2] == 127) //Data
                {
                    if (udpData[3] == 200) // Hello from AgIO
                    {
                        helloSteerPosition = counter;
                        counter = 0;

                        int16_t sa = (int16_t)(steerAngleActual * 100);

                        helloFromAutoSteer[5] = (uint8_t)sa;
                        helloFromAutoSteer[6] = sa >> 8;

                        helloFromAutoSteer[7] = (uint8_t)helloSteerPosition;
                        helloFromAutoSteer[8] = helloSteerPosition >> 8;
                        helloFromAutoSteer[9] = switchByte;

                        Hello.beginPacket(ipDestination, portDestination);
                        Hello.write(helloFromAutoSteer, sizeof(helloFromAutoSteer));
                        Hello.endPacket();
                    }

                    //set subnet
                    else if (udpData[3] == 201)
                    {
                        //make really sure this is the subnet pgn
                        if (udpData[4] == 5 && udpData[5] == 201 && udpData[6] == 201)
                        {
                            networkAddress.ipOne = udpData[7];
                            networkAddress.ipTwo = udpData[8];
                            networkAddress.ipThree = udpData[9];

                            //save in EEPROM and restart
                            EEPROM.put(60, networkAddress);
                            resetFunc();
                        }
                    }//end 201

                    //scan reply
                    else if (udpData[3] == 202)
                    {
                        //make really sure this is the reply pgn
                        if (udpData[4] == 3 && udpData[5] == 202 && udpData[6] == 202)
                        {
                            IPAddress rem_ip = Udp.remoteIP();

                            //scan pgn return
                            uint8_t scanReply[] = { 128, 129, 126, 203, 7,
                                networkAddress.ipOne, networkAddress.ipTwo, networkAddress.ipThree, 126,
                                 rem_ip[0],rem_ip[1],rem_ip[2] , 23 };

                            //checksum
                            int16_t CK_A = 0;
                            for (uint8_t i = 2; i < sizeof(scanReply) - 1; i++)
                            {
                                CK_A = (CK_A + scanReply[i]);
                            }
                            scanReply[sizeof(scanReply) - 1] = CK_A;

                            static uint8_t ipDest[] = { 255,255,255,255 };
                            uint16_t portDest = 9999; //AOG port that listens

                            //off to AOG
                            Hello.beginPacket(ipDest, portDest);
                            Hello.write(scanReply, sizeof(scanReply));
                            Hello.endPacket();
                        }
                    }

                } //end if 80 81 7F 
            }
        }

    } //end udp callback


//ISR Steering Wheel Encoder

  void EncoderFunc()
  {
      if (encEnable)
      {
          pulseCount++;
          encEnable = false;
      }
  }
