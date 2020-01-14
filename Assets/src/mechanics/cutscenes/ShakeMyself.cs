using UnityEngine;
using System.Collections;

public class ShakeMyself : MonoBehaviour {
	public Vector3 randomMultiplier;
	private Vector3 startPosition;

	void Start () {
		startPosition = transform.position;
	}

	void FixedUpdate () {
		Vector3 random = Random.insideUnitSphere;
		transform.position = startPosition + new Vector3(
			random.x * randomMultiplier.x,
			random.y * randomMultiplier.y,
			random.z * randomMultiplier.z
		);
	}
}
