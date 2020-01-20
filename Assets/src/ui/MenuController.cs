using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	void Start () {
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartGame () {
		SceneManager.LoadScene("scene_first_area");
	}
	
	public void HowToPlay () {
		Debug.LogError("HowToPlay() not implemented!");
	}
	
	public void Credits () {
		Debug.LogError("Credits() not implemented!");
	}

	public void ExitApplication () {
		Application.Quit();
	}
}
