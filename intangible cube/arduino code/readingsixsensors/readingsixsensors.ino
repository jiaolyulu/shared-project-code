#include <Wire.h>
#include "Adafruit_VL6180X.h"

#define TCAADDR 0x70

Adafruit_VL6180X vl = Adafruit_VL6180X();

//////////////////////////////////////////////////////////////////////////////////
void tcaselect(uint8_t i) {
  if (i > 7) return;
 
  Wire.beginTransmission(TCAADDR);
  Wire.write(1 << i);
  Wire.endTransmission(); 
}

//////////////////////////////////////////////////////////////////////////////////
void setup()
{
    Wire.begin();
   
    Serial.begin(115200);
    Serial.println("\nTCA9548A with two VL6180X sensors demo.");
    tcaselect(6); // now talking to VL6180X on 2
    vl.begin();
    tcaselect(7); // now talking to VL6180X on 2
    vl.begin();
    tcaselect(2); // now talking to VL6180X on 2
    vl.begin();
    tcaselect(3); // now talking to VL6180X on 2
    vl.begin();
    tcaselect(4); // now talking to VL6180X on 2
    vl.begin();   // init sensor
    tcaselect(5); // now talking to VL6180X on 4
    vl.begin();   // init sensor
}

//////////////////////////////////////////////////////////////////////////////////
void loop()
{
  tcaselect(6);
  Serial.print(6);Serial.print(","); Serial.print(vl.readRange());Serial.print(",");
  tcaselect(7);
  Serial.print(7);Serial.print(","); Serial.print(vl.readRange());Serial.print(",");
  tcaselect(2);
  Serial.print(2);Serial.print(",");Serial.print(vl.readRange());Serial.print(",");
  tcaselect(3);
  Serial.print(3);Serial.print(","); Serial.print(vl.readRange());Serial.print(",");
  tcaselect(4);
  Serial.print(4);Serial.print(","); Serial.print(vl.readRange());Serial.print(",");
  tcaselect(5);
  Serial.print(5);Serial.print(","); Serial.println(vl.readRange());
  
}
