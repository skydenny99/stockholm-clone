using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 근접무기용 WeaponAction입니다.
/// </summary>
public class MeleeWeaponAction : WeaponAction
{
	// 한 번 맞힐때마다 입힐 데미지입니다.
	private int damagePerHit_;
	// 한 번 맞힐때마다 맞은 대상에게 줄 Force의 크기입니다.
	private float knockbackForcePerHit_;

	public MeleeWeaponAction(Weapon weapon, string actorAnimationName, string weaponAnimationName, Vector2 recoilForceOffset,  int damagePerHit, float knockbackForcePerHit)
		: base(weapon, actorAnimationName, weaponAnimationName, recoilForceOffset)
	{
		this.damagePerHit_ = damagePerHit;
		this.knockbackForcePerHit_ = knockbackForcePerHit;
	}

	public override void OnExecute()
	{
	}

	public override void OnUpdate()
	{
	}

	public override void OnTrigger()
	{
	}

	public override void OnEnd()
	{
	}

	public override void Hit(IngameObject hittedObject)
	{
		if (hittedObject == null)
			return;

		IActionable hittedActionableObject = hittedObject as IActionable;
		if (hittedActionableObject == null)
			return;

		
		AttackAction action = new AttackAction();
		action.damage = damagePerHit_;
		action.knockbackOnHit = true;
		action.knockbackForce = weapon.actor.standingSide * knockbackForcePerHit_;
		
		hittedActionableObject.Action(action);
	}
}