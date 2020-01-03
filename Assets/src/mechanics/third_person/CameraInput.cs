using UnityEngine;
using System.Collections;


/// https://answers.unity.com/questions/395369/3rd-person-controller-1.html
public class CameraInput : MonoBehaviour {
	public Transform targetLookAt;
	
	public float distance = 5;
	public float minDistance = 3;
	public float maxDistance = 10;
	public float distanceBuffer = 1;

	public float xMouseSensitivity = 5;
	public float yMouseSensitivity = 5;
	public float mouseWheelSensitivity = 5;
	public float yMinLimit = -40;
	public float yMaxLimit = 80;


	public float mouseX, mouseY = 5;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		Reset();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (targetLookAt == null)
        	return;

		HandlePlayerInput();
			
		Vector3 newPos = CalculateDesiredPosition(mouseX, mouseY);
			
		UpdatePosition(newPos);

		transform.position += shakeOffset;
		shakeOffset = Vector3.zero;
	}

	private void Reset () {

	}

	private void HandlePlayerInput () {
		mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;

		mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);
	}

	private Vector3 CalculateDesiredPosition (float rotX, float rotY) {
		Vector3 dist = new Vector3(0, 0, -distance);
		Quaternion rot = Quaternion.Euler(rotY, rotX, 0);
		dist.z = -GetDistanceWithoutColliding(rot * dist, distance);
		return targetLookAt.position + (rot * dist);
	}

	private float GetDistanceWithoutColliding (Vector3 direction, float maxDistance) {
		RaycastHit rHit;
		bool hit = PlatformEdgeHandler.DoRaycast(
			Color.magenta,
			targetLookAt.position,
			direction,
			out rHit,
			maxDistance,
			(1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Camera Wall")));
		if (hit) {
			return rHit.distance - distanceBuffer;
		}

		return maxDistance - distanceBuffer;
	}

	private void UpdatePosition (Vector3 newPos) {
		transform.position = newPos;
		transform.LookAt(targetLookAt);
	}

	float ClampAngle (float angle, float min, float max) {
		while (angle < -360 || angle > 360) {
			if (angle < -360) angle += 360;
			if (angle > 360) angle -= 360;
		}

		return Mathf.Clamp(angle, min, max);
	}

	private Vector3 shakeOffset = Vector3.zero;
	void Message_ShakeCam (float shakeAmount) {
		shakeOffset = Random.insideUnitSphere * shakeAmount;
	}
}
