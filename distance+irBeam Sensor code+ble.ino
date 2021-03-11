const int IRBeamPin=2;
#include <Wire.h>
#include "Adafruit_VL6180X.h"
Adafruit_VL6180X vl = Adafruit_VL6180X();

#include <ArduinoBLE.h>



BLEService ledService("19B10010-E8F2-537E-4F6C-D104768A1214"); // create service

// create switch characteristic and allow remote device to read and write
//BLEByteCharacteristic ledCharacteristic("19B10011-E8F2-537E-4F6C-D104768A1214", BLERead | BLEWrite);
// create button characteristic and allow remote device to get notifications
BLEIntCharacteristic distanceCharacteristic("19B10012-E8F2-537E-4F6C-D104768A1214", BLERead | BLENotify);
BLEIntCharacteristic beamCharacteristic("6b2128e1-50ac-47a6-a5f8-9a935e780191", BLERead | BLENotify);
void setup() {
  Serial.begin(115200);
  // wait for serial port to open on native usb devices
  while (!Serial) {
    delay(1);
  }
  Serial.println("Adafruit VL6180x test!");
  if (! vl.begin()) {
    Serial.println("Failed to find distance sensor");
    while (1);
  }
  Serial.println("Distance Sensor found!");


  // begin initialization
  if (!BLE.begin()) {
    Serial.println("starting BLE failed!");

    while (1);
  }

  // set the local name peripheral advertises
  BLE.setLocalName("ButtonLED");
  // set the UUID for the service this peripheral advertises:
  BLE.setAdvertisedService(ledService);

  // add the characteristics to the service
  //ledService.addCharacteristic(ledCharacteristic);
  ledService.addCharacteristic(distanceCharacteristic);
  ledService.addCharacteristic(beamCharacteristic);
  // add the service
  BLE.addService(ledService);

  //ledCharacteristic.writeValue(0);
  distanceCharacteristic.writeValue(0);
  beamCharacteristic.writeValue(0);
  // start advertising
  BLE.advertise();

  Serial.println("Bluetooth device active, waiting for connections...");
}

void loop() {
  BLE.poll();
  printIRBeamSensor();
  printDistanceSensor();
  delay(100);
  // poll for BLE events
  


}
void printIRBeamSensor(){
  Serial.print("IR beam:");
  int IRBeamReading=digitalRead(IRBeamPin);
  Serial.println(IRBeamReading);
  beamCharacteristic.writeValue(IRBeamReading);
}
void printDistanceSensor(){ 
  uint8_t range = vl.readRange();
  uint8_t status = vl.readRangeStatus();

  if (status == VL6180X_ERROR_NONE) {
    Serial.print("Range: "); Serial.println(range);
    distanceCharacteristic.writeValue(range);
  }
  else{
    Serial.print("Range: "); Serial.println("bigger than 200mm");
    distanceCharacteristic.writeValue(200);
  }
}
