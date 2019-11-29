using UnityEngine;
using System.Collections;

public class ObjectLockonBehavior : MonoBehaviour {
	public LockonTargeter targeter;		// Usually the player
	public float canTargetDist = 10;
	public bool canTarget;
	public bool targeted;

	// Use this for initialization
	void Start () {
		lockBtnDown = false;
	}
	
	void Update () {
		lockBtnDown = !lockBtnDown ? Input.GetButtonDown("Jump") : lockBtnDown;
	}

	private bool lockBtnDown;
	void FixedUpdate () {
		float dist = Vector3.Distance(
			transform.position,
			targeter.transform.position
		);

		canTarget = dist <= canTargetDist;
		if (canTarget && lockBtnDown) {
			lockBtnDown = false;
			SendToTargeterLockOnRequest();
		}
	}

	private void SendToTargeterLockOnRequest () {
		// targeter.
	}
}
