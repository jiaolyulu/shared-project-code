// Recording Mic Script

var exec = require('child_process').execSync;
var mic = require('mic');
var fs = require('fs');

var micInstance = mic({
    rate: '16000',
    channels: '1',
    debug: true,
    exitOnSilence: 14,
    device: "hw:2,0"
});
var micInputStream = micInstance.getAudioStream();

var outputFileStream = fs.WriteStream(process.argv[2] + '.raw');

micInputStream.pipe(outputFileStream);

micInputStream.on('data', function(data) {
    console.log("Recieved Input Stream: " + data.length);
});

micInputStream.on('error', function(err) {
    cosole.log("Error in Input Stream: " + err);
});

micInputStream.on('startComplete', function() {
    console.log("Got SIGNAL startComplete");
    setTimeout(function() {
            micInstance.stop();
    }, 10000);
});

micInputStream.on('stopComplete', function() {
    console.log("Got SIGNAL stopComplete");
});


micInputStream.on('silence', function() {
    console.log("Got SIGNAL silence");
});

micInputStream.on('processExitComplete', function() {
    console.log("Got SIGNAL processExitComplete");
});

micInstance.start();
