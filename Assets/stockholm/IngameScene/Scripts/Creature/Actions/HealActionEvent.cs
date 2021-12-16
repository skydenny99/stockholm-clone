using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealActionEvent : ActionEvent
{
	public const string HEALED = "Healed";

	public HealActionEvent(string type, HealAction action, Creature target) : base(type, action, target)
	{
	}
	
	public int healAmount = 0;

	public new HealAction action
	{
		get { return action_ as HealAction; }
	}

	public new Creature target
	{
		get { return target_ as Creature; }
	}
}
