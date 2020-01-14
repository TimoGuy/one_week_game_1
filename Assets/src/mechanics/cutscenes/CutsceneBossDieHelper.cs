using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneBossDieHelper : MonoBehaviour {
	public GameObject playerContainer;
	public GameObject bossContainer;

	void Start () {
		playerContainer.SetActive(false);
		bossContainer.SetActive(false);
	}

	void BossDieEvent () {
		SceneManager.LoadScene("scene_defeated_boss");
	}
}
