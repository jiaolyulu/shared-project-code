var gpio = require('rpi-gpio');
var dictionary;

var startRecordingTime;
var endRecordingTime;
const fs = require('fs')
fs.readFile('./tone.json', 'utf8', (err, jsonString) => {
    if (err) {
        console.log("File read failed:", err)
        return 
    }
    dictionary = JSON.parse(jsonString)
    console.log('File data:', dictionary.rule.length) 
})

var exec = require('child_process').execSync;
var util = require('util');

gpio.on('change', function(channel, value) {
  if (channel==16){
    if (value==true){
      console.log('Channel ' + channel + ' value is now ' + value);
      recordMic("output");
    }
    
  }
  
});
gpio.setup(16, gpio.DIR_IN, gpio.EDGE_BOTH);


function recordMic(filename){
  command = util.format('node mic.js ' + filename + '&& echo Done! && ffmpeg -y -i output.raw -f flac output.flac && echo done!!!!');
  output = exec(command, function(error, stdout, stderr){console.log('stdout: ' + stdout); });
  sendData();
  return output.toString().trim();
}


 


//Example STT - "watson"
const SerialPort = require('serialport');			// include the serialport library
const Readline = require('@serialport/parser-readline');
var	portName =  "/dev/ttyUSB0";						// get the port name from the command line
const myPort = new SerialPort(portName);		// open the port
const parser = new Readline();				    // make a new parser to read ASCII lines
myPort.pipe(parser);							// pipe the serial stream to the parser

myPort.on('open', openPort);		// called when the serial port opens
myPort.on('close', closePort);		// called when the serial port closes
myPort.on('error', serialError);	// called when there's an error with the serial port
parser.on('data', listenPort);			// called when there's new data incoming



function listenPort(data) {
	console.log("I am listening!!!");
	console.log(data);
}

function closePort() {
	console.log('port closed');
}

function serialError(error) {
	console.log('there was an error with the serial port: ' + error);
	myPort.close();
}



var openBoolean=false;
var pinyin = require("pinyin");



function openPort() {
	var brightness = 0;				// the brightness to send for the LED
	console.log('port open');
  
  openBoolean=true;



}
function sendData() {
  var toneString=listen("output.flac");
  // convert the value to an ASCII string before sending it:
  if (toneString){
    myPort.write(toneString.toString());
  }
  
  var phrasenumber=Math.floor((toneString.length-1)/4);
  console.log("phraseNumber",phrasenumber);
  var substitude="";
  console.log("Sending " +toneString+ " out the serial port");
  if (phrasenumber>0){
    for (let i=0;i<=phrasenumber;i++){
      for (let index=0;index<dictionary.rule.length;index++){
        var subtone=toneString.substring(i*4,i*4+4);
        if (dictionary.rule[index].tone==subtone){
          substitude+=dictionary.rule[index].word[Math.floor(Math.random()*dictionary.rule[index].word.length)];
          console.log("subtone",subtone,dictionary.rule[index].word,dictionary.rule[index].tone)
          break;
        }
      }
    }
  }
  for (let index=0;index<dictionary.rule.length;index++){
    var subtone=toneString.substring(phrasenumber*4,toneString.length-1);
    if (dictionary.rule[index].tone==subtone){
      substitude+=dictionary.rule[index].word[Math.floor(Math.random()*dictionary.rule[index].word.length)];
      break;
    }
  }
  console.log("substitude",substitude);
  
  
  
}
function listen(something){
  var APIkey = "";
  var url = "";
  var data = "@" + something;
  command = util.format('curl -X POST -u \"apikey:%s\" -H \"Content-Type: audio/flac\" --data-binary %s \"%s/v1/recognize?model=zh-CN_BroadbandModel&timestamps=true&max_alternatives=3\" | grep -m1 \"transcript\" ', APIkey,data,url);
  output = exec(command, function(error, stdout, stderr){
    // if (err) {
    //   console.error(err);
    //   return;
    // }
      
  });
  var stringOutput= output.toString().trim();
  console.log(stringOutput);
  var number=stringOutput.search("transcript");
  var transcriptString=stringOutput.substring(15,stringOutput.length-2);
  transcriptString=transcriptString.replace(/\s/g,'');
  let toneString=pinyin(transcriptString, {
    segment: true,                // 启用分词
    group: true,                   // 启用词组
    style: pinyin.STYLE_TONE2
  }).toString().replace(/\D/g,'');
  console.log(toneString);
  toneString+="&";
  return "3321&";
}


