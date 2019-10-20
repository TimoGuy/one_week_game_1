using UnityEngine;
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
	}

	private bool CheckWantToAttack () {
		return Input.GetButtonDown("Fire1");
	}

	private void UpdateAttackVariant () {

	}

	private void Attack () {
		Debug.Log("Attack!!!");
		weaponSword_animator.SetTrigger("Attack");
	}
}
