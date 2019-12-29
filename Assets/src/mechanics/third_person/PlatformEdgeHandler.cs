using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {
	public enum PlayerState { NORMAL, HANGING_ON_EDGE, CLIMBING_FREE };
	public PlayerState playerState = PlayerState.NORMAL;
	private ThirdPersonControllerInput player;
	private CharacterController playerCC;
	public CameraInput cameraInput;

	// Use this for initialization
	void Start () {
		player = GetComponent<ThirdPersonControllerInput>();
		playerCC = GetComponent<CharacterController>();
		isOnGround = false;
	}
	
	void FixedUpdate () {
		FetchIsOnGround();
	}

	public bool overrideEh;
	// Update is called once per frame
	void Update () {
		if (playerState == PlayerState.NORMAL) {
			if (CanCheckEvents()) {
				UpdateJump();

				if (!IsOnGround()) {
					UpdateGrabEdgeWhileMidair();
				} else {
					bool approaching = playerCC.velocity.x != 0 || playerCC.velocity.z != 0;
					if (approaching) {
						UpdateClimbWallWhileMoving();
					}
				}
			}
		} else {
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");
			if (playerState == PlayerState.HANGING_ON_EDGE) {
				if (hangingDebounce <= 0 &&
					(inputX != 0 || inputY != 0 || overrideEh)) {
					Debug.Log("Undo!");
					playerState = PlayerState.NORMAL;
					player.enabled = true;
					player.ResetMvtBuildup();
					playerCC.enabled = true;

					ClimbOutOfLedge(new Vector3(0, 1.75f, 0.75f));
				} else {
					hangingDebounce -= Time.deltaTime;
				}
			} else if (playerState == PlayerState.CLIMBING_FREE) {
				if (inputX != 0 || inputY != 0) {
					Vector3 movement = Quaternion.LookRotation(player.GetLookDirection()) * new Vector3(inputX, inputY, 1).normalized * 5 * Time.deltaTime;
					Debug.Log("HAHAHAH\n" + player.GetLookDirection());
					playerCC.Move(movement);
					ProcessClimbing();

					var lookDir = player.GetLookDirection();
					cameraInput.mouseX = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
					cameraInput.mouseY = 0;
				}
			}
		}
	}

	private bool isOnGround;
	private RaycastHit hitInfo;
	private void FetchIsOnGround () {		// Only call once per frame
		isOnGround = DoDownwardsIsOnGroundRaycast(out this.hitInfo, 0.5f);
	}

	private bool DoDownwardsIsOnGroundRaycast (out RaycastHit hitInfo, float rayDist) {
		return DoRaycast(
			Color.red,
			transform.position + Vector3.down,
			Vector3.down,
			out hitInfo,
			rayDist,
			1 << LayerMask.NameToLayer("Ground")
		);
	}

	public static bool DoRaycast (
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
		if (Input.GetButtonDown("Jump")) {
			SendMessage("RequestJump");
		}
	}

	private void RaycastChecks (
		Vector3 lookVec,
		out RaycastHit rchit,
		out bool hitEdge,
		out bool hitHead
	) {
		var rchit1 = new RaycastHit();
		var rchit2 = new RaycastHit();
		
		hitEdge = DoRaycast(
			Color.red,
			transform.position + new Vector3(0, -0.1f),
			lookVec,
			out rchit1,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);
		hitHead = DoRaycast(
			Color.red,
			transform.position + Vector3.up,
			lookVec,
			out rchit2,
			1.0f,
			1 << LayerMask.NameToLayer("Ground")
		);

		if (!hitHead) rchit = rchit1;
		else rchit = rchit2;
	}

	private void UpdateGrabEdgeWhileMidair () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		var rchit = new RaycastHit();
		bool hitEdge, hitHead;
		RaycastChecks(lookVec, out rchit, out hitEdge, out hitHead);

		if (hitEdge && !hitHead) {	// Hit middle body to wall but miss upper body to wall
			InvokeHanging(rchit, lookVec);
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
		} else if (IsOnGround()) {
			float aboveHeadLevelHeight = 2.5f;
			RaycastHit hitInfo;
			bool hitAbove = DoRaycast(
				Color.red,
				transform.position + Vector3.up,
				Vector3.up,
				out hitInfo,
				aboveHeadLevelHeight,
				1 << LayerMask.NameToLayer("Ground")
			);
			bool hitAboveHeadLevel = DoRaycast(
				Color.red,
				transform.position + Vector3.up + new Vector3(0, aboveHeadLevelHeight),
				lookVec,
				out hitInfo,
				1.0f,
				1 << LayerMask.NameToLayer("Ground")
			);
			if (!hitAbove && !hitAboveHeadLevel) {
				InvokeHanging(raycastHit, lookVec);
			}
		}
	}
	
	private void SetupClimbing (RaycastHit rchit, Vector3 lookVec) {
		var diff = Vector3.Distance(rchit.point, transform.position) - hangingDistAway;
		Vector3 mov = lookVec.normalized * diff;
		Debug.Log("LookVec!!!\n" + rchit.normal);
		player.UpdateLookDirection(-rchit.normal);
		mov.y += 0.25f;
		transform.position += mov;
		SendMessage("TurnOnClimbing");
	}

	private void ProcessClimbing () {
		RaycastHit garb;
		if (DoDownwardsIsOnGroundRaycast(out garb, 0.1f)) {
			UndoClimbing();
			return;
		}

		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		var rchit = new RaycastHit();
		bool hitEdge, hitHead;
		RaycastChecks(lookVec, out rchit, out hitEdge, out hitHead);

		if (!hitEdge) {
			UndoClimbing();
		} else if (hitEdge && !hitHead) {
			InvokeHanging(rchit, lookVec);
		} else if (hitEdge && hitHead) {
			// Update look direction
			player.UpdateLookDirection(-rchit.normal);
		}
	}

	private void ClimbOutOfLedge (Vector3 climbMvt) {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		Vector3 jojo = Quaternion.LookRotation(lookVec) * climbMvt;
		Debug.Log(jojo);
		player.transform.position += jojo;

		playerCC.Move(new Vector3(0, player.FetchYVelo(false), 0));		// Force player to ground
		SendMessage("TriggerGetUpAnim");
	}

	private float hangingDebounce = 0.35f;
	private void InvokeHanging (RaycastHit rchit, Vector3 lookVec) {
		// Grab that ledge!!!
		Debug.Log("Hey hey spiderman");
		playerState = PlayerState.HANGING_ON_EDGE;
		hangingDebounce = 0.35f;
		player.enabled = false;
		playerCC.enabled = false;
		player.UpdateLookDirection(-rchit.normal);
		InchAndAdjustWhileHanging(lookVec);
		SendMessage("TurnOffClimbing");
		SendMessage("TriggerHangAnim");
	}

	void UndoClimbing () {
		Debug.Log("Heeyyyyyy get me off!");
		playerState = PlayerState.NORMAL;
		player.enabled = true;
		player.ResetMvtBuildup();
		SendMessage("TurnOffClimbing");
	}

	private Vector3 Origin () {
		return transform.position + new Vector3(0, hangingTopOfPlatformOffset);
	}

	float debounceTime = 0;
	void DebounceEvents (float time) {
		debounceTime = time;
	}

	private bool CanCheckEvents () {
		if (debounceTime > 0) {
			debounceTime -= Time.deltaTime;
		} else if (debounceTime <= 0) {
			debounceTime = 0;
			return true;
		}
		return false;
	}
}
