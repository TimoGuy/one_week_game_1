using UnityEngine;
using System.Collections;

public class PlayerAnimatorHandler : MonoBehaviour {
	public Animator playerAnimator;

	void Start () {
#if UNITY_EDITOR
		if (playerAnimator == null) {
			Debug.LogError("playerAnimator assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
	}

	void TriggerAttackAnim () {
		playerAnimator.SetTrigger("Attack");
	}

	void TurnOnClimbing () {
		playerAnimator.SetBool("Climbing", true);
	}

	void TurnOffClimbing () {
		playerAnimator.SetBool("Climbing", false);
	}

	void SetRunningSpeed (float speed) {
		playerAnimator.SetFloat("Blend_Running", speed);
	}

	void TurnOnMidair () {
		playerAnimator.SetBool("Midair", true);
	}

	void TurnOffMidair () {
		playerAnimator.SetBool("Midair", false);
	}

	void SetMidairSpeed (float midair) {
		playerAnimator.SetFloat("Blend_Midair", midair);
	}

	void TriggerHangAnim () {
		playerAnimator.SetTrigger("Hang Anim");
	}

	void TriggerGetUpAnim () {
		playerAnimator.SetTrigger("Get Up Anim");
	}

	void SetClimbBlendCoords (object[] coords) {
		float x = Mathf.Abs((float)coords[0]);
		float y = Mathf.Abs((float)coords[1]);
		playerAnimator.SetFloat("Blend_Climb_X", x);
		playerAnimator.SetFloat("Blend_Climb_Y", y);
	}

	void SetIsHurtAnim (bool flag) {
		playerAnimator.SetBool("IsHurt", flag);
	}
}
