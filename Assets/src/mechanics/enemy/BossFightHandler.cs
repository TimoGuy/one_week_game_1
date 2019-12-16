using UnityEngine;
using System.Collections;

public class BossFightHandler : MonoBehaviour {
	public GameObject phase2;
	public BossFightBubbleScores bubbleScores;
	public Transform progLazerContainer;
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
		if (!animator.GetBool("Override_DoLazerBeam")) {
			animator.SetBool("DoLazerBeam", Random.value > 0.5f);
		}
	}

	void SetupLazerStartDir () {
		Transform player =
			GameObject.FindGameObjectWithTag("Player").transform;
		float playerAngFromOrigin =
			Mathf.Atan2(player.position.z, player.position.x) * Mathf.Rad2Deg;
		progLazerContainer.localRotation = Quaternion.Euler(0, -playerAngFromOrigin + 180, 0);
	}

	// Received Message
	void BreakCircle () {
		animator.SetTrigger("CircleBroken");
	}
}
