using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour {
	public int textboxIndex = -1;
	private GameObject playerToReenable;
	private Camera[] camsToReenable;

	void OnEnable () {
		camsToReenable = Camera.allCameras;
		playerToReenable = GameObject.FindGameObjectWithTag("Player");

		for (int i = 0; i < camsToReenable.Length; i++) {
			camsToReenable[i].enabled = false;
		}
		playerToReenable.SetActive(false);

		GetComponent<Camera>().enabled = true;
	}

	void OnDisable () {
		for (int i = 0; i < camsToReenable.Length; i++) {
			camsToReenable[i].enabled = true;
		}
		playerToReenable.SetActive(true);

		GetComponent<Camera>().enabled = false;
	}

	void ShowTextbox () {
		if (textboxIndex < 0) {
			Debug.LogError("Textbox id request (" + textboxIndex + ") out of range");
			return;
		}

		// TODO: show textbox here!
	}
}
