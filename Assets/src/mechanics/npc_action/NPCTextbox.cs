using UnityEngine;
using System.Collections;

public class NPCTextbox : MonoBehaviour {
	public TextboxHandler textboxHandler;
	public string[] textMessages;

	void NPC_Action () {
#if UNITY_EDITOR
		if (textboxHandler == null) {
			Debug.LogError("textboxHandler must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		textboxHandler.LoadTextboxes(textMessages);
	}
}
