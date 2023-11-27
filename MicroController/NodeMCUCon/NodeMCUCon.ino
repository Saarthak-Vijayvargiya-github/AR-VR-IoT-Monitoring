#if defined(ESP32)
  #include <WiFi.h>
#elif defined(ESP8266)
  #include <ESP8266WiFi.h>
#endif
#include <Firebase_ESP_Client.h>
#include "addons/TokenHelper.h"
#include "addons/RTDBHelper.h"
#include<ThingSpeak.h>

#define WIFI_SSID "your-wifi-ssid"
#define WIFI_PASSWORD "your-wifi-pass"
#define API_KEY "your-Firebase-API-KEY"
#define DATABASE_URL "your-firebase-database-url"
#define CHANNEL_ID your-thingspeak-channelID
#define WRITE_API_KEY "your-thingspeak-writeAPI-Key"

FirebaseData fbdo;
FirebaseAuth auth;
FirebaseConfig config;
WiFiClient client;

bool signupOK = false;
unsigned long sendDataPrevMillis = 0;
short countError = 0;
String dcMotorInstance = "dcMotor";
String sysInstance = "system";

#define motorPot A0
#define motorPin D3
#define motorStat D1

short volt;
short dutyCycle;

void setup(){
  pinMode(motorPin, OUTPUT);
  pinMode(motorPot, INPUT);
  pinMode(motorStat, INPUT);
  Serial.begin(115200);

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
  ThingSpeak.begin(client);
}

void DCMotorData(){
  if (Firebase.RTDB.setBool(&fbdo, dcMotorInstance + "/dcMotorWorking", digitalRead(motorStat))){
    //Serial.println("PATH: " + fbdo.dataPath());
  }
  else {
    Serial.println("FAILED! REASON: " + fbdo.errorReason());
    countError++;
  }
  if (Firebase.RTDB.setInt(&fbdo, dcMotorInstance + "/dutyCycle", dutyCycle)){
    //Serial.println("PATH: " + fbdo.dataPath());
  }
  else {
    Serial.println("FAILED! REASON: " + fbdo.errorReason());
    countError++;
  }
}

void loop(){
  motor:
    volt = analogRead(motorPot);
    dutyCycle = map(volt,0,1023,0,100);
    volt = map(volt,0,1023,0,255);
    Serial.println(volt);
    analogWrite(0, volt);
    Firebase.RTDB.setBool(&fbdo, sysInstance + "/controller2", true);
    DCMotorData();
    if ((millis() - sendDataPrevMillis) > 15000 || sendDataPrevMillis == 0){
      ThingSpeak.writeField(CHANNEL_ID, 1, dutyCycle, WRITE_API_KEY);
      sendDataPrevMillis = millis();
    }
    if(countError > 100){
      countError = 0;
      ESP.restart();
    }
}
