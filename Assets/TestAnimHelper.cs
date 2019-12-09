using UnityEngine;
using System.Collections;

public class TestAnimHelper : MonoBehaviour {
	void CalledByAnimator_Rotate45 () {
		var ang = transform.rotation.eulerAngles;
		ang.y += 45;
		transform.rotation = Quaternion.Euler(ang);
	}
}
