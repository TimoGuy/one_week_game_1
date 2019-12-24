using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public void StartGame () {
		SceneManager.LoadScene("scene_fight_arena");
	}
	
	public void HowToPlay () {
		
	}
	
	public void Credits () {
		
	}

	public void ExitApplication () {
		Application.Quit();
	}
}
