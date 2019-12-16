using UnityEngine;
using System.Collections;

public class TestAnimHelper : MonoBehaviour {
	public TestAnimHelper child;

	void CalledByAnimator_Rotate45 () {
		var ang = transform.rotation.eulerAngles;
		ang.y += 45;
		transform.rotation = Quaternion.Euler(ang);
	}

	void CalledByAnimator_Rotate45Child () {
		child.SendMessage("CalledByAnimator_Rotate45", SendMessageOptions.RequireReceiver);
	}

	void CalledByAnimator_Rotate45Oppo () {
		var ang = transform.rotation.eulerAngles;
		ang.y -= 45;
		transform.rotation = Quaternion.Euler(ang);
	}

	void CalledByAnimator_Rotate45OppoChild () {
		child.SendMessage("CalledByAnimator_Rotate45Oppo", SendMessageOptions.RequireReceiver);
	}
}
