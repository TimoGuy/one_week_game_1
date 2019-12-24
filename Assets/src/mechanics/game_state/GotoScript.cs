using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScript : MonoBehaviour {
	public string gotoScene = "Insert Scene Name here";
	public string collideWithTag = "Untagged";

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == collideWithTag) {
			SceneManager.LoadScene(gotoScene);
		}
	}
}
