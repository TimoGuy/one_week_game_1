using UnityEngine;
using System.Collections;

public class SFXPlayer_Player : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip acFootstep, acJump, acSlash;

	void Footstep () { audioSource.PlayOneShot(acFootstep); }
	void Jump () { audioSource.PlayOneShot(acJump); }
	void Slash () { audioSource.PlayOneShot(acSlash); }
}
