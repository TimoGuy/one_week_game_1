using UnityEngine;
using System.Collections;

public class BossFightBubbleScores : MonoBehaviour {
	public int numTimesStompBeforeCrack = 5;
	private int numTimesStomped;
	private MeshRenderer meshRenderer;
	private SphereCollider myCollider;
	private bool amIVertical;

	// Use this for initialization
	void OnEnable () {
		meshRenderer = GetComponent<MeshRenderer>();
		myCollider = GetComponent<SphereCollider>();
		myCollider.enabled = meshRenderer.enabled = false;
		numTimesStomped = 0;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player Weapon") {
			myCollider.enabled = meshRenderer.enabled = false;
			numTimesStomped = 0;
			SendMessageUpwards("BreakCircle", SendMessageOptions.RequireReceiver);
		}
	}

	void StompedReceiveShock () {
		numTimesStomped++;
		if (numTimesStomped >= numTimesStompBeforeCrack &&
			!meshRenderer.enabled) {
			// Show cracks
			myCollider.enabled = meshRenderer.enabled = true;
		}
	}
}
