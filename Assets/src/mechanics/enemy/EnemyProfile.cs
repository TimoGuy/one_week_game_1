using UnityEngine;
using System.Collections;

public class EnemyProfile : MonoBehaviour {
	public int attackPoints;
	public Transform parentEnemyWrapper;
	private Vector3 _ietPosCopy = Vector3.zero;
	public string triggerCodeFunction = "OnEnemyDie";
	public MonoBehaviour triggerCode;
	private Transform player;

	// Use this for initialization
	void Start () {
		FindPlayer();
		SaveInnerTransform();
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

	private void SaveInnerTransform () {
		_ietPosCopy = transform.localPosition;
	}

	public void MoveToInnerTransAsOffset () {
		parentEnemyWrapper.position +=
			parentEnemyWrapper.rotation
			* (transform.localPosition - _ietPosCopy);

		// Reset position of inner
		transform.localPosition = _ietPosCopy;
	}
}
