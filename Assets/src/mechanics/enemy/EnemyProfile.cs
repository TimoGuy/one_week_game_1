using UnityEngine;
using System.Collections;

public class EnemyProfile : MonoBehaviour {
	public int attackPoints;
	public Transform innerEnemyTrans;
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
		_ietPosCopy = innerEnemyTrans.localPosition;
		// _ietPosCopy.rotation = innerEnemyTrans.rotation;
		// _ietPosCopy.localScale = innerEnemyTrans.localScale;
	}

	public void MoveToInnerTransAsOffset () {
		Debug.Log("Hey my duuude?!");
		Debug.Log(innerEnemyTrans.position - _ietPosCopy);
		transform.localPosition +=
			innerEnemyTrans.localPosition - _ietPosCopy;
		// transform.rotation +=
		// 	innerEnemyTrans.rotation - _ietPosCopy.rotation;
		// transform.localScale +=
		// 	innerEnemyTrans.localScale - _ietPosCopy.localScale;

		// Reset position of inner
		innerEnemyTrans.localPosition = _ietPosCopy;
	}
}
