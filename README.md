# AR Monitoring and Control Of Industrial IoT devices

This project work represents a partial completion of the EEE F411 Internet of Things course instructed by [Dr. Vinay Chamola](https://web.bits-pilani.ac.in/pilani/vinaychamola/profile), Associate Professor at BITS Pilani, Rajasthan. The primary emphasis of the project is on overseeing and managing industrial machinery through augmented reality (AR) headsets such as [Hololens](https://www.microsoft.com/en-us/hololens). It serves as an illustration of how IoT-enabled machines can be supervised and operated using an application compatible with Hololens.

---
## How it works

The project encompasses an interactive application crafted in the [Unity](https://unity.com/) Game Engine specifically designed for use with Hololens. The IoT circuit becomes operational, transmitting and receiving data, only when both power supply and WiFi connectivity are simultaneously available. Real-time data collected from the distance sensor and potentiometer is seamlessly sent to Google's [Firebase](https://firebase.google.com/) Realtime Database, where it is promptly updated in the Unity app in real-time. 
A standout feature in our project was the introduction of a slider mechanism, allowing users to control the servo motor's angle. This not only emphasized user-centric design but also used an engaging element in the control system. The value of the slider mechanism sets the angle of the servo motor via database. 
Additionally, the values recorded by potentiometer are periodically sent to the [ThingSpeak](https://thingspeak.com/) from which various graphs and MATLAB Visualisations can be obtained. Inside the app, a <em>Monitor</em> button is made which displays the data recorded in ThingSpeak in a form of a colourful graph.

---
## Hardware Used
- ESP32 DevKit V1
- NodeMCU 0.9 (ESP-12 Module)
- Ultrasonic Distance Sensor
- An LED of any colour
- Potentiometer
- Micro Servo Motor
---
## Software Versions
- Unity: 2022.3.12f1
- Arduino IDE: Any version (Here 1.8.19)
- Please ensure that you have libraries installed for [ESP32](https://randomnerdtutorials.com/getting-started-with-esp32/#esp32-arduino-ide) and [ESP8266](https://randomnerdtutorials.com/how-to-install-esp8266-board-arduino-ide/) boards.
- Visual Studio: 2022
- Firebase SDK: firebase_unity_sdk_11.6.0
- MRTK: [1.0.2209.0](https://www.microsoft.com/en-us/download/details.aspx?id=102778) 
---
## Source Code

| File | Link | Desciption |
| ---- | ---- | ---------- |
| Main Script | [DataBaseManager.cs](Assets/DataBaseManager.cs) | Manages the initial communication between Unity and Firebase |
| Distance Script | [Distance.cs](Assets/Distance.cs) | Receives and displays distance related data |
| Distance Script | [DCMotor.cs](Assets/DCMotor.cs) | Receives and displays potentiometer related data |
| Distance Script | [Servo.cs](Assets/Servo.cs) | Sends and displays servo related data |
| ESP32 Code | [CloudControllerV3.ino](MicroController/CloudControllerV3/CloudControllerV3.ino) | For Distance and Servo motor (Firebase) |
| NodeMCU Code | [NodeMCUCon.ino](MicroController/NodeMCUCon/NodeMCUCon.ino) | For Potentiometer related data to Firebase and Thingspeak|

---
## Prerequisites

1. Basic understanding of IoT devices and the Arduino.
2. Basic understanding of the Unity Game Engine, C++ & Object Oriented programming.

---
## DISCLAIMER

As this is an open ended project, this doesn't contain any 3D model of any IoT device right now, but can be extended in that direction. Due to limited time (20 days) alloted to this project, it can be further extended by implementation of graphs, usage of better micro-controllers and diverse sensors. Currently, Firebase SDK does not support Universal Windows Platform, due to which the apllication can't be deployed and tested on Hololens, but we are sure enough that as soon as the SDK supports UWP, this application should be able to work on Hololens. We do not claim this to be fully error-less, and potential errors could arise even while running the code.

---
## Have fun! Thank-You!
