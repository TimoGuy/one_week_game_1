using UnityEngine;
using System.Collections;

public class CutsceneGotoSleepHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public Animator myCharPuppetAnimator;
	public GSFirstArea gameStateManager;

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
		if (gameStateManager == null) {
			Debug.LogError("gameStateManager must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
	}

	void OnEnable () {
		playerGameObj.SetActive(false);
	}

	void OnDisable () {
		playerGameObj.SetActive(true);
	}

	public void TriggerPlayerLiedown () {
		myCharPuppetAnimator.SetTrigger("Lie_Down");
	}

	public void MoveToGameState (int index) {
		gameStateManager.gameStateIndex = index;
	}
}
