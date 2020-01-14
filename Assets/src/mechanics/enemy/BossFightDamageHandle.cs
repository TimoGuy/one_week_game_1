using UnityEngine;
using System.Collections;

public class BossFightDamageHandle : MonoBehaviour {
	public BossFightHandler bossFightHandler;
	public LifeManager lifeManager;
	public GameObject bossDieCutsceneContainer;
	private float stunTimer;

	void Start () {
#if UNITY_EDITOR
		if (lifeManager == null) {
			Debug.LogError("lifeManager assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (bossFightHandler == null) {
			Debug.LogError("bossFightHandler assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (bossDieCutsceneContainer == null) {
			Debug.LogError("bossDieCutsceneContainer assignment is required");
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif

		bossFightHandler.SetLifeInAnimator(lifeManager.GetCurrentLife());
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
		bossFightHandler.SetLifeInAnimator(lifeManager.GetCurrentLife());
		if (lifeManager.GetCurrentLife() <= 0) {
			bossDieCutsceneContainer.SetActive(true);
		}
	}
}
