/*
*UDP Receive sent from GPS Source - listen on port 2211 Single GPS
*
* Character data
*/


// buffer for receiving GGA and VTG
char NMEA_packetBuffer[256];       

void udpNMEA()
{
    // When ethernet is not running, return directly. parsePacket() will block when we don't
    if (udp.isRunning)
    {
        int packetLength = udp.NMEA.parsePacket();

        if (packetLength > 0)
        {
            udp.NMEA.read(NMEA_packetBuffer, packetLength);
            for (int i = 0; i < packetLength; i++)
            {
                parser << NMEA_packetBuffer[i];
            }
        }
    }
    else
    {
        return;
    }
}
