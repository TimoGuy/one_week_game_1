#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyWeapon)), CanEditMultipleObjects]
public class EnemyWeaponEditor : Editor {
	
	public SerializedProperty 
		attackEffect_Prop,
		damageToDeal_Prop,
		knockbackOrigin_Prop,
		knockbackYShootUp_Prop,
		knockbackForce_Prop,
		stunTime_Prop;
	
	void OnEnable () {
		// Setup the SerializedProperties
		attackEffect_Prop = serializedObject.FindProperty("attackEffect");
		damageToDeal_Prop = serializedObject.FindProperty("damageToDeal");
		knockbackOrigin_Prop = serializedObject.FindProperty("knockbackOrigin");
		knockbackYShootUp_Prop = serializedObject.FindProperty("knockbackYShootUp");
		knockbackForce_Prop = serializedObject.FindProperty("knockbackForce");
		stunTime_Prop = serializedObject.FindProperty("stunTime");
	}
	
	public override void OnInspectorGUI () {
		serializedObject.Update();
		
		EditorGUILayout.PropertyField(attackEffect_Prop);
		EnemyWeapon.AttackEffectType state = (EnemyWeapon.AttackEffectType)attackEffect_Prop.enumValueIndex;
		
		EditorGUILayout.IntSlider(damageToDeal_Prop, 0, 100, new GUIContent("damageToDeal"));
		
		switch (state) {
			case EnemyWeapon.AttackEffectType.NORMAL:
				break;

			case EnemyWeapon.AttackEffectType.KNOCKBACK:
				knockbackOrigin_Prop.vector3Value =
					EditorGUILayout.Vector3Field(new GUIContent("knockbackOrigin"), knockbackOrigin_Prop.vector3Value);
				knockbackYShootUp_Prop.floatValue =
					EditorGUILayout.FloatField(new GUIContent("knockbackYShootUp"), knockbackYShootUp_Prop.floatValue);
				knockbackForce_Prop.floatValue =
					EditorGUILayout.FloatField(new GUIContent("knockbackForce"), knockbackForce_Prop.floatValue);
				break;

			case EnemyWeapon.AttackEffectType.STUN:
				stunTime_Prop.floatValue =
					EditorGUILayout.FloatField(new GUIContent("stunTime"), stunTime_Prop.floatValue);
				break;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
