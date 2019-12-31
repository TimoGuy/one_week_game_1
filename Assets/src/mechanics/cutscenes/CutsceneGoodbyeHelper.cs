using UnityEngine;
using System.Collections;

public class CutsceneGoodbyeHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public GameObject npcToDelete;
	public GameObject myContainer;
	public Animator npcFriendAnimator;
	public TextboxHandler handlerToWatch;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerGameObj == null) {
			Debug.LogError("playerGameObj must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (npcToDelete == null) {
			Debug.LogError("npcToDelete must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (myContainer == null) {
			Debug.LogError("myContainer must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (npcFriendAnimator == null) {
			Debug.LogError("npcFriendAnimator must not be null");
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

	void OnDisable () {
		playerGameObj.SetActive(true);
	}
	
	public void TriggerDoTurnAround () {
		npcFriendAnimator.SetTrigger("Do_TurnAround");
	}

	public void StartWatchingForTextboxEnd () {
		watchForTxtbxEnd = true;
	}

	public void DeleteNPCPuppet () {
		GameObject.Destroy(npcToDelete);
	}

	public void ShutDownCutscene () {
		GameObject.Destroy(myContainer);
	}
}
