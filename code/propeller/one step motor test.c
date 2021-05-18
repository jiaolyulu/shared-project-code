/*
  Blank Simple Project.c
  http://learn.parallax.com/propeller-c-tutorials 
*/
#include "simpletools.h"                      // Include simple tools
#include "fdserial.h"

fdserial *port;
char data[12] = "nodataatalll";
int index=0;


void rotate(void *par[]);

volatile int cog1,cog2,cog3,cog4;
int main()                                    // Main function
{
  // Add startup code here.
  pause(100);
  simpleterm_close();
  set_pause_dt(CLKFREQ/50000);
  int pinArray1[]={2,6,1,14,10};
  int pinArray2[]={2,5,2,15,11};
  int pinArray3[]={2,4,3,26,12};
  int pinArray4[]={2,3,4,27,13};
  
  for (int indx=1;indx<=4;indx++){
    cogstop(indx);
  }
     
  port = fdserial_open(31, 30, 0, 9600);
  char c;
  
  while(1)
  {
    c = fdserial_rxChar(port);
    if(c != -1)
    {
      if (c!='&'){
        data[index]=c;
        index++;
      }        
      if (c=='&'){
        
        for (int i=0;i<index;i++){
          fdserial_txChar(port,data[i]-'0');    
        }
                 
        fdserial_txChar(port,'\n');
        fdserial_txFlush(port); 
        unsigned int stack1[40+25];
        unsigned int stack2[40+25];
        unsigned int stack3[40+25];
        unsigned int stack4[40+25]; 
        for (int i=0;i<index;i++){
          if (data[i]=='1'){
            cog1=cogstart(&rotate,(void*)pinArray1,stack1,sizeof(stack1));
            pause(360000); 
          }
          if (data[i]=='2'){
            cog2=cogstart(&rotate,(void*)pinArray2,stack2,sizeof(stack2));
            pause(360000); 
          }   
          if (data[i]=='3'){
            cog3=cogstart(&rotate,(void*)pinArray3,stack3,sizeof(stack3));
            pause(360000); 
          }   
          if (data[i]=='4'){
            cog4=cogstart(&rotate,(void*)pinArray4,stack4,sizeof(stack4));
            pause(360000); 
          }
                               
        
        }  
        fdserial_rxFlush(port);
        index=0; 
        pause(200000);
        
      }
     
    }  
    
  }  
}
void rotate(void *pinArray[]){
  int dirPin=(int)pinArray[0];
  int stepPin=(int)pinArray[1];
  int motorNum=(int)pinArray[2];
  int ledPin=(int)pinArray[3];
  int hallPin=(int)pinArray[4];
  set_direction(hallPin,0);
  set_direction(ledPin,1);
  
  high(dirPin);
  
  high(ledPin);
  int position=input(hallPin);
  int i=0;
  while (i<6400){
    i++; 
    if (i<=1600){  //(-abs(i-800)+1200)/80*3
      int timeInterval=50;
      if (motorNum==1){
        timeInterval=100;
      } 
      if (motorNum==4){
        timeInterval=(2400+i)/36;
      }        
      if (motorNum==3){
        timeInterval=(-abs(i-800)+4000)/36;
      } 
      if (motorNum==2){
        timeInterval=(4400-i)/36;
      }
      high(stepPin);
      pause(timeInterval);
      low(stepPin);
      pause(timeInterval);
      
    } 
    else{
      position=input(hallPin);
      if (position!=1){
        break;
      }        
    }      
    if (1600<i&&i<=1700){
      pause(50);
    }  
    if (i>1700&&i<6400){
      high(stepPin);
      pause(20);
      low(stepPin);
      pause(20);
    }      
  }       
  low(ledPin);
  if (motorNum==1){cogstop(cog1);}
  if (motorNum==2){cogstop(cog2);}
  if (motorNum==3){cogstop(cog3);}
  if (motorNum==4){cogstop(cog4);}
}  
