
/*
  simpleMovements.ino

  This  sketch simpleMovements shows how they move each servo motor of Braccio

  Created on 18 Nov 2015
  by Andrea Martino

  This example is in the public domain.
*/

#include <Braccio.h>
#include <Servo.h>

Servo base;
Servo shoulder;
Servo elbow;
Servo wrist_rot;
Servo wrist_ver;
Servo gripper;

int stepDelay = 20;
int m1 = 90;
int m2 = 45;
int m3 = 180;
int m4 = 180;
int m5 = 90;
int m6 = 10;

char inData[25];
bool isRead = false;
int index = 0;

void setup() {
  //Initialization functions and set up the initial position for Braccio
  //All the servo motors will be positioned in the "safety" position:
  //Base (M1):90 degrees
  //Shoulder (M2): 45 degrees
  //Elbow (M3): 180 degrees
  //Wrist vertical (M4): 180 degrees
  //Wrist rotation (M5): 90 degrees
  //gripper (M6): 10 degrees
  Braccio.begin();
  Serial.begin(9600);
}

void loop() {
  /*
    Step Delay: a milliseconds delay between the movement of each servo.  Allowed values from 10 to 30 msec.
    M1=base degrees. Allowed values from 0 to 180 degrees
    M2=shoulder degrees. Allowed values from 15 to 165 degrees
    M3=elbow degrees. Allowed values from 0 to 180 degrees
    M4=wrist vertical degrees. Allowed values from 0 to 180 degrees
    M5=wrist rotation degrees. Allowed values from 0 to 180 degrees
    M6=gripper degrees. Allowed values from 10 to 73 degrees. 10: the toungue is open, 73: the gripper is closed.
  */

  readSerialStr();

  Braccio.ServoMovement(stepDelay, m1, m2, m3, m4, m5, m6);

  delay(100);
}

void readSerialStr() {
      Serial.println("Serial1");
  if (Serial.available() > 0) {
    Serial.println("Serial2");
    char incomingByte = Serial.read();
    while (incomingByte != '\n' && isDigit(incomingByte)) {
      Serial.println("Serial3");
      Serial.println(incomingByte);
      isRead = true;
      delay(100);
      inData[index] = incomingByte;
      index++;
      incomingByte = Serial.read();
    }
    inData[index] = '\0';
  }

  if (isRead) {
    char m1_char[4];
    char m2_char[4];
    char m3_char[4];
    char m4_char[4];
    char m5_char[4];
    char m6_char[4];

    m1_char[0] = inData[0];
    m1_char[1] = inData[1];
    m1_char[2] = inData[2];
    m1_char[3] = '\0';
    m2_char[0] = inData[3];
    m2_char[1] = inData[4];
    m2_char[2] = inData[5];
    m2_char[3] = '\0';
    m3_char[0] = inData[6];
    m3_char[1] = inData[7];
    m3_char[2] = inData[8];
    m3_char[3] = '\0';
    m4_char[0] = inData[9];
    m4_char[1] = inData[10];
    m4_char[2] = inData[11];
    m4_char[3] = '\0';
    m5_char[0] = inData[12];
    m5_char[1] = inData[13];
    m5_char[2] = inData[14];
    m5_char[3] = '\0';
    m6_char[0] = inData[15];
    m6_char[1] = inData[16];
    m6_char[2] = inData[17];
    m6_char[3] = '\0';

    m1 = atoi(m1_char);
    m2 = atoi(m2_char);
    m3 = atoi(m3_char);
    m4 = atoi(m4_char);
    m5 = atoi(m5_char);
    m6 = atoi(m6_char);

    isRead = false;
    index = 0;
  }

}
