using UnityEngine;
using System.Collections;

public class BreakFromAttack : MonoBehaviour {
	public GameObject enableThisOnDestroy;
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player Weapon") {
			if (enableThisOnDestroy != null) {
				enableThisOnDestroy.SetActive(true);
			}
			Destroy(gameObject);
		}
	}
}
