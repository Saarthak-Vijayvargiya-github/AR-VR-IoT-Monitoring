# AR Monitoring and Control Of Industrial IoT devices

This project work represents a partial completion of the EEE F411 Internet of Things course instructed by [Dr. Vinay Chamola](https://www.bits-pilani.ac.in/pilani/vinay-chamola/), Associate Professor at BITS Pilani, Rajasthan. The primary emphasis of the project is on overseeing and managing industrial machinery through augmented reality (AR) headsets such as [Hololens](https://www.microsoft.com/en-us/hololens). It serves as an illustration of how IoT-enabled machines can be supervised and operated using an application compatible with Hololens.

---
## What's Inside

The project encompasses an interactive application crafted in the [Unity](https://unity.com/) Game Engine specifically designed for use with Hololens. The IoT circuit becomes operational, transmitting and receiving data, only when both power supply and WiFi connectivity are simultaneously available. Real-time data collected from the distance sensor and potentiometer is seamlessly sent to Google's [Firebase](https://firebase.google.com/) Realtime Database, where it is promptly updated in the Unity app in real-time. The status of working of all the controllers and sensors are also updated in real time.

The micro-controllers are programmed to automatically restart themselves in case of recurring errors caused by issues like power fluctuations, internet errors, sensor malfunctioning, etc.

A standout feature in our project was the introduction of a slider mechanism, allowing users to control the servo motor's angle. This not only emphasized user-centric design but also used an engaging element in the control system. The value of the slider mechanism sets the angle of the servo motor via database.

Additionally, the values recorded by potentiometer are periodically sent to the [ThingSpeak](https://thingspeak.com/) from which various graphs and MATLAB Visualisations can be obtained. Inside the app, a <em>Monitor</em> button is made which displays the data recorded in ThingSpeak in a form of a colourful [graph](Images/ThingSpeakGraph.png).

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
- Please ensure that you have libraries installed for [ESP32](https://randomnerdtutorials.com/installing-the-esp32-board-in-arduino-ide-windows-instructions/) and [ESP8266](https://randomnerdtutorials.com/how-to-install-esp8266-board-arduino-ide/) boards.
- Visual Studio: 2022
- Firebase SDK: firebase_unity_sdk_11.6.0
- MRTK: [1.0.2209.0](https://www.microsoft.com/en-us/download/details.aspx?id=102778) 
---
## Source Code

| File | Link | Description |
| ---- | ---- | ---------- |
| Database Script | [DataBaseManager.cs](Assets/DataBaseManager.cs) | Starts communication between Unity and Firebase and manages the overall display |
| Distance Script | [Distance.cs](Assets/Distance.cs) | Receives and displays distance related data |
| Potentiometer Script | [DCMotor.cs](Assets/DCMotor.cs) | Receives and displays potentiometer related data |
| Servo Script | [Servo.cs](Assets/Servo.cs) | Sends and displays servo related data |
| ESP32 Code | [CloudControllerV3.ino](MicroController/CloudControllerV3/CloudControllerV3.ino) | For Distance and Servo motor (Firebase) |
| NodeMCU Code | [NodeMCUCon.ino](MicroController/NodeMCUCon/NodeMCUCon.ino) | For Potentiometer related data to Firebase and Thingspeak|

---
## Setup

<table>
  <tr>
     <td align="center"><b>Diagram</b></td>
     <td align="center"><b>Implementation</b></td>
     <td align="center"><b>User Interface</b></td>
  </tr>
  <tr>
    <td><img src="https://github.com/Saarthak-Vijayvargiya-github/AR-VR-IoT-Monitoring/assets/96655833/83e2b742-4e53-4c10-9b6c-325c32db1e46" width=500></td>
    <td><img src="https://github.com/Saarthak-Vijayvargiya-github/AR-VR-IoT-Monitoring/assets/96655833/88cec7f4-abe0-46ba-8201-082b507d07bd" width=500></td>
    <td><img src="https://github.com/Saarthak-Vijayvargiya-github/AR-VR-IoT-Monitoring/assets/96655833/b08ada76-eeac-45b9-8e8b-b2f5dc216d6c" width=500></td>
  </tr>
</table>

The actual functioning of the system is demonstrated in the form of a [video](Images/Demo_Vid1.mp4), which you can download and watch. Alternatively, you can watch the same below in lower resolution, but <strong>make sure to switch the playback speed to 0.5x</strong> for better understanding.

https://github.com/Saarthak-Vijayvargiya-github/AR-VR-IoT-Monitoring/assets/96655833/5fd22154-9668-400b-8370-a8ddba5f90de


---
## Caution

- We have not uploaded the folder <em>[Assets/Firebase/](Assets/Firebase/)Plugins/</em> due to Github's file size constraints. Hence if you want to use this project, please configure the application with Firebase SDK inside Unity.
- The project may not work if Unity editor is not configured with MRTK.
- The google-services.json has been removed for privacy purposes. Hence make your own database.
- The internet information in [MicroController](MicroController/) codes have been removed for privacy purposes.

---
## Prerequisites

- Basic understanding of IoT devices and the Arduino.
- Basic understanding of the Unity Game Engine, C++ & Object Oriented programming.

---
## Disclaimer

As this is an open-ended project, it currently does not include any 3D models of IoT devices, but there is potential for expansion in that direction. Further enhancements could involve the incorporation of graphs, the utilization of more advanced microcontrollers, and the integration of various sensors. Currently, the Firebase SDK does not support the Universal Windows Platform (UWP), preventing the deployment and testing of the application on Hololens. However, we are confident that once the SDK supports UWP, the application should be able to work on Hololens. It's important to note that we do not claim the code to be entirely error-free, and there may be potential errors during its execution.

---
## Have fun! Thank-You!