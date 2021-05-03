var exec = require('child_process').execSync;
var util = require('util');
var mic = require('mic');
var fs = require('fs');

var isRecording = false;

var micInstance = mic({
    rate: '44000',
    channels: '1',
    debug: true,
    device: "hw:2,0"
});
var micInputStream = micInstance.getAudioStream();
 
outputFileStream = fs.WriteStream('output.raw');
 
// micInputStream.pipe(outputFileStream);
 
micInputStream.on('data', function(data) {
    console.log("Recieved Input Stream: " + data.length);
});
 
micInputStream.on('error', function(err) {
    cosole.log("Error in Input Stream: " + err);
});
 
micInputStream.on('startComplete', function() {
    console.log("Got SIGNAL startComplete");
    setTimeout(function() {
            micInstance.pause();
            
    }, 3000);
});
    
micInputStream.on('stopComplete', function() {
    console.log("Got SIGNAL stopComplete");
    outputFileStream.close();
    console.log("--------------- LAST MSG!!");
    setTimeout(function() {
        console.log("--------------- Final MSG!!");
        startMic(4);
    }, 4000);
});
    
micInputStream.on('pauseComplete', function() {
    console.log("Got SIGNAL pauseComplete");
    setTimeout(function() {
        micInstance.resume();
    }, 3000);
});
 
micInputStream.on('resumeComplete', function() {
    console.log("Got SIGNAL resumeComplete");
    setTimeout(function() {
        micInstance.stop();
        isRecording = false;
        console.log("Recording: " + isRecording);
    }, 5000);
});
 
micInputStream.on('silence', function() {
    console.log("Got SIGNAL silence");
});
 
micInputStream.on('processExitComplete', function() {
    console.log("Got SIGNAL processExitComplete");
    
});
 
function startMic(x){
    if (isRecording == false){
        isRecording = true;
        console.log("start recording" + x);
        // outputFileStream = fs.WriteStream('output' + x + '.raw');
        micInputStream.pipe(outputFileStream);

        micInstance.start();
    }
}


startMic(3);

