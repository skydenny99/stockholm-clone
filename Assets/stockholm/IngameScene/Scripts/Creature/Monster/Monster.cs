using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature, IControlable
{
	public bool isNightMonster;
	public Weapon weapon;

	public virtual void Attack()
	{
		weapon.Attack(attackLayerMask);
	}

	public virtual void CancelAttack()
	{
		//weapon.CancelAttack();
	}
}
