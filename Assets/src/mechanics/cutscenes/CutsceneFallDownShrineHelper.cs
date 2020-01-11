﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneFallDownShrineHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public Animator myCharPuppetAnimator;
	public Animator gateAnimator;
	public GameObject waterGameObject;
	public TextboxHandler handlerToWatch;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerGameObj == null) {
			Debug.LogError("playerGameObj must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (myCharPuppetAnimator == null) {
			Debug.LogError("myCharPuppetAnimator must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (gateAnimator == null) {
			Debug.LogError("gateAnimator must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (waterGameObject == null) {
			Debug.LogError("waterGameObject must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (handlerToWatch == null) {
			Debug.LogError("handlerToWatch must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		myAnimator = GetComponent<Animator>();
	}
	
	private bool watchForTxtbxEnd = false;
	void FixedUpdate () {
		if (!watchForTxtbxEnd) return;

		if (!handlerToWatch.enabled) {
			GotoBossFightScene();
		}
	}

	void OnEnable () {
		playerGameObj.SetActive(false);
	}

	void OnDisable () {
		playerGameObj.SetActive(true);
	}

	public void OpenGate () {
		gateAnimator.SetTrigger("Open_Gate");
	}

	public void GotoBossFightScene () {
		SceneManager.LoadScene("scene_fight_arena");
	}

	public void TriggerPlayerFlail () {
		myCharPuppetAnimator.SetTrigger("Fall_Flailing");
	}

	public void DisableWaterGameObject () {
		waterGameObject.SetActive(false);
	}

	public void StartWatchingForTextboxEnd () {
		watchForTxtbxEnd = true;
	}
}
