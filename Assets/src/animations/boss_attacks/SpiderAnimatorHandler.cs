using UnityEngine;
using System.Collections;

public class SpiderAnimatorHandler : MonoBehaviour {
	public Animator spiderAnimator;

	void Start () {
#if UNITY_EDITOR
		if (spiderAnimator == null) {
			Debug.LogError("spiderAnimator assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif
	}
	
	void Spider_EndIntroHold () { spiderAnimator.SetTrigger("End_Intro_Hold"); }
	void Spider_JumpUp () { spiderAnimator.SetTrigger("Start_Jump_Up"); }
	void Spider_ButtSlam () { spiderAnimator.SetTrigger("Start_Butt_Slam"); }
	void Spider_DoLazer (bool flag) { spiderAnimator.SetBool("Do_Lazer", flag); }
	void Spider_EndConcentrate () { spiderAnimator.SetTrigger("End_Concentrate"); }
}
