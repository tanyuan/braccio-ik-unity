
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

  readSerial();

  Braccio.ServoMovement(stepDelay, m1, m2, m3, m4, m5, m6);

  delay(100);
}

void readSerial() {
  if (Serial.available()) {
    char data = Serial.read();
    if (Serial.available()) {
      switch (data) {
        case 'B':
          m1 = Serial.parseInt();
          break;
        case 'S':
          m2 = Serial.parseInt();
          break;
        case 'E':
          m3 = Serial.parseInt();
          break;
        case 'W':
          m4 = Serial.parseInt();
          break;
        case 'R':
          m5 = Serial.parseInt();
          break;
        case 'G':
          m6 = Serial.parseInt();
          break;
      }
    }
  }
}

