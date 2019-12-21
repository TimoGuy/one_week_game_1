using UnityEngine;
using System.Collections;

public class BossFightDamageHandle : MonoBehaviour {
	public LifeManager lifeManager;
	private float stunTimer;

	void Start () {
		if (lifeManager == null) {
			Debug.LogError("lifeManager assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}

		SendMessageUpwards("SetLifeInAnimator", lifeManager.GetCurrentLife());
	}

	void Update () {
		if (stunTimer > 0) {
			stunTimer -= Time.deltaTime;
		} else if (stunTimer < 0) {
			stunTimer = 0;
		}
	}

	void ReceiveDamage (int damage) {
		if (stunTimer > 0) return;
		stunTimer += 0.25f;
		lifeManager.SendMessage("DecrementLife", damage);
		SendMessageUpwards("SetLifeInAnimator", lifeManager.GetCurrentLife());
	}
}
