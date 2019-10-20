﻿using UnityEngine;
using System.Collections;

public class WeaponSwordAttack : MonoBehaviour {
	public CharacterController player;
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
		// TODO make more attack variants!
	}

	private void Attack () {
		weaponSword_animator.SetTrigger("Attack");
	}

	private bool IsCurrentlyIdling () {
		var animInfo = weaponSword_animator.GetCurrentAnimatorStateInfo(0);
		return animInfo.IsName("idle");
	}

	private void UpdateFacingDirection () {
		transform.rotation =
			Quaternion.LookRotation(new Vector3(player.velocity.x, 0, player.velocity.z));

	}
}
