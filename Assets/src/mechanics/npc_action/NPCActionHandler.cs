using UnityEngine;
using System.Collections;

public class NPCActionHandler : MonoBehaviour {
	public ToolTipHandler tooltipHandler;
	private GameObject npcInContact;

	void Start () {
#if UNITY_EDITOR
		if (tooltipHandler == null) {
			Debug.LogError("tooltipHandler must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
	}

	void Update () {
		if (npcInContact != null &&
			Input.GetButtonDown("Action")) {
			npcInContact.SendMessage("NPC_Action", SendMessageOptions.RequireReceiver);
			npcInContact = null;	// Require enter into trigger again
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "NPC Action") {
			npcInContact = other.gameObject;
			tooltipHandler.DisplayToolTip(
				other.gameObject.GetComponent<ToolTip>().GetText()
			);
		}
	}
}
