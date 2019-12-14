using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip acPowerDown, acJump, acFall, acSlam;

	void PlayPowerDown () { audioSource.PlayOneShot(acPowerDown); }
	void PlayJump () { audioSource.PlayOneShot(acJump); }
	void PlayFall () { audioSource.PlayOneShot(acFall); }
	void PlaySlam () { audioSource.PlayOneShot(acSlam); }
}
