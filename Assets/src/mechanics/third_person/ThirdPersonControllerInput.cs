using UnityEngine;
using System.Collections;

public class ThirdPersonControllerInput : MonoBehaviour {
	public Transform myCamera;
	public WeaponSwordAttack weaponSwordAttack;
	public float mvtSpeed = 5;
	public float mvtAccel = 1;
	public float gravityConst = 0.5f;
	public float jumpHeight = 10;

	private CharacterController characterController;
	private PlatformEdgeHandler platformEdgeHandler;

	public bool targetingMode;
	private LockonTargeter targeterHandler;
	private CameraInput cameraInput;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		platformEdgeHandler = GetComponent<PlatformEdgeHandler>();
		targeterHandler = GetComponent<LockonTargeter>();
		cameraInput = myCamera.GetComponent<CameraInput>();
		ResetMvtBuildup();
		SaveStartTransform();
	}
	
	private float mvtBuildup;
	private Vector2 normalizedInputVec;
	void FixedUpdate () {
		if (!platformEdgeHandler.IsOnGround()) return;
		float inputX = 0, inputY = 0;
		if (weaponSwordAttack.IsCurrentlyIdling()) {
			inputX = Input.GetAxisRaw("Horizontal");
			inputY = Input.GetAxisRaw("Vertical");
		}
		
		if (inputX != 0 || inputY != 0) {
			mvtBuildup += mvtAccel;
			normalizedInputVec = new Vector2(inputX, inputY).normalized;
			UpdateLookDirection(
				CalcCameraLookDir(normalizedInputVec)
			);
		} else {
			mvtBuildup -= mvtAccel;
		}
		mvtBuildup = Mathf.Clamp(mvtBuildup, 0, mvtSpeed);
	}

	private Vector3 moveVector;
	// Update is called once per frame
	void Update () {
		var lookDir = GetLookDirection();
		moveVector = lookDir.normalized * mvtBuildup;
		moveVector = new Vector3(moveVector.x, FetchYVelo(), moveVector.z);

		characterController.Move(moveVector * Time.deltaTime);

		if (targetingMode) {
			var relative = GetFlattenedForwardCamera();
			cameraInput.mouseX = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
		}
	}

	public void ResetMvtBuildup () {
		mvtBuildup = 0;
	}

	private Vector3 __pos;
	private Quaternion __rot;
	private Vector3 __scal;
	private void SaveStartTransform () {
		__pos = transform.localPosition;
		__rot = transform.localRotation;
		__scal = transform.localScale;
	}

	public void ResetTransform () {
		transform.localPosition = __pos;
		transform.localRotation = __rot;
		transform.localScale = __scal;
	}

	bool _reqJump = false;
	public void RequestJump () {
		_reqJump = true;
	}

	private Vector3 GetFlattenedForwardCamera () {
		if (targetingMode) {
			Vector3 lookDir = targeterHandler.lockingTarget.position - transform.position;
			lookDir.y = 0;
			return lookDir.normalized;
		}
		return new Vector3(myCamera.forward.x, 0, myCamera.forward.z).normalized;
	}

	private Vector3 GetFlattenedRightCamera () {
		if (targetingMode) {
			Vector3 lookDir = targeterHandler.lockingTarget.position - transform.position;
			lookDir.y = 0;
			return Quaternion.Euler(0, 90, 0) * lookDir.normalized;
		}
		return new Vector3(myCamera.right.x, 0, myCamera.right.z).normalized;
	}

	private bool _isJumping = false;
	public float FetchYVelo (bool useIsJumpingFlag=true) {
		float yVelo = characterController.velocity.y;
		if (useIsJumpingFlag && _isJumping) {
			if (yVelo >= 0 && platformEdgeHandler.IsOnGround()) {
				_isJumping = false;
			}
		} else {
			// Prevent flying up ramp
			// and flying off going down
			RaycastHit hitInfo;
			platformEdgeHandler.IsOnGround(out hitInfo);
			yVelo = Mathf.Min(
				yVelo,
				-Mathf.Max(hitInfo.distance / Time.deltaTime, 0)	// Division by time to counter later multiplication by time to force down distance
			);
		}

		yVelo -= gravityConst * Time.deltaTime;
		if (_reqJump) {
			yVelo = jumpHeight;
			_isJumping = true;
			_reqJump = false;
		}

		return yVelo;
	}

	private Vector3 CalcCameraLookDir (Vector2 normalizedInputVec) {
		return GetFlattenedForwardCamera() * normalizedInputVec.y
				+ GetFlattenedRightCamera() * normalizedInputVec.x;
	}

	private Vector3 _lookVector = Vector3.zero;
	public void UpdateLookDirection (Vector3 lookDirVec) {
		if (lookDirVec.x != 0 || lookDirVec.z != 0) {
			_lookVector = lookDirVec;
		}
	}

	public Vector3 GetLookDirection () {
		return _lookVector;
	}

	void InvokeTargetMode () {
		targetingMode = true;
	}
	void UndoTargetMode () {
		targetingMode = false;
	}
}
