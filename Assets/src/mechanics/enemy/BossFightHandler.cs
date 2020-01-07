using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class BossFightHandler : MonoBehaviour {
	public GameObject phase2;
	public BossFightBubbleScores bubbleScores;
	public Transform progLazerContainer;
	public int phase2StartLife = 5;
	private Animator animator;

	void Awake () {
		animator = GetComponent<Animator>();
	}

	void CheckIfTurnOnPhase2 () {
		if (animator.GetBool("Phase2")) {
			phase2.SetActive(true);
		} else {
			phase2.SetActive(false);
		}
	}

	void SendStompToBubble () {
		bubbleScores.SendMessage("StompedReceiveShock");
	}

	void RandomChooseToDoLazerOrStomp () {
		if (!animator.GetBool("Override_DoLazerBeam")) {
			animator.SetBool("DoLazerBeam", Random.value > 0.5f);
		}

		SendMessage(
			"Spider_DoLazer",
			animator.GetBool("DoLazerBeam")
		);
	}

	void SetupLazerStartDir () {
		Transform player =
			GameObject.FindGameObjectWithTag("Player").transform;
		float playerAngFromOrigin =
			Mathf.Atan2(player.position.z, player.position.x) * Mathf.Rad2Deg;
		progLazerContainer.localRotation = Quaternion.Euler(0, -playerAngFromOrigin + 180, 0);
	}

	void BossDieEvent () {
		SceneManager.LoadScene("scene_defeated_boss");
	}

	// Received Message
	void BreakCircle () {
		animator.SetTrigger("CircleBroken");
	}

	public void SetLifeInAnimator (int life) {
		animator.SetInteger("Life", life);
		if (life <= phase2StartLife) {
			animator.SetBool("Phase2", true);
		} else {
			animator.SetBool("Phase2", false);
		}
	}
}
