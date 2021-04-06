int DTMFdata=0;
void setup() {
  // put your setup code here, to run once:
  pinMode(3,INPUT);
  pinMode(4,INPUT);
  pinMode(5,INPUT);
  pinMode(6,INPUT);
  pinMode(7,INPUT);
  Serial.begin(9600);
}

void loop() {
  DTMFdata=0;
  // put your main code here, to run repeatedly:
  if (digitalRead(7)==HIGH){
    if (digitalRead(6)==HIGH){                                                                                                                                                                                                                                                                                                                              
    }
    if (digitalRead(5)==HIGH){
      DTMFdata+=4;
    }
    if (digitalRead(4)==HIGH){
      DTMFdata+=2;
    }
    if (digitalRead(3)==HIGH){
      DTMFdata+=1;
    }
    if (DTMFdata==10){
      DTMFdata=0;
    }
    Serial.println(DTMFdata);
  }
  
}
