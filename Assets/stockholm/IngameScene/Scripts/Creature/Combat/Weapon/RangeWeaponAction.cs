using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 여러 Projectile들을 처리
/// <summary>
/// 원거리 무기용 WeaponAction입니다.
/// </summary>
public class RangeWeaponAction : WeaponAction
{
	// 발사할 Projectile들 입니다.
	protected Projectile[] projectiles_;

	public RangeWeaponAction(Weapon weapon, string actorAnimationName, string weaponAnimationName, Vector2 recoilForceOffset, params Projectile[] projectiles)
		: base(weapon, actorAnimationName, weaponAnimationName, recoilForceOffset)
	{
		this.projectiles_ = projectiles;
	}

	public override void OnExecute()
	{
	}

	public override void OnTrigger()
	{
		Projectile proj = UnityEngine.Object.Instantiate(projectiles_[0], weapon.transform.position, Quaternion.identity);

		proj.Fire(35f, 10f, weapon.actor.standingSide, weapon.hitLayer, weapon.actor, HitInProjectile);
	}

	public override void OnUpdate()
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
		action.damage = projectiles_[0].damage;

		hittedActionableObject.Action(action);
	}

	private void HitInProjectile(Creature caller, IngameObject hittedObject)
	{
		Hit(hittedObject);
	}
}