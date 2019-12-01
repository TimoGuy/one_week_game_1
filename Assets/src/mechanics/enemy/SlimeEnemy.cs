using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour {
	public Transform parentEnemyWrapper;
	private EnemyProfile _profile;
	private Animator _animator;
	public float playerChargeDist;
	private const string CHARGE_BOOL_NAME = "isCharging";
	public float playerAttackDist;
	private const string ATTACK_BOOL_NAME = "isAttacking";
	public float knockBackDist;
	private const string KNOCKBACK_TRIGGER_NAME = "getHurt";

	// Use this for initialization
	void Start () {
		_profile = GetComponent<EnemyProfile>();
		_animator = GetComponent<Animator>();
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player Weapon") {
			// Get hurt
			Vector3 lookAt =
				_profile.GetPlayerTransform().position
				- transform.position;
			lookAt.y = 0;
			lookAt.Normalize();

			parentEnemyWrapper.position -= lookAt * knockBackDist;
			_animator.SetTrigger(KNOCKBACK_TRIGGER_NAME);
		}
	}

	// Called by animation
	public void AimTowardsPlayer () {
		Vector3 lookAt =
			_profile.GetPlayerTransform().position
			- transform.position;
		lookAt.y = 0;

		var q = new Quaternion();
		q.SetLookRotation(lookAt);
		parentEnemyWrapper.rotation = q;
	}

	// Called by animation
	public void ResetChargeAndAttackDists () {
		_animator.SetBool(CHARGE_BOOL_NAME, false);
		_animator.SetBool(ATTACK_BOOL_NAME, false);
	}
	
	// Called by animation
	public void CheckIfChargingOrAttacking () {
		float dist = Vector3.Distance(
			transform.position,
			_profile.GetPlayerTransform().position
		);

		if (_animator.GetBool(CHARGE_BOOL_NAME) &&
			dist <= playerAttackDist) {
			_animator.SetBool(ATTACK_BOOL_NAME, true);
		} else if (dist <= playerChargeDist) {
			// Charge!!!!
			_animator.SetBool(CHARGE_BOOL_NAME, true);
		} else {
			// Stop it for now eh
			_animator.SetBool(CHARGE_BOOL_NAME, false);
		}
	}
}
