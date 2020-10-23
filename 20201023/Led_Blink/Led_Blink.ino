const int ledPin=2;
const int buttonPin=3;
void setup() {
  // put your setup code here, to run once:
  pinMode(ledPin,OUTPUT);
  pinMode(buttonPin,INPUT_PULLUP); // 设置buttonPin为上拉电阻（高阻断）
}

void loop() {
  // put your main code here, to run repeatedly:
  

  if (digitalRead(buttonPin)==LOW){ //如果button引脚的读数为低电压，则意味着按钮被按下了。
    digitalWrite(ledPin,HIGH);
    delay(500);
    digitalWrite(ledPin,LOW);
    delay(500);
    digitalWrite(ledPin,HIGH);
    delay(1000);
    digitalWrite(ledPin,LOW);
    delay(500);
    digitalWrite(ledPin,HIGH);
    delay(1500);
    digitalWrite(ledPin,LOW);
    delay(500);
  }
}
