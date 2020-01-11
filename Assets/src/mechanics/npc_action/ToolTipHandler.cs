using UnityEngine;
using System.Collections;

public class ToolTipHandler : MonoBehaviour {
	public TextMesh toolTipText;

	void Start () {
#if UNITY_EDITOR
		if (toolTipText == null) {
			Debug.LogError("toolTipText must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
	}

	public void DisplayToolTip (string text) {
		toolTipText.text = text;
		this.enabled = true;
		toolTipText.gameObject.SetActive(true);
	}
}
