using UnityEngine;
using System.Collections;

public class BottomlessPitHandler : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Bottomless Pit") {
			SetEnabledCameraInput(false);
		}
	}

	public static void SetEnabledCameraInput (bool flag) {
		Camera.main.GetComponent<CameraInput>().enabled = flag;
	}
}
