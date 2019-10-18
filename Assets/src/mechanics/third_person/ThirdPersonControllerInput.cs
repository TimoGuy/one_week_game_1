using UnityEngine;
using System.Collections;

public class ThirdPersonControllerInput : MonoBehaviour {
	public Transform myCamera;
	public float mvtSpeed = 5;
	public float mvtAccel = 1;
	public float gravityConst = 0.5f;

	private CharacterController characterController;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		gravityBuildup = 0;
		mvtBuildup = 0;
	}
	
	private float mvtBuildup;
	private Vector2 normalizedInputVec;
	void FixedUpdate () {
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

	// Update is called once per frame
	void Update () {
		Vector3 moveVector =
			(myCamera.forward * normalizedInputVec.y + myCamera.right * normalizedInputVec.x).normalized *
			mvtBuildup *
			Time.deltaTime;
		moveVector = new Vector3(moveVector.x, 0, moveVector.z);
		UpdateGravity(ref moveVector);

		characterController.Move(moveVector);
	}

	private float gravityBuildup;
	private void UpdateGravity (ref Vector3 moveVector) {
		if (characterController.isGrounded) {
			gravityBuildup = 0;
		} else {
			gravityBuildup += gravityConst * Time.deltaTime;
		}
		
		moveVector.y -= gravityBuildup;
	}
}
