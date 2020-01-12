using UnityEngine;
using System.Collections;

public class CutsceneWakeUpHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public GameObject myContainer;
	public TextboxHandler handlerToWatch;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerGameObj == null) {
			Debug.LogError("playerGameObj must not be null");
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
#endif
		myAnimator = GetComponent<Animator>();
	}

	private bool watchForTxtbxEnd = false;
	void FixedUpdate () {
		if (!watchForTxtbxEnd) return;

		if (!handlerToWatch.enabled) {
			// Turned off!
			watchForTxtbxEnd = false;
			myAnimator.SetTrigger("Do_Outro");
		}
	}

	void OnEnable () {
		playerGameObj.SetActive(false);
	}

	public void ReenablePlayer () {
		playerGameObj.SetActive(true);
	}

	public void LockCursor () {
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void StartWatchingForTextboxEnd () {
		watchForTxtbxEnd = true;
	}

	public void ShutDownCutscene () {
		GameObject.Destroy(myContainer);
	}
}
