// d3---Q1
// d4---Q2
// d5---Q3
// d6---Q4
// d7---Q5
#define relayPin1 8
#define relayPin2 9
const int rows=10;
const int columns=8;
const int BeatInterval=250;

// a three dimensional array storing the beats information
const int Mode[rows][2][columns]= {{{1,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0}},
                                   {{1,0,1,0,1,0,1,0},{0,1,0,1,0,1,0,1}},
                                   {{1,0,0,1,0,0,0,0},{0,0,0,0,0,0,0,0}},
                                   {{0,0,0,1,0,0,0,0},{1,0,0,0,0,0,0,0}},
                                   {{0,0,0,0,0,0,0,0},{1,0,0,0,0,0,0,0}},
                                   {{0,0,0,0,0,0,0,0},{1,0,1,0,0,0,1,0}},
                                   {{0,0,0,0,1,0,0,0},{1,0,0,0,0,0,0,0}},
                                   {{1,0,0,0,0,0,0,0},{0,0,0,0,1,0,0,0}},
                                   {{0,0,0,0,0,0,0,0},{1,0,0,0,0,0,0,0}},
                                   {{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0}},};
void setup() {
  // put your setup code here, to run once:
  pinMode(relayPin1,OUTPUT);
  pinMode(relayPin2,OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  for (int i=0;i<rows;i++){
    for (int j=0;j<columns;j++){
      if (Mode[i][0][j]||Mode[i][1][j]){  // if either relay1 and relay 2 has to be triggered
        if (Mode[i][0][j]){
          digitalWrite(relayPin1,HIGH);
        }
        if (Mode[i][1][j]){
          digitalWrite(relayPin2,HIGH);
        }
        delay(50);
        digitalWrite(relayPin1,LOW);
        digitalWrite(relayPin2,LOW);
        delay(BeatInterval-50);
      }
      else{                               // if none of the relay need to be triggered
        delay(BeatInterval);
      }
      
    }
  }
}
