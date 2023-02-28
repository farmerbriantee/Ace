// Buffer For Receiving 8888 UDP Data
uint8_t helloUdpData[UDP_TX_PACKET_MAX_SIZE];

//Heart beat hello AgIO
uint8_t helloFromNav[] = { 128, 129, 121, 121, 5, 0, 0, 0, 0, 0, 71 };

// UDP Receive sent from AgIO - sent to port 8888
void ReceiveUDP_Hello()
{
    // When ethernet is not running, return directly. parsePacket() will block when we don't
    if (udp.isRunning)
    {
        //get data from AgIO sent by 9999 to this 8888
        uint16_t len = udp.Hello.parsePacket();

        //make sure from AgIO
        if (udp.Hello.remotePort() == 9999)
        {
            // Check for len > 4, because we check byte 0, 1, 2 and 3
            if (len > 4)
            {
                udp.Hello.read(helloUdpData, UDP_TX_PACKET_MAX_SIZE);

                //Hello Sent from AgIO - reply imu
                if (helloUdpData[0] == 128 && helloUdpData[1] == 129 && helloUdpData[2] == 127)
                {
                    //hello
                    if (helloUdpData[3] == 200) // Hello from AgIO                                    
                    {
                        udp.SendUdpByte(helloFromNav, sizeof(helloFromNav), udp.navAddress, udp.portAgIO_9999);
                    }

                    //Scan Modules
                    else if (helloUdpData[3] == 202)
                    {
                        //make really sure this is the reply pgn
                        if (helloUdpData[4] == 3 && helloUdpData[5] == 202 && helloUdpData[6] == 202)
                        {
                            IPAddress rem_ip = udp.Hello.remoteIP();

                            //scan reply back to AgIO
                            uint8_t scanReply[] = { 128, 129, udp.thisIP, 203, 4,
                                udp.navAddress[0], udp.navAddress[1], udp.navAddress[2], udp.thisIP,
                                rem_ip[0],rem_ip[1],rem_ip[2], 23 };

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
                            udp.SendUdpByte(scanReply, sizeof(scanReply), ipDest, portDest);
                        }
                    }// end 202

                    else if (helloUdpData[3] == 201)
                    {
                        //make really sure this is the subnet pgn
                        if (helloUdpData[4] == 5 && helloUdpData[5] == 201 && helloUdpData[6] == 201)
                        {
                            udp.navAddress[0] = helloUdpData[7];
                            udp.navAddress[1] = helloUdpData[8];
                            udp.navAddress[2] = helloUdpData[9];

                            //save in EEPROM and restart
                            udp.SaveModuleIP();

                            SCB_AIRCR = 0x05FA0004; //Teensy Reset
                        }
                    }//end 201
                } //end if 80 81 7F
            }
        }
    }
}


