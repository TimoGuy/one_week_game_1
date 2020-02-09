using UnityEngine;
using System.Collections;

public class MainMenuEventHandler : MonoBehaviour {
	public MenuController menuController;
	public AudioSource mainMenuBgmAS;
	private float fadeOutInterval = 0;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (menuController == null) {
			Debug.LogError("menuController must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (mainMenuBgmAS == null) {
			Debug.LogError("mainMenuBgmAS must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		myAnimator = GetComponent<Animator>();
	}

	void FixedUpdate () {
		if (mainMenuBgmAS.enabled) {
			mainMenuBgmAS.volume -= fadeOutInterval * Time.deltaTime;
			if (mainMenuBgmAS.volume <= 0) {
				mainMenuBgmAS.Stop();
				mainMenuBgmAS.enabled = false;
			}
		}
	}

	public void DoFadeOut () {
		myAnimator.SetTrigger("Fade_Out");
		fadeOutInterval = 0.75f;
	}

	public void ReportFadeOutFinished () {
		menuController.ExecuteOnFadeOutFinish();
	}
}
