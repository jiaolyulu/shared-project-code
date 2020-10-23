//需要把led连在d2口
const int ledPin=2;
const int buttonPin=3;
void setup() {
  // put your setup code here, to run once:
  pinMode(ledPin,OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (millis()%4500<500){
    digitalWrite(ledPin,HIGH);
  }
  if (millis()%4500<1000 && millis()%4500>=500){
    digitalWrite(ledPin,LOW);
  }
  if (millis()%4500<2000 && millis()%4500>=1000){
    digitalWrite(ledPin,HIGH);
  }
  if (millis()%4500<2500 && millis()%4500>=2000){
    digitalWrite(ledPin,LOW);
  }
  if (millis()%4500<4000 && millis()%4500>=2500){
    digitalWrite(ledPin,HIGH);
  }
  if (millis()%4500>=4000){
    digitalWrite(ledPin,LOW);
  }

}
