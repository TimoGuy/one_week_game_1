using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public GameObject howToPlayText;
	public GameObject creditsText;
	public MainMenuEventHandler mainMenuEventHandler;
	private string myEvent;

	void Start () {
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartGame () {
		myEvent = "Start_Game";
		mainMenuEventHandler.DoFadeOut();
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
		myEvent = "Quit_Game";
		mainMenuEventHandler.DoFadeOut();
	}

	public void ExecuteOnFadeOutFinish () {
		Camera.main.gameObject.SetActive(false);
		GL.Clear(true, true, Color.black);

		if (myEvent == "Start_Game") {
			SceneManager.LoadScene("scene_first_area");
		} else if (myEvent == "Quit_Game") {
			Application.Quit();
		}
	}
}
