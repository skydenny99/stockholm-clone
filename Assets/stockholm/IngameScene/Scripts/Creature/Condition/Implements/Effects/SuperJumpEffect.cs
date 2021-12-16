using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpEffect : Buff
{
	private StatisticsExpression superjumpExpression_;
	protected override void Awake()
	{
		base.Awake();
		superjumpExpression_ = GetComponent<StatisticsExpression>();
		Debug.Assert(superjumpExpression_ != null);
	}

    public override void applyEffect()
    {
        Player player = (target as Player);
        Debug.Assert(player != null);


		player.statistics.AddExpression(superjumpExpression_);

    }

    public override void revokeEffect()
    {
        Player player = (target as Player);
        Debug.Assert(player != null);

		player.statistics.RemoveExpression(superjumpExpression_);
    }

}
