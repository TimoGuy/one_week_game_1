using UnityEngine;
using System.Collections;

public class SFXPlayer_Player : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip acFootstep, acJump, acSlash;

	public float pitchLow, pitchHigh;
	public float volLow, volHigh;

	void Footstep () { PlayOneShotShakeUp(acFootstep); }
	void Jump () { PlayOneShotShakeUp(acJump); }
	void Slash () { PlayOneShotShakeUp(acSlash); }

	private void PlayOneShotShakeUp (AudioClip c) {
		audioSource.pitch = Random.Range(pitchLow, pitchHigh);
		audioSource.volume = Random.Range(volLow, volHigh);
		audioSource.PlayOneShot(c);
	}
}
