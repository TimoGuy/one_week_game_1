using UnityEngine;
using System.Collections;

public class ThrowRagdoll : MonoBehaviour {
	public Rigidbody objRigidbody;
	public Vector3 forceDir;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if (objRigidbody == null) {
			Debug.LogError("objRigidbody must not be null");
			UnityEditor.EditorApplication.isPlaying = false;
			return;
		}
#endif
	}

	void OnEnable () {
		objRigidbody.AddForce(transform.rotation * forceDir, ForceMode.Impulse);
	}
}
