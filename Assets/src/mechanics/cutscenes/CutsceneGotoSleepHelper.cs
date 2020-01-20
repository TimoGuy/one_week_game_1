using UnityEngine;
using System.Collections;

public class CutsceneGotoSleepHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public UnityStandardAssets.ImageEffects.BloomOptimized mainCamBloom;
	public Animator myCharPuppetAnimator;
	public GameObject myContainer;
	public TextboxHandler handlerToWatch;
	public GSFirstArea gameStateManager;
	public GameObject hideThisCrawler;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerGameObj == null) {
			Debug.LogError("playerGameObj must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (mainCamBloom == null) {
			Debug.LogError("mainCamBloom must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (myCharPuppetAnimator == null) {
			Debug.LogError("myCharPuppetAnimator must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (myContainer == null) {
			Debug.LogError("myContainer must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (handlerToWatch == null) {
			Debug.LogError("handlerToWatch must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (gameStateManager == null) {
			Debug.LogError("gameStateManager must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (hideThisCrawler == null) {
			Debug.LogError("hideThisCrawler must not be null");
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
			// Turned off!
			watchForTxtbxEnd = false;
			myAnimator.SetTrigger("Do_Wakeup");
		}
	}

	void OnEnable () {
		playerGameObj.SetActive(false);
		hideThisCrawler.SetActive(false);
	}

	void OnDisable () {
		playerGameObj.SetActive(true);
		hideThisCrawler.SetActive(true);
	}

	public void TriggerPlayerLiedown () {
		myCharPuppetAnimator.SetTrigger("Lie_Down");
	}

	public void TriggerPlayerStandup () {
		myCharPuppetAnimator.SetTrigger("Stand_Up");
	}

	public void StartWatchingForTextboxEnd () {
		watchForTxtbxEnd = true;
	}

	public void ShutDownCutscene () {
		mainCamBloom.enabled = true;
		GameObject.Destroy(myContainer);
	}

	public void MoveToGameState (int index) {
		gameStateManager.gameStateIndex = index;
	}
}
