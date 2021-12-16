using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionEvent : ActionEvent
{
	public const string HIT = "Hit";

	public AttackActionEvent(string type, AttackAction action, Creature target) : base(type, action, target)
	{
	}
	
	public bool knockbacked = false;
	public int damage = 0;

	public new AttackAction action
	{
		get { return action_ as AttackAction; }
	}

	public new Creature target
	{
		get { return target_ as Creature; }
	}
}
