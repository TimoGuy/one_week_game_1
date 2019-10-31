using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {
	public enum PlayerState { NORMAL, HANGING_ON_EDGE };
	public PlayerState playerState = PlayerState.NORMAL;
	private ThirdPersonControllerInput player;
	private CharacterController playerCC;

	// Use this for initialization
	void Start () {
		player = GetComponent<ThirdPersonControllerInput>();
		playerCC = GetComponent<CharacterController>();
		isOnGround = false;
		prevIsGrounded = false;
	}
	
	void FixedUpdate () {
		FetchIsOnGround();
	}

	public Vector3 climbMvt = new Vector3(0, 1.75f, 0.75f);
	public bool overrideEh;
	// Update is called once per frame
	void Update () {
		if (playerState == PlayerState.NORMAL) {
			UpdateJump();
			if (!IsOnGround()) {
				UpdateGrabEdgeWhileMidair();
			}
		} else if (playerState == PlayerState.HANGING_ON_EDGE) {
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");
			if (inputX != 0 || inputY != 0 || overrideEh) {
				Debug.Log("Undo!");
				playerState = PlayerState.NORMAL;
				player.enabled = true;
				playerCC.enabled = true;

				var lookVec = player.GetLookDirection();
				lookVec.y = 0;
				Vector3 jojo = Quaternion.Euler(lookVec) * climbMvt;
				Debug.Log(jojo);
				player.transform.position += jojo;
				playerCC.Move(Vector3.zero);
			}
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
		Debug.DrawRay(ray.origin, ray.direction, rayColor);

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
			playerState = PlayerState.HANGING_ON_EDGE;
			player.enabled = false;
			playerCC.enabled = false;
			InchAndAdjustWhileHanging(lookVec);
		}
	}

	public float hangingTopOfPlatformOffset = 0.75f;
	public float hangingDistAway = 0.5f;
	public Vector3 inchFactor = new Vector3(0, 0.05f);
	private void InchAndAdjustWhileHanging (Vector3 lookVec) {
		RaycastHit hitInfo;

		bool up = DoRaycast(
			Color.yellow,
			Origin(),
			lookVec,
			out hitInfo,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);

		if (up) {
			// Inch up until no more collision
			bool colliding = true;
			while (colliding) {
				transform.position += inchFactor;
				colliding = DoRaycast(
					Color.yellow,
					Origin(),
					lookVec,
					out hitInfo,
					1.0f,
					1 << LayerMask.NameToLayer("Ground")
				);
			}
		} else {
			// Inch down until feel collision
			bool colliding = false;
			while (!colliding) {
				transform.position -= inchFactor;
				colliding = DoRaycast(
					Color.yellow,
					Origin() - inchFactor,
					lookVec,
					out hitInfo,
					1.0f,
					1 << LayerMask.NameToLayer("Ground")
				);
			}
		}

		var diff = Vector3.Distance(hitInfo.point, transform.position) - hangingDistAway;
		transform.position += lookVec.normalized * diff;
	}

	private Vector3 Origin () {
		return transform.position + new Vector3(0, hangingTopOfPlatformOffset);
	}
}
