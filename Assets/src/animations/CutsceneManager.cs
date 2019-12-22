using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour {
	// void Start () {
	// 	GetComponents<Camera>(cameras);
	// 	Camera myCam = GetComponent<Camera>();
	// 	cameras.Remove(myCam);
	// }
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

	}
}
