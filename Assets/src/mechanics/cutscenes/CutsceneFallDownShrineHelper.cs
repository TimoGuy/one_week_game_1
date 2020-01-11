using UnityEngine;
using System.Collections;

public class CutsceneFallDownShrineHelper : MonoBehaviour {
	public GameObject playerGameObj;
	public Animator gateAnimator;
	private Animator myAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerGameObj == null) {
			Debug.LogError("playerGameObj must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
		if (gateAnimator == null) {
			Debug.LogError("gateAnimator must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
