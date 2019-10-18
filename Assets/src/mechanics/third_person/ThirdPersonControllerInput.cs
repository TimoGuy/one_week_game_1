﻿using UnityEngine;
using System.Collections;

public class ThirdPersonControllerInput : MonoBehaviour {
	public Transform myCamera;
	public float mvtSpeed = 5;
	public float mvtAccel = 1;
	public float gravityConst = 0.5f;
	public float jumpHeight = 10;

	private CharacterController characterController;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
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

	private Vector3 moveVector;

	// Update is called once per frame
	void Update () {
		float prevY = moveVector.y;
		moveVector =
			(myCamera.forward * normalizedInputVec.y + myCamera.right * normalizedInputVec.x).normalized
			* mvtBuildup;
		moveVector = new Vector3(moveVector.x, prevY, moveVector.z);
		UpdateGravity();

		characterController.Move(moveVector * Time.deltaTime);
	}

	public void RequestJump () {
		moveVector.y = jumpHeight;
	}

	private void UpdateGravity () {
		// if (characterController.isGrounded) {
		// 	gravityBuildup = 0;
		// }
		
		// gravityBuildup += gravityConst * Time.deltaTime;
		
		moveVector.y -= gravityConst * Time.deltaTime;
	}
}
