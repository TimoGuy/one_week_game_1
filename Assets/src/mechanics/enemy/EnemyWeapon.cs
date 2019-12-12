using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour {
	public enum AttackEffectType {
		NORMAL, KNOCKBACK, STUN
	}
	public AttackEffectType attackEffect = AttackEffectType.NORMAL;

	// All
	public int damageToDeal;

	// If knockback
	public Vector3 knockbackOrigin;
	public float knockbackYShootUp = 10;
	public float knockbackForce = 1;
	
	// If stun
	public float stunTime;
}


