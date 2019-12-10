using UnityEngine;
using System.Collections;

public class AttackReceiver : MonoBehaviour {
	public string ReceivingTag = "Untagged";

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == ReceivingTag) {
			Debug.Log("Attacked!!!");
			GetComponent<AudioSource>().Play(0);
		}
	}
}
