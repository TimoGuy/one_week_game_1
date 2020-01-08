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

	private float debounce = 0;
	void Update () {
		if (debounce <= 0 ||
			Input.anyKeyDown) {
			toolTipText.gameObject.SetActive(false);
			this.enabled = false;
			return;
		}

		debounce -= Time.deltaTime;
	}

	public void DisplayToolTip (string text) {
		toolTipText.text = text;
		this.enabled = true;
		toolTipText.gameObject.SetActive(true);
		debounce = 2;
	}
}
