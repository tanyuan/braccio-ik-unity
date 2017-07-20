/// Arduino Tinkerkit Braccio robotic arm simulator with IK (Inverse Kinematics) for Unity
/// Shan-Yuan Teng <tanyuan@cmlab.csie.ntu.edu.tw>

using UnityEngine;
using System.Collections;

public class SolveIK : MonoBehaviour {

	public bool useIK = true; // IK mode or manual adjustment
	public bool autoEnd = true; // horizontal end in IK mode

	public Vector3 targetPosition;
	public Vector3 currentPosition;

	[Range(0.0f, 180.0f)]
	public float thetaBase = 90f;
	[Range(15.0f, 165.0f)]
	public float thetaShoulder = 45f;
	[Range(0.0f, 180.0f)]
	public float thetaElbow = 180f;
	[Range(0.0f, 180.0f)]
	public float thetaWristVertical = 90f;
	[Range(0.0f, 180.0f)]
	public float thetaWristRotation = 0f;
	[Range(10.0f, 73.0f)]
	public float thetaGripper = 10f;

	public GameObject[] arms = new GameObject[5];

	/* Arm dimensions( m ) */
	float BASE_HGT = 0.078f;
	float HUMERUS = 0.124f;
	float ULNA = 0.124f;
	float GRIPPER = 0.058f;

	/* pre-calculations */
	float hum_sq;
	float uln_sq;

	void Start () {
		/* pre-calculations */
		hum_sq = HUMERUS*HUMERUS;
		uln_sq = ULNA*ULNA;
	}

	void Update () {

		// Set target position from itself
		targetPosition = transform.position;

		if (useIK) {
			SetArm (targetPosition.x, targetPosition.y, targetPosition.z, autoEnd);
		}

		// Update robot arm model
		arms [0].transform.localRotation = Quaternion.Euler(new Vector3 (0f, thetaBase, 0f));
		arms [1].transform.localRotation = Quaternion.Euler(new Vector3 (0f, 0f, thetaShoulder - 90f));
		arms [2].transform.localRotation = Quaternion.Euler(new Vector3 (0f, 0f, thetaElbow - 90f));
		arms [3].transform.localRotation = Quaternion.Euler(new Vector3 (0f, 0f, thetaWristVertical - 90f));
		arms [4].transform.localRotation = Quaternion.Euler(new Vector3 (0f, thetaWristRotation, 0f));

		// Current wrist position
		currentPosition = arms [3].transform.position;
	}

	void SetArm(float x, float y, float z, bool endHorizontal) {
		// Base angle
		float bas_angle_r = Mathf.Atan2( x, z );
		float bas_angle_d = bas_angle_r * Mathf.Rad2Deg + 90f;

		float wrt_y = y - BASE_HGT; // Wrist relative height to shoulder
		float s_w = x * x + z * z + wrt_y * wrt_y; // Shoulder to wrist distance square
		float s_w_sqrt = Mathf.Sqrt (s_w);

		// Elbow angle: knowing 3 edges of the triangle, get the angle
		float elb_angle_r = Mathf.Acos ((hum_sq + uln_sq - s_w) / (2f * HUMERUS * ULNA));
		float elb_angle_d = 270f - elb_angle_r * Mathf.Rad2Deg;

		// Shoulder angle = a1 + a2
		float a1 = Mathf.Atan2 (wrt_y, Mathf.Sqrt (x * x + z * z));
		float a2 = Mathf.Acos ((hum_sq + s_w - uln_sq) / (2f * HUMERUS * s_w_sqrt));
		float shl_angle_r = a1 + a2;
		float shl_angle_d = 180f - shl_angle_r * Mathf.Rad2Deg;

		// Keep end point horizontal
		if (endHorizontal) {
			float end_x = arms [4].transform.position.x;
			float end_y = arms [4].transform.position.y;
			float end_z = arms [4].transform.position.z;

			float end_last_angle = thetaWristVertical;

			float dx = end_x - x;
			float dz = end_z - z;

			float wrt_angle_r = Mathf.Atan2 (end_y - y, Mathf.Sqrt (dx * dx + dz * dz));
			float wrt_angle_d = end_last_angle + wrt_angle_r * Mathf.Rad2Deg;

			// Update angle
			if (wrt_angle_d >= 0f && wrt_angle_d <= 180f)
				thetaWristVertical = wrt_angle_d;
		}

		// Update angles
		if (bas_angle_d >= 0f && bas_angle_d <=180f)
			thetaBase = bas_angle_d;
		if (shl_angle_d >= 15f && shl_angle_d <=165f)
			thetaShoulder = shl_angle_d;
		if (elb_angle_d >= 0f && elb_angle_d <=180f)
			thetaElbow = elb_angle_d;
	}
}
