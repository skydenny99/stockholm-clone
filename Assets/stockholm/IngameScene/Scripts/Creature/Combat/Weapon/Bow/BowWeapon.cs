using System;
using UnityEngine;

/// <summary>
/// Arrow을 쏘는 무기입니다.
/// </summary>
public class BowWeapon : Weapon
{
	/// <summary>
	/// 쏠 projectile입니다.
	/// </summary>
	public Projectile arrow;


	/// <summary>
	/// BowWeapon을 구현합니다.
	/// </summary>
	/// <param name="actionSequence">구현할 WeaponActionSequence입니다.</param>
	protected override void ImplementWeapon(ref WeaponActionSequence actionSequence)
	{
		actionSequence = new WeaponActionSequence();

		actionSequence.Append(new RangeWeaponAction(this, "", "Attack", Vector2.zero, arrow));
	}
}