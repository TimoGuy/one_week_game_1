using UnityEngine;
using System.Collections;

public class TextboxHandler : MonoBehaviour {
	public string[] textMessages;
	private int textMsgInd = 0;
	private float debounce;
	private const float DEBOUNCE_AMT = 0.1f;

	public TextMesh textMesh;
	public GameObject txtbxBgPlane;

	private ThirdPersonControllerInput player;
	
	void Start () {
#if UNITY_EDITOR
		if (textMesh == null) {
			Debug.LogError("textMesh assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (txtbxBgPlane == null) {
			Debug.LogError("txtbxBgPlane assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
		player =
			GameObject.FindGameObjectWithTag("Player")
			.GetComponent<ThirdPersonControllerInput>();
	}

	void OnEnable () {
		LoadPlayerObj();
		player.enabled = false;
		textMesh.gameObject.SetActive(true);
		txtbxBgPlane.SetActive(true);
		textMsgInd = 0;
	}

	void OnDisable () {
		LoadPlayerObj();
		player.enabled = true;
		textMesh.gameObject.SetActive(false);
		txtbxBgPlane.SetActive(false);
	}

	void Update () {
		if (textMessages == null ||
			textMsgInd >= textMessages.Length) {
			this.enabled = false;
			return;
		}

		debounce -= Time.deltaTime;
		textMesh.text = textMessages[textMsgInd];

		if (Input.GetButtonDown("Action") && debounce <= 0) {
			textMsgInd++;
			debounce = DEBOUNCE_AMT;
		}
	}

	public void LoadTextboxes (string[] messages) {
		this.textMessages = messages;
		this.enabled = true;
		debounce = DEBOUNCE_AMT;
	}

	private void LoadPlayerObj (bool overrideFlag=false) {
		if (player == null || overrideFlag) {
			player =
				GameObject.FindGameObjectWithTag("Player")
				.GetComponent<ThirdPersonControllerInput>();
		}
	}
}
