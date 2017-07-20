using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoSerial : MonoBehaviour {
	public SolveIK solveIK;
	public int delaySeconds = 5;
	public string portName;
	SerialPort arduino;
	bool startCommands = false;

	void Start () {
		arduino = new SerialPort (portName, 9600);
		arduino.Open ();

	}

	void Update () {
		if (startCommands == false) 
			StartCoroutine (SendCommands ());
	}

	IEnumerator SendCommands () {
		startCommands = true;
		yield return new WaitForSeconds(delaySeconds);

		if (arduino.IsOpen) {
			string str;

			string thetaBaseStr = (Mathf.RoundToInt(solveIK.thetaBase)).ToString("000");
			string thetaShoulderStr = (Mathf.RoundToInt(solveIK.thetaShoulder)).ToString("000");
			string thetaElbowStr = (Mathf.RoundToInt(solveIK.thetaElbow)).ToString("000");
			string thetaWristVerticalStr = (Mathf.RoundToInt(solveIK.thetaWristVertical)).ToString("000");
			string thetaWristRotationStr = (Mathf.RoundToInt(solveIK.thetaWristRotation)).ToString("000");
			string thetaGripperStr = (Mathf.RoundToInt(solveIK.thetaGripper)).ToString("000");

			str = thetaBaseStr + thetaShoulderStr + thetaElbowStr + thetaWristVerticalStr + thetaWristRotationStr + thetaGripperStr + "\n";

			arduino.Write (str);
		
			Debug.Log ("Send Serial: " + str);
		}
		startCommands = false;

	}
}
