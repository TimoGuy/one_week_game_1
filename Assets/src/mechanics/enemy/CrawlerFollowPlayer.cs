using UnityEngine;
using System.Collections;

public class CrawlerFollowPlayer : MonoBehaviour {
	public float recognizeIsNearThreshold = 15;
	public float verticalNearThreshold = 2;
	public float followSpeed = 5;
	public float acceleration = 0.1f;
	public float turnSpeed = 10;
	public float deadZoneTurn = 5;
	public float gravity = 1.5f;
	public ThirdPersonControllerInput player;
	public Animator myAnimator;
	public TextboxHandler textboxHandlerForKnowWhenToDisable;
	private CharacterController myCC;
	private float currentSpeed = 0;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if (player == null) {
			Debug.LogError("player assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (myAnimator == null) {
			Debug.LogError("myAnimator assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (textboxHandlerForKnowWhenToDisable == null) {
			Debug.LogError("textboxHandlerForKnowWhenToDisable assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
		myCC = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (textboxHandlerForKnowWhenToDisable.enabled) return;

		Vector3 lookDirection = player.transform.position - transform.position;
		float verticalDiff = Mathf.Abs(lookDirection.y);
		lookDirection.y = 0;

		if (verticalDiff < verticalNearThreshold &&
			Vector3.Distance(lookDirection, Vector3.zero) < recognizeIsNearThreshold &&
			!Physics.Linecast(transform.position, player.transform.position, 1 << LayerMask.NameToLayer("Ground"))) {
			float direction =
				90 - Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg;

			float deltaDir = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, direction);
			if (Mathf.Abs(deltaDir) >= deadZoneTurn) {
				transform.rotation =
					Quaternion.Euler(new Vector3(
						0,
						transform.rotation.eulerAngles.y + Mathf.Clamp(deltaDir, -turnSpeed, turnSpeed)
					)
				);
			} else {
				transform.rotation =
					Quaternion.Euler(new Vector3(
						0,
						direction
					)
				);
			}

			// Move to/from player
			if (player.IsHurt()) {
				// Move from
				currentSpeed = Mathf.Lerp(currentSpeed, -followSpeed, acceleration);
			} else {
				// Move to
				currentSpeed = Mathf.Lerp(currentSpeed, followSpeed, acceleration);
			}
		} else {
			// Stop
			currentSpeed = Mathf.Lerp(currentSpeed, 0, acceleration);
		}
		if (myAnimator == null) {
			this.enabled = false;
			myCC.enabled = false;
			return;
		}
		myAnimator.SetFloat("Blend_Walk", Mathf.Abs(currentSpeed / followSpeed));
		Vector3 moveVec = -(transform.rotation * Vector3.forward) * currentSpeed;
		moveVec.y = myCC.velocity.y - gravity;
		myCC.Move(moveVec * Time.deltaTime);
	}

	private static float ToRegularAngleBounds (float angle) {
		while (angle < 0) angle += 360;
		while (angle >= 360) angle -= 360;
		return angle;
	}
}
