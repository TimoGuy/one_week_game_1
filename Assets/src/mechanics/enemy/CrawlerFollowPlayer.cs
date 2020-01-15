using UnityEngine;
using System.Collections;

public class CrawlerFollowPlayer : MonoBehaviour {
	public float recognizeIsNearThreshold = 5;
	public float followSpeed = 1;
	public float acceleration = 0.05f;
	public float turnSpeed = 10;
	public float deadZoneTurn = 5;
	public Rigidbody myRigidbody;
	public ThirdPersonControllerInput player;
	private EnemyWeapon enemyWeapon;
	private float currentSpeed = 0;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if (player == null) {
			Debug.LogError("player assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (myRigidbody == null) {
			Debug.LogError("myRigidbody assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
		enemyWeapon = GetComponentInChildren<EnemyWeapon>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 lookDirection = player.transform.position - transform.position;
		lookDirection.y = 0;

		if (Vector3.Distance(lookDirection, Vector3.zero) < recognizeIsNearThreshold) {
			float direction = //ToRegularAngleBounds(
				90 - Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg;
			//);

			float deltaDir = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, direction);
			// Debug.Log("Lookdir: " + lookDirection + ";;; dir: " + direction + ";;; myDir: " + transform.rotation.eulerAngles.y + ";;; deltaDir: " + deltaDir);
			if (Mathf.Abs(deltaDir) >= deadZoneTurn) {
				transform.rotation =
					Quaternion.Euler(new Vector3(
						0,
						transform.rotation.eulerAngles.y + Mathf.Clamp(deltaDir, -turnSpeed, turnSpeed)
					)
				);
			} else {
				transform.rotation =
					Quaternion.Euler(new Vector3(
						0,
						direction
					)
				);
			}

			// Move to/from player
			if (player.IsHurt()) {
				// Move from
				currentSpeed = Mathf.Lerp(currentSpeed, -followSpeed, acceleration);
			} else {
				// Move to
				currentSpeed = Mathf.Lerp(currentSpeed, followSpeed, acceleration);
			}
		} else {
			// Stop
			currentSpeed = Mathf.Lerp(currentSpeed, 0, acceleration);
		}
		if (myRigidbody == null) {
			this.enabled = false;		// This means the underlying enemy is dead
			return;
		}
		Debug.Log("Sppeeed: " + (-(transform.rotation * Vector3.forward) * currentSpeed));
		// myRigidbody.AddForce(-(transform.rotation * Vector3.forward) * currentSpeed, ForceMode.Impulse);
		myRigidbody.velocity = -(transform.rotation * Vector3.forward) * currentSpeed;
	}

	private static float ToRegularAngleBounds (float angle) {
		while (angle < 0) angle += 360;
		while (angle >= 360) angle -= 360;
		return angle;
	}
}
