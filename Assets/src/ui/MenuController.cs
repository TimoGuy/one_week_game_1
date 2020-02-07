using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public GameObject howToPlayText;
	public GameObject creditsText;

	void Start () {
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartGame () {
		SceneManager.LoadScene("scene_first_area");
	}
	
	public void HowToPlay () {
		howToPlayText.SetActive(true);
		creditsText.SetActive(false);
	}
	
	public void Credits () {
		creditsText.SetActive(true);
		howToPlayText.SetActive(false);
	}

	public void ExitApplication () {
		Application.Quit();
	}
}
