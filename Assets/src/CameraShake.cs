using UnityEngine;
using System.Collections;

// https://gist.github.com/ftvs/5822103
public class CameraShake : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	private Transform camTransform;
	
	// How long the object should shake for.
	public float shakeDuration = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	void Awake () {
		if (camTransform == null) {
			camTransform = Camera.main.transform;
		}
	}

	void LateUpdate () {
		if (shakeDuration > 0) {
			camTransform.SendMessage("Message_ShakeCam", shakeAmount, SendMessageOptions.RequireReceiver);
			shakeDuration -= Time.deltaTime * decreaseFactor;
		} else {
			shakeDuration = 0f;
		}
	}

	void Invoked_ShakeSoft () {
		shakeDuration = 0.25f;
	}

	void Invoked_ShakeHard () {
		shakeDuration = 1;
	}
}