using UnityEngine;
using System.Collections;

public class TestAnimHelper : MonoBehaviour {
	public Transform reverseRotation;
	
	void CalledByAnimator_Rotate45 () {
		RotateYByAmount(transform, 45);
		
		if (reverseRotation != null)
			RotateYByAmount(reverseRotation, -45);
	}

	private void RotateYByAmount (Transform myTransform, float degrees) {
		var ang = myTransform.rotation.eulerAngles;
		ang.y += degrees;
		myTransform.rotation = Quaternion.Euler(ang);
	}
}
