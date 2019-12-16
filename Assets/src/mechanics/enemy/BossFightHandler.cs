using UnityEngine;
using System.Collections;

public class BossFightHandler : MonoBehaviour {
	public GameObject phase2;
	public BossFightBubbleScores bubbleScores;
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

	void RandomChooseToDoLazerOrStomp () {
		animator.SetBool("DoLazerBeam", Random.value > 0.5f);
	}

	// Received Message
	void BreakCircle () {
		animator.SetTrigger("CircleBroken");
	}
}
