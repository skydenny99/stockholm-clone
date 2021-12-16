using System;
using UnityEngine;

/// <summary>
/// Gargoyle의 근접 무기입니다.
/// </summary>
public class GargoyleMeleeWeapon : Weapon
{
	/// <summary>
	/// Gargoyle의 근접 무기를 구현합니다.
	/// </summary>
	/// <param name="actionSequence">구현할 WeaponActionSequence입니다.</param>
	protected override void ImplementWeapon(ref WeaponActionSequence actionSequence)
	{
		actionSequence = new WeaponActionSequence();

		actionSequence.Append(new MeleeWeaponAction(this, "", "Attack", Vector2.zero, 10, 0.5f));
	}
}