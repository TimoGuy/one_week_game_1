using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {
	private ThirdPersonControllerInput player;

	// Use this for initialization
	void Start () {
		player = GetComponent<ThirdPersonControllerInput>();
		isOnGround = false;
		prevIsGrounded = false;
	}
	
	void FixedUpdate () {
		FetchIsOnGround();
	}

	// Update is called once per frame
	void Update () {
		UpdateJump();
		if (!IsOnGround()) {
			UpdateGrabEdgeWhileMidair();
		}
	}

	private bool isOnGround;
	private bool prevIsGrounded;
	private RaycastHit hitInfo;
	private void FetchIsOnGround () {		// Only call once per frame
		isOnGround = DoRaycast(
			Color.red,
			transform.position + Vector3.down,
			Vector3.down,
			out hitInfo,
			0.5f,
			1 << LayerMask.NameToLayer("Ground")
		);
	}

	private bool DoRaycast (
		Color rayColor,
		Vector3 origin,
		Vector3 direction,
		out RaycastHit hitInfo,
		float rayDist,
		int layerMask
	) {
		Ray ray = new Ray();
		ray.origin = origin;
		ray.direction = direction;
		Debug.DrawRay(ray.origin, ray.direction, Color.red);

		return Physics.Raycast(
			ray,
			out hitInfo,
			rayDist,
			layerMask
		);
	}

	// Getter
	public bool IsOnGround (out RaycastHit hitInfo) {
		hitInfo = this.hitInfo;
		return isOnGround;
	}

	// Getter
	public bool IsOnGround () {
		RaycastHit garb;
		return IsOnGround(out garb);
	}

	private void UpdateJump () {
		if (!IsOnGround() &&
			prevIsGrounded) {
			SendMessage("RequestJump");
		}

		prevIsGrounded = IsOnGround();
	}

	private void UpdateGrabEdgeWhileMidair () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;

		var garb = new RaycastHit();
		bool hitEdge = DoRaycast(
			Color.red,
			transform.position,
			lookVec,
			out garb,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);
		bool hitHead = DoRaycast(
			Color.red,
			transform.position + Vector3.up,
			lookVec,
			out garb,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);
		if (hitEdge && !hitHead) {	// Hit middle body to wall but miss upper body to wall
			// Grab that ledge!!!
			Debug.Log("Hey hey spiderman");
		}
	}
}
