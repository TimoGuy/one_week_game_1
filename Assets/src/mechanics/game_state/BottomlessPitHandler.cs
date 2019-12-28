using UnityEngine;
using System.Collections;

public class BottomlessPitHandler : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Bottomless Pit") {
			Camera.main.GetComponent<CameraInput>().enabled = false;
		}
	}
}
