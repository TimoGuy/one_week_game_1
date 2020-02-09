using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GSFirstArea : MonoBehaviour {
	public int gameStateIndex = 0;
	public GameObject[] gameState0;
	public GameObject[] gameState1;
	public GameObject[] gameState2;
	public GameObject[] gameState3;

	private int _currentGSInd = -1;
	private List<GameObject[]> _gameStateHolder;

	void Start () {
		_gameStateHolder = new List<GameObject[]>();
		_gameStateHolder.Add(gameState0);
		_gameStateHolder.Add(gameState1);
		_gameStateHolder.Add(gameState2);
		_gameStateHolder.Add(gameState3);
	}

	void FixedUpdate () {
		if (_currentGSInd != gameStateIndex) {
			SoloEnableGameState(gameStateIndex);
		}
	}

	void SetGameState (int index) {
		this.SoloEnableGameState(index);
	}

	private void SoloEnableGameState (int index) {
		_currentGSInd = gameStateIndex;
		if (index < 0 || index >= _gameStateHolder.Count) {
#if UNITY_EDITOR
			Debug.LogError("gameState index must be in range [0," + _gameStateHolder.Count + ") but was: " + index);
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			return;
		}

		for (int i = 0; i < _gameStateHolder.Count; i++) {
			if (i == index) continue;
			foreach (GameObject obj in _gameStateHolder[i]) {
				if (obj == null) continue;
				if (_gameStateHolder[index].Contains(obj)) continue;
				obj.SetActive(false);
			}
		}

		foreach (GameObject obj in _gameStateHolder[index]) {
			if (obj == null) continue;
			obj.SetActive(true);
		}
	}
}
