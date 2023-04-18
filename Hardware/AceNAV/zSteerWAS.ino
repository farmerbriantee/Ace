//Send WAS position to Steer module at imu rate 100 hz

#define WAS_SENSOR_PIN A0       //ADC0
#define ANALOG_SENSOR_PIN A1    //ADC1

//WAS pgn
uint8_t wasPGN_249[] = { 128, 129, 121, 249, 8, 0, 0, 0, 0, 0, 0, 0, 0, 71 };
int8_t wasPGN_249_Size = sizeof(wasPGN_249) - 1;

int16_t helloSteerPosition = 0;

//steering ADC Value
int16_t steeringPosition = 0; //from steering sensor

void SampleWAS()
{
	// R2 = (VOUT * R1) / (VIN - VOUT)
	steeringPosition = adcWAS->adc0->analogRead(WAS_SENSOR_PIN);

	wasPGN_249[5] = (uint8_t)steeringPosition;
	wasPGN_249[6] = (uint8_t)(steeringPosition >> 8);

	//checksum
	int16_t CK_A = 0;
	for (uint8_t i = 2; i < wasPGN_249_Size; i++)
		CK_A = (CK_A + wasPGN_249[i]);

	wasPGN_249[wasPGN_249_Size] = CK_A;

	//send to autosteer module
	udp.SendUdpByte(wasPGN_249, sizeof(wasPGN_249), udp.steerAddress, udp.portSteer_8888);
}

void ADC_Setup()
{
	//WAS A/D convertor setup
	pinMode(WAS_SENSOR_PIN, INPUT_DISABLE); //AO
	pinMode(ANALOG_SENSOR_PIN, INPUT_DISABLE); //A1

	adcWAS->adc0->setAveraging(16); // set number of averages
	adcWAS->adc0->setResolution(12); // set bits of resolution
	adcWAS->adc0->setConversionSpeed(ADC_CONVERSION_SPEED::MED_SPEED); // change the conversion speed
	adcWAS->adc0->setSamplingSpeed(ADC_SAMPLING_SPEED::MED_SPEED); // change the sampling speed

	adcWAS->adc1->setAveraging(16); // set number of averages
	adcWAS->adc1->setResolution(12); // set bits of resolution
	adcWAS->adc1->setConversionSpeed(ADC_CONVERSION_SPEED::LOW_SPEED); // change the conversion speed
	adcWAS->adc1->setSamplingSpeed(ADC_SAMPLING_SPEED::LOW_SPEED); // change the sampling speed
}