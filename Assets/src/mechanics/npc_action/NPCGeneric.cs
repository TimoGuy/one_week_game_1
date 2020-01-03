using UnityEngine;
using System.Collections;

public class NPCGeneric : MonoBehaviour {
	public enum EnableOrDisable { ENABLE, DISABLE };

	public EnableOrDisable onNPCAction;
	public GameObject gameObjectToEnable;

	void NPC_Action () {
#if UNITY_EDITOR
		if (gameObjectToEnable == null) {
			Debug.LogError("gameObjectToEnable must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		if (onNPCAction == EnableOrDisable.ENABLE) {
			gameObjectToEnable.SetActive(true);
		} else {
			gameObjectToEnable.SetActive(false);
		}
	}
}
