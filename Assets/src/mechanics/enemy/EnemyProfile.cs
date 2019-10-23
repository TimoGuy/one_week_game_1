using UnityEngine;
using System.Collections;

public class EnemyProfile : MonoBehaviour {
	public int attackPoints;
	public string triggerCodeFunction;
	public MonoBehaviour triggerCode;
	private Transform player;

	// Use this for initialization
	void Start () {
		FindPlayer();
	}

	public void OnDie () {
		if (triggerCode != null &&
			triggerCodeFunction.Trim().Length > 0) {
			triggerCode.SendMessage(triggerCodeFunction.Trim());
		} 
	}

	public Transform GetPlayerTransform () {
		return player;
	}

	private void FindPlayer () {
		player = GameObject.FindWithTag("Player").transform;
	}
}
