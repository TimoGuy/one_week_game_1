﻿using UnityEngine;
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
}