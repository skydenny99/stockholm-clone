using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
	public AttackAction() : base() { }

	public override void Act(IActionable t)
	{
		base.Act(t);

		Creature target = t as Creature;
		if (target == null)
			return;

		if (target.isAlive == false)
			return;

		AttackActionEvent actionEvent = new AttackActionEvent(AttackActionEvent.HIT, this, target);

		target.Damage(damage);
		actionEvent.damage = damage;

		if (knockbackOnHit)
		{
			target.Knockback(knockbackForce, knockbackTime);
			actionEvent.knockbacked = true;
		}

		if (hit != null)
			hit.Invoke(actionEvent);
	}

	private ActionCallback<AttackActionEvent> hit_;
	public ActionCallback<AttackActionEvent> hit
	{
		get { return hit_; }
	}

	public int damage = 0;
	public bool knockbackOnHit = false;
	public Vector2 knockbackForce = new Vector2(1f, 0f);
	public float knockbackTime = 0.4f;
}
