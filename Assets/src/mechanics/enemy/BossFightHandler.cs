using UnityEngine;
using System.Collections;

public class BossFightHandler : MonoBehaviour {
	public GameObject phase2;
	public BossFightBubbleScores bubbleScores;
	public Transform progLazerContainer;
	public Transform playerCollision;
	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}

	void CheckIfTurnOnPhase2 () {
		animator.SetBool("Phase2", true);
		phase2.SetActive(true);
	}

	void SendStompToBubble () {
		bubbleScores.SendMessage("StompedReceiveShock");
	}

	void Update () {
		RandomChooseToDoLazerOrStomp();
	}

	void RandomChooseToDoLazerOrStomp () {
		if (!animator.GetBool("Override_DoLazerBeam")) {
			animator.SetBool("DoLazerBeam", Random.value > 0.5f);
		}
	}

	void CheckLazerTurnLeftOrRight () {
		float playerAngFromOrigin = Mathf.Atan2(playerCollision.position.z, playerCollision.position.x) * Mathf.Rad2Deg;
		float diff = Mathf.DeltaAngle(
			-playerAngFromOrigin, progLazerContainer.rotation.eulerAngles.y
		);
		Debug.Log(diff);
		if (diff < 0) {
			animator.SetFloat("LazerSpinRight", 0);
			var ang = progLazerContainer.rotation.eulerAngles;
			ang.y += 45;
			progLazerContainer.rotation = Quaternion.Euler(ang);
		} else {
			animator.SetFloat("LazerSpinRight", 1);
			var ang = progLazerContainer.rotation.eulerAngles;
			ang.y -= 45;
			progLazerContainer.rotation = Quaternion.Euler(ang);
		}
	}

	// Received Message
	void BreakCircle () {
		animator.SetTrigger("CircleBroken");
	}
}
