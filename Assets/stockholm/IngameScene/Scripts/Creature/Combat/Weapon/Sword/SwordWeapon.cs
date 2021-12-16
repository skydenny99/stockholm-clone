using System;
using UnityEngine;

/// <summary>
/// 3연타를 하는 Sword입니다.
/// </summary>
public class SwordWeapon : Weapon
{
	/// <summary>
	/// Sword를 구현합니다.
	/// </summary>
	/// <param name="actionSequence">구현할 WeaponActionSequence입니다.</param>
	protected override void ImplementWeapon(ref WeaponActionSequence actionSequence)
	{
		actionSequence = new WeaponActionSequence();

		actionSequence.Append(new MeleeWeaponAction(this, "", "Attack", Vector2.zero, 10, 0.5f))
			.Append(new WeaponCondition(this, "", 0.3f, true))
			.Append(new MeleeWeaponAction(this, "", "Attack2", Vector2.left * 1f, 10, 0.5f))
			.Append(new WeaponCondition(this, "", 0.3f, true))
			.Append(new MeleeWeaponAction(this, "", "Attack3", Vector2.zero, 20, 1f));
	}
}