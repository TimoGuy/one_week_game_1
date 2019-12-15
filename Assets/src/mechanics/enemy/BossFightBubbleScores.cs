using UnityEngine;
using System.Collections;

public class BossFightBubbleScores : MonoBehaviour {
	public int numTimesStompBeforeCrack = 5;
	private int numTimesStomped = 0;
	private MeshRenderer meshRenderer;
	private SphereCollider myCollider;
	private bool amIVertical;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		myCollider = GetComponent<SphereCollider>();
		myCollider.enabled = meshRenderer.enabled = false;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player Weapon") {
			bool isVert = other.GetComponentInParent<WeaponSwordAttack>().IsVerticalSlash();
			if (isVert == amIVertical) {
				myCollider.enabled = meshRenderer.enabled = false;
				numTimesStomped = 0;
				SendMessageUpwards("BreakCircle", SendMessageOptions.RequireReceiver);
			}
		}
	}

	void StompedReceiveShock () {
		numTimesStomped++;
		if (numTimesStomped >= numTimesStompBeforeCrack &&
			!meshRenderer.enabled) {
			// Show cracks
			myCollider.enabled = meshRenderer.enabled = true;
			RandomHorOrVert();
		}
	}

	private void RandomHorOrVert () {
		amIVertical = Random.value > 0.5f;
		Debug.Log("Haha Iam vert: " + amIVertical);
		if (amIVertical) {
			transform.rotation = Quaternion.Euler(90, 0, 0);
		} else {
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}
}
