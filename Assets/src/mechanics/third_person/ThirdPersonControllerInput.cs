using UnityEngine;
using System.Collections;

public class ThirdPersonControllerInput : MonoBehaviour {
	public Transform myCamera;
	public float mvtSpeed = 5;
	public float mvtAccel = 1;
	public float gravityConst = 0.5f;
	public float jumpHeight = 10;

	private CharacterController characterController;
	private PlatformEdgeHandler platformEdgeHandler;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		platformEdgeHandler = GetComponent<PlatformEdgeHandler>();
		mvtBuildup = 0;
	}
	
	private float mvtBuildup;
	private Vector2 normalizedInputVec;
	void FixedUpdate () {
		if (!platformEdgeHandler.IsOnGround()) return;
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector2 myNormInputVec = new Vector2(inputX, inputY).normalized;
		
		if (Mathf.Abs(myNormInputVec.x) + Mathf.Abs(myNormInputVec.y) > 0) {
			mvtBuildup += mvtAccel;
			normalizedInputVec = myNormInputVec;
		} else {
			mvtBuildup -= mvtAccel;
		}
		mvtBuildup = Mathf.Clamp(mvtBuildup, 0, mvtSpeed);
	}

	private Vector3 moveVector;
	// Update is called once per frame
	void Update () {
		moveVector =
			(GetFlattenedForwardCamera() * normalizedInputVec.y
				+ myCamera.right * normalizedInputVec.x)
				.normalized
			* mvtBuildup;
		moveVector = new Vector3(moveVector.x, FetchYVelo(), moveVector.z);

		characterController.Move(moveVector * Time.deltaTime);
	}

	bool _reqJump = false;
	public void RequestJump () {
		_reqJump = true;
	}

	private Vector3 GetFlattenedForwardCamera () {
		return new Vector3(myCamera.forward.x, 0, myCamera.forward.z).normalized;
	}

	private bool _isJumping = false;
	private float FetchYVelo () {
		float yVelo = characterController.velocity.y;
		if (_isJumping) {
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
}
