using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {
	public enum PlayerState { NORMAL, HANGING_ON_EDGE, CLIMBING_FREE };
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
			} else {
				bool approaching = playerCC.velocity.x != 0 || playerCC.velocity.z != 0;
				if (approaching) {
					UpdateClimbWallWhileMoving();
				}
			}
		} else {
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");
			if (playerState == PlayerState.HANGING_ON_EDGE) {
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
			} else if (playerState == PlayerState.CLIMBING_FREE) {
				if (inputX != 0 || inputY != 0) {
					playerCC.Move(new Vector3(inputX, inputY, 0).normalized * 5 * Time.deltaTime);

					if (!ShootRaycastAndCheckIfHittingClimbableWall()) {
						Debug.Log("Heeyyyyyy get me off!");
						playerState = PlayerState.NORMAL;
						player.enabled = true;
					}
				}
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

	private void RaycastChecks (
		Vector3 lookVec,
		out RaycastHit rchit,
		out bool hitEdge,
		out bool hitHead
	) {
		hitEdge = DoRaycast(
			Color.red,
			transform.position,
			lookVec,
			out rchit,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);
		hitHead = DoRaycast(
			Color.red,
			transform.position + Vector3.up,
			lookVec,
			out rchit,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);
	}

	private void UpdateGrabEdgeWhileMidair () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		var rchit = new RaycastHit();
		bool hitEdge, hitHead;
		RaycastChecks(lookVec, out rchit, out hitEdge, out hitHead);

		if (hitEdge && !hitHead) {	// Hit middle body to wall but miss upper body to wall
			// Grab that ledge!!!
			Debug.Log("Hey hey spiderman");
			playerState = PlayerState.HANGING_ON_EDGE;
			player.enabled = false;
			playerCC.enabled = false;
			InchAndAdjustWhileHanging(lookVec);
		} else if (hitEdge && hitHead) {	// Investigate to see if it's climbable
			CheckIfClimbableWall(rchit, lookVec);
		}
	}

	private void UpdateClimbWallWhileMoving () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		var rchit = new RaycastHit();
		bool hitEdge, hitHead;
		RaycastChecks(lookVec, out rchit, out hitEdge, out hitHead);

		if (hitEdge && hitHead) {	// Investigate to see if it's climbable
			CheckIfClimbableWall(rchit, lookVec);
		}
	}

	private bool ShootRaycastAndCheckIfHittingClimbableWall () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		var rchit = new RaycastHit();
		bool hitEdge, hitHead;
		RaycastChecks(lookVec, out rchit, out hitEdge, out hitHead);

		if (hitEdge && hitHead) {
			return rchit.transform.tag == "Climbable";
		}
		return false;
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

	private void CheckIfClimbableWall (RaycastHit raycastHit, Vector3 lookVec) {
		if (raycastHit.transform.tag == "Climbable") {
			Debug.Log("Player is now climbing!");
			playerState = PlayerState.CLIMBING_FREE;
			player.enabled = false;
			// playerCC.enabled = false;
			SetupClimbing(raycastHit, lookVec);
		}
	}
	
	private void SetupClimbing (RaycastHit rchit, Vector3 lookVec) {
		var diff = Vector3.Distance(rchit.point, transform.position) - hangingDistAway;
		transform.position += lookVec.normalized * diff;
	}

	private Vector3 Origin () {
		return transform.position + new Vector3(0, hangingTopOfPlatformOffset);
	}
}
