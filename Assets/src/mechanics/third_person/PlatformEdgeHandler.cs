using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {
	public enum PlayerState { NORMAL, HANGING_ON_EDGE, CLIMBING_FREE, WAIT_ON_GET_UP_ANIM };
	public PlayerState playerState = PlayerState.NORMAL;
	private ThirdPersonControllerInput player;
	private CharacterController playerCC;
	private CameraInput cameraInput;

	// Use this for respawning
	private Vector3 respawnPosition;
	private float respawnMidairTime = 2;
	private float currentMidairTime = 0;

	void Start () {
		player = GetComponent<ThirdPersonControllerInput>();
		playerCC = GetComponent<CharacterController>();
		cameraInput = player.myCamera.GetComponent<CameraInput>();
		FetchIsOnGround();
	}
	
	void FixedUpdate () {
		FetchIsOnGround();
		if (playerState == PlayerState.NORMAL) {
			if (IsOnGround() && cameraInput.enabled) {
				SetRespawnPoint(transform.position);
			} else  {
				IncrementMidairTime(Time.deltaTime);
			}
		}
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
					StartClimbOutOfLedge();
				} else {
					hangingDebounce -= Time.deltaTime;
				}
			} else if (playerState == PlayerState.WAIT_ON_GET_UP_ANIM) {
				if (hangingDebounce <= 0) {
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
				SendMessage("SetClimbBlendCoords", new object[] { inputX, inputY });
				if (inputX != 0 || inputY != 0) {
					Vector3 movement = Quaternion.LookRotation(player.GetLookDirection()) * new Vector3(inputX, inputY, 1).normalized * 5 * Time.deltaTime;
					playerCC.Move(movement);
					ProcessClimbing();
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

	private float jumpFalloffTimer = 0;
	private float jumpFalloffDebounce = 0.2f;
	private void UpdateJump () {
		if (IsOnGround()) {
			jumpFalloffTimer = 0;
		} else {
			jumpFalloffTimer += Time.deltaTime;
		}

		if (Input.GetButtonDown("Jump") &&
			player.isActiveAndEnabled &&
			jumpFalloffTimer < jumpFalloffDebounce) {
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

	private void StartClimbOutOfLedge () {
		SendMessage("TriggerGetUpAnim");
		playerState = PlayerState.WAIT_ON_GET_UP_ANIM;
		hangingDebounce = 0.4166666666667f;
	}

	private void ClimbOutOfLedge (Vector3 climbMvt) {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		Vector3 jojo = Quaternion.LookRotation(lookVec) * climbMvt;
		Debug.Log(jojo);
		player.transform.position += jojo;

		playerCC.Move(new Vector3(0, player.FetchYVelo(false), 0));		// Force player to ground
		SendMessage("TurnOffClimbing");
		SendMessage("TurnOffMidair");
	}

	private float hangingDebounce = 0.15f;
	private void InvokeHanging (RaycastHit rchit, Vector3 lookVec) {
		// Grab that ledge!!!
		Debug.Log("Hey hey spiderman");
		playerState = PlayerState.HANGING_ON_EDGE;
		hangingDebounce = 0.15f;
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

	private void SetRespawnPoint (Vector3 pos) {
		respawnPosition = pos;
		currentMidairTime = 0;
	}

	private void IncrementMidairTime (float time) {
		currentMidairTime += time;
		if (currentMidairTime >= respawnMidairTime) {
			BottomlessPitHandler.SetEnabledCameraInput(true);
			transform.position = respawnPosition;
			SetRespawnPoint(transform.position);
			player.ResetMvtBuildup();
			isOnGround = true;
		}
	}
}
