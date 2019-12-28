using UnityEngine;
using System.Collections;

public class NPCActionHandler : MonoBehaviour {
	private GameObject npcInContact;

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
		}
	}
}
