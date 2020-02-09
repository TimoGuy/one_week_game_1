using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneBossDieHelper : MonoBehaviour {
	public GameObject playerContainer;
	public GameObject bossContainer;
	public TextboxHandler handlerToWatch;
	public AudioSource darkWorldBgmAS;
	private float fadeOutInterval = 0;

	void Start () {
#if UNITY_EDITOR
		if (playerContainer == null) {
			Debug.LogError("playerContainer must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (bossContainer == null) {
			Debug.LogError("bossContainer must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (handlerToWatch == null) {
			Debug.LogError("handlerToWatch must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (darkWorldBgmAS == null) {
			Debug.LogError("darkWorldBgmAS must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif

		playerContainer.SetActive(false);
		bossContainer.SetActive(false);
	}

	private bool watchForTxtbxEnd = false;
	void FixedUpdate () {
		if (darkWorldBgmAS.enabled) {
			darkWorldBgmAS.volume -= fadeOutInterval * Time.deltaTime;
			if (darkWorldBgmAS.volume <= 0) {
				darkWorldBgmAS.Stop();
				darkWorldBgmAS.enabled = false;
			}
		}

		if (!watchForTxtbxEnd) return;

		if (!handlerToWatch.enabled) {
			BossDieEvent();
		}
	}

	public void StartWatchingForTextboxEnd () {
		watchForTxtbxEnd = true;
	}

	void BossDieEvent () {
		SceneManager.LoadScene("scene_defeated_boss");
	}

	public void FadeOutMusic (float amount) {
		fadeOutInterval = Mathf.Abs(amount);
	}
}
