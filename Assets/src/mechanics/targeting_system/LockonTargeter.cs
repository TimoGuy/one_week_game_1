using UnityEngine;
using System.Collections;

public class LockonTargeter : MonoBehaviour {
	public Transform lockingTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			RemoveLockOntoObject();
		}
	}

	public void LockOntoObjectRequest (ObjectLockonBehavior olb) {
		if (lockingTarget != null)
			return;

		lockingTarget = olb.transform;
		SendMessage("InvokeTargetMode", lockingTarget);
	}

	private void RemoveLockOntoObject () {
		if (lockingTarget == null)
			return;

		lockingTarget = null;
		SendMessage("UndoTargetMode");
	}
}
