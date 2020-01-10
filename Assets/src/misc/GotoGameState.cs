using UnityEngine;
using System.Collections;

public class GotoGameState : MonoBehaviour {
	public GameObject gameStateMnger;
	public int gameStateIndex;

	void Start () {
#if UNITY_EDITOR
		if (gameStateMnger == null) {
			Debug.LogError("gameStateMnger must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
		gameStateMnger.SendMessage("SetGameState", gameStateIndex);
	}
}
