using UnityEngine;
using System.Collections;

public class PlatformEdgeHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		prevIsGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateJump();
	}

	public bool IsOnGround (out RaycastHit hitInfo) {
		Ray ray = new Ray();
		ray.origin = transform.position + Vector3.down;
		ray.direction = Vector3.down;
		Debug.DrawRay(ray.origin, ray.direction, Color.red);
		return Physics.Raycast(
			ray,
			out hitInfo,
			0.5f,
			1 << LayerMask.NameToLayer("Ground")
		);
	}

	public bool IsOnGround () {
		RaycastHit garb;
		return IsOnGround(out garb);
	}

	private bool prevIsGrounded;
	private void UpdateJump () {
		if (!IsOnGround() &&
			prevIsGrounded) {
			SendMessage("RequestJump");
		}

		prevIsGrounded = IsOnGround();
	}
}
