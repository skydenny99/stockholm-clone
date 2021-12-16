using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : Action {

	public HealAction() : base()
	{
		
	}

	public override void Act(IActionable t)
	{
		base.Act(t);

		Creature target = t as Creature;
		if (target == null)
			return;

		if (target.isAlive == false)
			return;

		HealActionEvent actionEvent = new HealActionEvent(HealActionEvent.HEALED, this, target);

		int healableAmount = target.maxHp - target.hp;

		target.Heal(healAmount);
		actionEvent.healAmount = Mathf.Clamp(healAmount, 0, healableAmount);

		if (healed != null)
			healed.Invoke(actionEvent);
	}
	
	private ActionCallback<HealActionEvent> healed_;
	public ActionCallback<HealActionEvent> healed
	{
		get { return healed_; }
	}

	public int healAmount = 0;
}
