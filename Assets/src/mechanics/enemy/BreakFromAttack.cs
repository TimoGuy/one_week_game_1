using UnityEngine;
using System.Collections;

public class BreakFromAttack : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player Weapon") {
			Destroy(gameObject);
		}
	}
}
