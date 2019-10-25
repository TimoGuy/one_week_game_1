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
		Debug.Log("Hey!");
		_ietPosCopy = transform.position;
		// _ietPosCopy.rotation = innerEnemyTrans.rotation;
		// _ietPosCopy.localScale = innerEnemyTrans.localScale;
	}

	public void MoveToInnerTransAsOffset () {
		Debug.Log(transform.position - _ietPosCopy);
		parentEnemyWrapper.position +=
			transform.position - _ietPosCopy;
		// transform.rotation +=
		// 	innerEnemyTrans.rotation - _ietPosCopy.rotation;
		// transform.localScale +=
		// 	innerEnemyTrans.localScale - _ietPosCopy.localScale;

		// Reset position of inner
		transform.position = _ietPosCopy;
	}
}
