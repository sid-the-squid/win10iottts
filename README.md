<h1>Offline TTS & basic mp3 playback via a RPi 3b, using simple HTTP commands.</h1>

I'll start by saying as you're all no doubt aware all of this is doable with Linux, with far more documentation and code examples.
So you might ask why go through the pain of using windows 10 IoT?

Well simply the TTS engine in windows 10 IoT is the same as windows desktop, and far superior to any offline TTS engine I've found on Linux so far.

The initial idea was just for TTS, however I had an idea for another project, which also required the functionality to play a mp3 file from a local web server.

<b>So what do I use this for?</b>

To give a totally offline TTS voice to Homeassistant, and to give random mp3 playback from scanned NFC tags via Homeassistant.
I thought it would be fun for my daughter to be able to scan a tag on one of her toy animals and have some information play regarding the toy.

<h4>Hardware</h4>

<li>Raspberry pi 3b (rpi 3b+ & 4 are not supported), I assume it would also work with a pi2 and pi3a.
<li>usb audio device, I used this one (https://www.amazon.co.uk/gp/product/B00IRVQ0F8)
<li>external speakers
<li>micro sd card, I think 8gb or 16gb min.
<li>A windows PC with Visual studio 2015 or greater (works with community edition)

if you're happy with the default en-US language & voice packs, a standard windows 10 IoT image will be fine, if not see section below for installing other languages.

one thing to note, the very odd 'wifi connection' in the 'setup new device' never worked for me, even after I connected to a 2.4ghz wifi and put all the details in, so bear in mind when you first setup your pi, you'll need a keyboard & mouse and screen connected just to connect to wifi, obvs not an issue if you use ethernet.

<h4>Credits</h4>

This project is based on Andrej Tozon's work from 2017
http://tozon.info/blog/post/2017/07/28/Text-To-Speech-with-Windows-10-Iot-Core-UWP-on-Raspberry-Pi

it also contains a copy of the UniversalMediaEngine for windows 10 IoT https://github.com/ms-iot/UniversalMediaEngine

All dependencies are updated to run on visual studio 2022, now sadly using vs2022 you can't remote debug the pi :(, yep its a PITA but apparently vs2017 was the last to be able to do that, and as I don't own a vs license and have to use the community edition we find ourselves here.

Remember to build for 'release' 'arm', you should be able to 'deploy' to your pi straight from visual studio.


<h4>Wait I'd like a language other than en-US.</h4>

One foreword about the windows 10 IoT languages packs, I found it quite challenging to get any voice installed other than the default en-US, being British I'd rather have one of the en-GB voices, if you too would like a language or voice other than en-US, here's how to 'hack' them into a IoT image.

First you'll need to follow this guide to setup the image customisation software
https://docs.microsoft.com/en-us/windows-hardware/manufacture/iot/create-a-basic-image?view=windows-11

To get en-gb language first you must modify your image like this
https://docs.microsoft.com/en-us/windows/iot-core/develop-your-app/multilang#samples

<b>top tip</b> the 'OEM Input xml file' you want to modify C:\iotcoreworkspace\Source-arm\Products\ProductA\OEMInput.xml (assuming c:\iotcoreworkspace is your defined workspace)
it took me far too long to figure this one out.

Then you must do these steps here
https://stackoverflow.com/questions/55225478/windows-10-iot-core-language-installation-for-speechrecognizer

Then and only then can you have a speech voice other than the en-us voices.

<h4>ok so we're all deployed, what are some of the things it can do?</h4>

<b>to turn clock on or off</b>
<li>http://192.168.1.198:8085/config?action=clock&value=off
<li>http://192.168.1.198:8085/config?action=clock&value=on

<b>list all available voices</b>
<li>http://192.168.1.198:8085/config?action=voices&value=""

<b>set voice (examples)</b>
<li>http://192.168.1.198:8085/config?action=voice&value=Microsoft Hazel
<li>http://192.168.1.198:8085/config?action=voice&value=Microsoft George

<b>to speak</b>
<li>http://192.168.1.198:8085/say?text=hello world

<b>to play mp3s hosted on a web server</b>
<li>http://192.168.1.198:8085/play?action=play&path=http://homeassistant:8123/local/nyc_hub_rap.mp3
<li>http://192.168.1.198:8085/play?action=play&path=http://homeassistant:8123/local/pfm.mp3

<b>to stop playback</b>
<li>http://192.168.1.198:8085/play?action=stop&path=""
