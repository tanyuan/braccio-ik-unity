using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoSerial : MonoBehaviour {
	public SolveIK solveIK;
	public string portName;
	SerialPort arduino;

	void Start () {
		arduino = new SerialPort (portName, 9600);
		arduino.Open ();
	}

	void Update () {
		if (arduino.IsOpen) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				arduino.Write ("B");
				arduino.Write (solveIK.thetaBase + "\n");
				arduino.Write ("S");
				arduino.Write (solveIK.thetaShoulder + "\n");
				arduino.Write ("E");
				arduino.Write (solveIK.thetaElbow + "180\n");
				arduino.Write ("W");
				arduino.Write (solveIK.thetaWristVertical + "\n");
				arduino.Write ("R");
				arduino.Write (solveIK.thetaWristRotation + "\n");
				arduino.Write ("G");
				arduino.Write (solveIK.thetaGripper + "\n");
				Debug.Log ("Serial: Send current angles");
			} else if (Input.GetKeyDown ("0")) {
				arduino.Write ("B");
				arduino.Write ("90\n");
				arduino.Write ("S");
				arduino.Write ("45\n");
				arduino.Write ("E");
				arduino.Write ("180\n");
				arduino.Write ("W");
				arduino.Write ("180\n");
				arduino.Write ("R");
				arduino.Write ("90\n");
				arduino.Write ("G");
				arduino.Write ("10\n");
				Debug.Log ("Serial: home angles");
			}
		}
	}
}
