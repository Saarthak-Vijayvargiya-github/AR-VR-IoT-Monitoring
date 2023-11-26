#if defined(ESP32)
  #include <WiFi.h>
#elif defined(ESP8266)
  #include <ESP8266WiFi.h>
#endif
#include <Firebase_ESP_Client.h>
#include "addons/TokenHelper.h"
#include "addons/RTDBHelper.h"

// Definitions for Internet 
#define WIFI_SSID "your-wifi-ssid"
#define WIFI_PASSWORD "your-wifi-pass"
#define API_KEY "your-Firebase-API-KEY"
#define DATABASE_URL "your-firebase-database-url"

// Firebase objects
FirebaseData fbdo;
FirebaseAuth auth;
FirebaseConfig config;

// Firebase variables
bool signupOK = false;
short countError = 0;
String distInstance = "distanceSensor";
String servoInstance = "servo";
String sysInstance = "system";


#include<ESP32Servo.h>
#define trig 27
#define echo 26
#define distStat 14
#define servoPin 21
#define servoStat 22

// Ultrasonic variables
long duration;    int distance;

// Servo Variables
Servo s1;   short deg = 0;

// Utilities
void wifiSetup(){
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Connecting to Wi-Fi");
  while (WiFi.status() != WL_CONNECTED){
    Serial.print(".");
    delay(300);
  }
  Serial.println();
  Serial.print("Connected with IP: ");
  Serial.println(WiFi.localIP());
  Serial.println();
}

void firebaseSetup(){
  config.api_key = API_KEY;
  config.database_url = DATABASE_URL;
  while(1){
    if (Firebase.signUp(&config, &auth, "", "")){   // Anonymous Signin
      Serial.println("ok");
      signupOK = true;
      break;
    }
    else{
      Serial.printf("%s\n", config.signer.signupError.message.c_str());
    }
  }
  config.token_status_callback = tokenStatusCallback; //see addons/TokenHelper.h
  Firebase.begin(&config, &auth);
  Firebase.reconnectWiFi(true);
}

void setup(){
  Serial.begin(115200);
  pinMode(trig, OUTPUT);
  pinMode(echo, INPUT);
  pinMode(distStat, INPUT);
  pinMode(servoPin, OUTPUT);
  pinMode(servoStat, INPUT);
  s1.attach(servoPin);

  wifiSetup();
  firebaseSetup();
}

// Sensors
void DistanceData(){
    if (Firebase.RTDB.setBool(&fbdo, distInstance + "/distWorking", digitalRead(distStat))){
      Serial.println("PATH: " + fbdo.dataPath());
    }
    else {
      Serial.println("FAILED! REASON: " + fbdo.errorReason());
      countError++;
    }
    if (Firebase.RTDB.setInt(&fbdo, distInstance + "/distance", distance)){
      Serial.println("PATH: " + fbdo.dataPath());
    }
    else {
      Serial.println("FAILED! REASON: " + fbdo.errorReason());
      countError++;
    }
}

void ServoData(){
    if (Firebase.RTDB.setBool(&fbdo, servoInstance + "/servoWorking", digitalRead(servoStat))){
      Serial.println("PATH: " + fbdo.dataPath());
    }
    else {
      Serial.println("FAILED! REASON: " + fbdo.errorReason());
      countError++;
    }
    if (Firebase.RTDB.getInt(&fbdo, servoInstance + "/angle")){
      deg = fbdo.intData();
      Serial.println("Successful Read: " + fbdo.dataPath());
    }
    else {
      Serial.println("FAILED! REASON: " + fbdo.errorReason());
      countError++;
    }
}

void loop(){
    distance:
      digitalWrite(trig, LOW);
      delay(2);
      digitalWrite(trig, HIGH);
      delay(50);
      digitalWrite(trig, LOW);
      duration = pulseIn(echo, HIGH);
      distance = duration*(0.034/2);

     servo:
      s1.write(deg);
    
    if (Firebase.ready() && signupOK){
      Firebase.RTDB.setBool(&fbdo, sysInstance + "/controller", true);
      DistanceData();
      ServoData();
    }
    if(countError > 100){
      countError = 0;
      ESP.restart();
    }
}
