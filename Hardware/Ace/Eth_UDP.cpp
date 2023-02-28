// 
// 
// 

#include "Eth_UDP.h"

//constructor
Eth_UDP::Eth_UDP(void) 
{
    navAddress[0] = 192;
    navAddress[1] = 168;
    navAddress[2] = 5; // .5 The Default Subnet of AOG
    navAddress[3] = thisIP;

    EEPROM.get(60, EEP_NetRead);              // read identifier

    if (EEP_NetRead != EEP_Net)            // check on first start and write EEPROM
    {
        EEPROM.put(60, EEP_Net);
        EEPROM.put(62, navAddress[0]);
        EEPROM.put(63, navAddress[1]);
        EEPROM.put(64, navAddress[2]);
    }
    else
    {
        EEPROM.get(62, navAddress[0]);
        EEPROM.get(63, navAddress[1]);
        EEPROM.get(64, navAddress[2]);
    }
}

//destructor
Eth_UDP::~Eth_UDP(void) {}

//Initalize ethernet connections
void Eth_UDP::Start()
{
    // start the Ethernet connection:
    Serial.println("Initializing ethernet with static IP address");

    // Start Ethernet with IP from settings
    Ethernet.begin(mac, navAddress); 

    Serial.print("\r\nEthernet IP of module: "); Serial.println(Ethernet.localIP());

    //set this module net IP to broadcast
    navAddress[3] = 255;

    //build the Steer module Address
    steerAddress = navAddress;
    steerAddress[3] = 126;

    Serial.print("\r\nEthernet Broadcast IP: "); Serial.println(navAddress);

    Serial.print("\r\nAgIO listening to port: "); Serial.println(portAgIO_9999);

    // Check for Ethernet hardware present
    if (Ethernet.hardwareStatus() == EthernetNoHardware)
    {
        Serial.println("\r\nEthernet shield was not found. GPS via USB only.");
        isRunning = false;
        return;
    }

    if (Ethernet.linkStatus() == LinkOFF)
    {
        Serial.println("\r\nEthernet cable is not connected.");
        return;
    }

    Serial.println("\r\nEthernet status OK\r\n");

    // init UPD GPS (5120) Port
    if (NMEA.begin(portNMEA_2211))
    {
        isRunning = true;
        Serial.print("Ethernet GPS UDP listening on port: ");
        Serial.println(portNMEA_2211);
    }

    // init UPD Port getting AutoSteer (8888) data from AGIO
    if (PGN.begin(portSteer_8888))
    {
        Serial.print("Ethernet PGN UDP listening to port: ");
        Serial.println(portSteer_8888);
    }

    // init UPD Port getting Hello and scan (7777) from AgIO
    if (Hello.begin(portHello_7777))
    {
        Serial.print("Ethernet Hello Scan listening to port: ");
        Serial.println(portHello_7777);
    }
}

void Eth_UDP::SendUdpByte(uint8_t* _data, uint8_t _length, IPAddress _ip, uint16_t _port)
{
    PGN.beginPacket(_ip, _port);
    PGN.write(_data, _length);
    PGN.endPacket();
}

void Eth_UDP::SendUdpChar(char* _charBuf, uint8_t _length, IPAddress _ip, uint16_t _port)
{
    PGN.beginPacket(_ip, _port);
    PGN.write(_charBuf, _length);
    PGN.endPacket();
}

void Eth_UDP::SaveModuleIP(void)
{
    //ID stored in 60
    EEPROM.put(62, navAddress[0]);
    EEPROM.put(63, navAddress[1]);
    EEPROM.put(64, navAddress[2]);
}

//EtherClass Ether;

