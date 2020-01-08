using UnityEngine;
using System.Collections;

public class DoorAnimateHandler : MonoBehaviour {
	public Animator doorAnimator;

	void Start () {
#if UNITY_EDITOR
		if (doorAnimator == null) {
			Debug.LogError("doorAnimator assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (gameObject.tag != "NPC Action") {
			Debug.LogError("Action Area's gameObject.tag must be \"NPC Action\"");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
	}

	void NPC_Action () {
		doorAnimator.SetTrigger("Open_Door");
		Destroy(gameObject);
	}
}
