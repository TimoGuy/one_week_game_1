using UnityEngine;
using System.Collections;

public class WeaponSwordAttack : MonoBehaviour {
	public ThirdPersonControllerInput player;
	public GameObject modelSword;
	public Transform weaponSword;
	public string attackVariantType = "none";
	private Animator weaponSword_animator;
	private bool assumeHasModelSword = false;

	// Use this for initialization
	void Start () {
		weaponSword_animator = weaponSword.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsSwordEnabled()) {
			if (CheckWantToAttack()) {
				Attack();
			}
		}

		if (IsCurrentlyIdling()) {
			UpdateFacingDirection();
		}
	}

	private bool IsSwordEnabled () {
		if (assumeHasModelSword ==
			modelSword.activeInHierarchy) {
			return assumeHasModelSword;
		}

		assumeHasModelSword = modelSword.activeInHierarchy;
		SendMessageUpwards("SetHasSword", assumeHasModelSword);
		return assumeHasModelSword;
	}

	private bool CheckWantToAttack () {
		return Input.GetButtonDown("Fire1") &&
			player.isActiveAndEnabled;
	}

	private void Attack () {
		weaponSword_animator.SetTrigger("Attack");
		SendMessageUpwards("TriggerAttackAnim");
	}

	public bool IsCurrentlyIdling () {
		var animInfo = weaponSword_animator.GetCurrentAnimatorStateInfo(0);
		return animInfo.IsName("idle");
	}

	private void UpdateFacingDirection () {
		var lookVec = player.GetLookDirection();
		lookVec.y = 0;
		if (lookVec != Vector3.zero)
			transform.rotation =
				Quaternion.LookRotation(lookVec);
	}
}
