/*
*UDP Receive sent from AgIO - sent to port 8888
* 
* We really don't want to get all the module pgns here at all !
* So it is never called.
* 
*/ 

// Buffer For Receiving 8888 UDP Data
uint8_t agioUdpData[UDP_TX_PACKET_MAX_SIZE];


void ReceiveUDP_PGN()
{
    // When ethernet is not running, return directly. parsePacket() will block when we don't
    if (udp.isRunning)    
    {
        //get data from AgIO sent by 9999 to this 8888
        uint16_t len = udp.PGN.parsePacket();
        
        //make sure from AgIO
        if (udp.PGN.remotePort() == 9999)
        {
            // Check for len > 4, because we check byte 0, 1, 2 and 3
            if (len > 4)
            {
                udp.PGN.read(agioUdpData, UDP_TX_PACKET_MAX_SIZE);

                Serial.println("You shouldn't be here");
            }
        }
    }
}

