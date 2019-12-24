using UnityEngine;
using System.Collections;

public class WeaponSwordAttack : MonoBehaviour {
	public ThirdPersonControllerInput player;
	public Transform weaponSword;
	public string attackVariantType = "none";

	private Animator weaponSword_animator;

	// Use this for initialization
	void Start () {
		weaponSword_animator = weaponSword.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (CheckWantToAttack()) {
			UpdateAttackVariant();
			Attack();
		}

		if (IsCurrentlyIdling()) {
			UpdateFacingDirection();
		}
	}

	private bool CheckWantToAttack () {
		return Input.GetButtonDown("Fire1");
	}

	private void UpdateAttackVariant () {
		if (Input.GetAxisRaw("Horizontal") != 0 ||
			Input.GetAxisRaw("Vertical") != 0) {
			weaponSword_animator.SetBool("IsVerticalSlash", true);
		} else {
			weaponSword_animator.SetBool("IsVerticalSlash", false);
		}
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

	public bool IsVerticalSlash () {
		// Warning: only is reliable if player is currently animating a slash
		return weaponSword_animator.GetBool("IsVerticalSlash");
	}
}
