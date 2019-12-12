using UnityEngine;
using System.Collections;

public interface IAttackReceiver {
	void ReceiveDamage (int damage);
	void ReceiveKnockback (Vector3 knockbackOrigin, float knockbackYShootUp, float knockbackForce);
	void ReceiveStun (float stunTime);
}

public class AttackReceiver : MonoBehaviour {
	public string ReceivingTag = "Untagged";

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == ReceivingTag) {
			ProcessAttack(other.GetComponent<EnemyWeapon>());
		}
	}

	private void ProcessAttack (EnemyWeapon weapon) {
		SendMessage(
			"ReceiveDamage",
			weapon.damageToDeal,
			SendMessageOptions.RequireReceiver);

		switch (weapon.attackEffect) {
			case EnemyWeapon.AttackEffectType.NORMAL:
				break;

			case EnemyWeapon.AttackEffectType.KNOCKBACK:
				SendMessage(
					"ReceiveKnockback",
					new object[] {
						weapon.knockbackOrigin,
						weapon.knockbackYShootUp,
						weapon.knockbackForce
					},
					SendMessageOptions.RequireReceiver);
				break;

			case EnemyWeapon.AttackEffectType.STUN:
				SendMessage(
					"ReceiveStun",
					weapon.stunTime,
					SendMessageOptions.RequireReceiver);
				break;
		}
	}
}
