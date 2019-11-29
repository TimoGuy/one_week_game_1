using UnityEngine;
using System.Collections;

public class LockonTargeter : MonoBehaviour {
	public Transform lockingTarget;
	private bool uniqueInput;

	// Use this for initialization
	void Start () {
		uniqueInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (uniqueInput) {
			if (Input.GetButtonDown("Jump")) {
				RemoveLockOntoObject();
			}
		} else {
			uniqueInput = !Input.GetButton("Jump");
		}
	}

	public void LockOntoObjectRequest (ObjectLockonBehavior olb) {
		if (lockingTarget != null || !uniqueInput)
			return;

		uniqueInput = false;
		lockingTarget = olb.transform;
		SendMessage("InvokeTargetMode");
	}

	private void RemoveLockOntoObject () {
		if (lockingTarget == null || !uniqueInput)
			return;

		uniqueInput = false;
		lockingTarget = null;
		SendMessage("UndoTargetMode");
	}
}
