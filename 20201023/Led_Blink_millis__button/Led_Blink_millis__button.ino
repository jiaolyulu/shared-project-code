//需要把一个按钮一端接d3与正极，一端接地
//需要把led连在d2口
const int ledPin=2;
const int buttonPin=3;
int buttonPinReading=HIGH;
int preButtonPinReading=HIGH;
unsigned long timeStart=0;
boolean button=false;

void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  pinMode(ledPin,OUTPUT);
  pinMode(buttonPin,INPUT_PULLUP);
}

void loop() {
  // put your main code here, to run repeatedly:
  preButtonPinReading=buttonPinReading;
  buttonPinReading=digitalRead(buttonPin);

  if (buttonPinReading==HIGH&&preButtonPinReading==LOW&&millis()-timeStart>500){  //为了让每次按下button的动作只被识别一次，我们需要做很多努力。因为button的数值很多时候会出现误触。当按下按钮，当我们输出按钮引脚的读数，可以看到是000100..01..100,在低电平中总是夹杂着一两个高电平输出。为了去除那些干扰数据，我们需要首先：1、识别电平由低到高的瞬间。（也就是我们释放button的瞬间）2、判断这次与上一次触发的间隔时间。如果间隔时间很短（在这里我设置间隔时间至少是500毫秒），则意味着此次可能是干扰数据。
    timeStart=millis();
    button=!button; //改变按钮的状态。由开--关或者由关--开模式。
    Serial.println(button);
  }

  if (button){//当开关出于开启模式的时候，开始播放led的动画。
    if ((millis()-timeStart)%4500<500){ //这里我们我们需要把
      digitalWrite(ledPin,HIGH);
    }
    if ((millis()-timeStart)%4500<1000 && (millis()-timeStart)%4500>=500){
      digitalWrite(ledPin,LOW);
    }
    if ((millis()-timeStart)%4500<2000 && (millis()-timeStart)%4500>=1000){
      digitalWrite(ledPin,HIGH);
    }
    if ((millis()-timeStart)%4500<2500 && (millis()-timeStart)%4500>=2000){
      digitalWrite(ledPin,LOW);
    }
    if ((millis()-timeStart)%4500<4000 && (millis()-timeStart)%4500>=2500){
      digitalWrite(ledPin,HIGH);
    }
    if ((millis()-timeStart)%4500>=4000){
      digitalWrite(ledPin,LOW);
    }
  }
  else{
    digitalWrite(ledPin,LOW);
  }
}
