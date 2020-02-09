using UnityEngine;
using System.Collections;

public class MainMenuEventHandler : MonoBehaviour {
	public MenuController menuController;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (menuController == null) {
			Debug.LogError("menuController must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		myAnimator = GetComponent<Animator>();
	}

	public void DoFadeOut () {
		myAnimator.SetTrigger("Fade_Out");
	}

	public void ReportFadeOutFinished () {
		menuController.ExecuteOnFadeOutFinish();
	}
}
