using UnityEngine;
using System.Collections;

public class SpiderAnimatorHandler : MonoBehaviour {
	public Animator spiderAnimator;
	public Transform InnerLazerContainerAnimated;
	public Transform SpiderModelContainer;
	private bool FollowLazerRotation;

	void Start () {
		FollowLazerRotation = false;

#if UNITY_EDITOR
		if (spiderAnimator == null) {
			Debug.LogError("spiderAnimator assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (InnerLazerContainerAnimated == null) {
			Debug.LogError("InnerLazerContainerAnimated assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (SpiderModelContainer == null) {
			Debug.LogError("SpiderModelContainer assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
	}

	void FixedUpdate () {
		if (FollowLazerRotation) {
			Vector3 rotation = InnerLazerContainerAnimated.rotation.eulerAngles;
			rotation.x = rotation.z = 0;
			rotation.y += 90;
			SpiderModelContainer.rotation = Quaternion.Euler(rotation);
		}
	}
	
	void Spider_EndIntroHold () { spiderAnimator.SetTrigger("End_Intro_Hold"); }
	void Spider_JumpUp () { spiderAnimator.SetTrigger("Start_Jump_Up"); }
	void Spider_ButtSlam () { spiderAnimator.SetTrigger("Start_Butt_Slam"); }
	void Spider_DoLazer (bool flag) { spiderAnimator.SetBool("Do_Lazer", flag); }
	void Spider_EndConcentrate () { spiderAnimator.SetTrigger("End_Concentrate"); }
	void Spider_FollowLazerRotation (int flag) { FollowLazerRotation = flag != 0; }		// Uses int so that can be used by Unity
}
