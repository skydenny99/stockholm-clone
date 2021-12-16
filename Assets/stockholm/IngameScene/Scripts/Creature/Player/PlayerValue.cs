using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValue : CreatureValue, IStatisticsValue
{
	public PlayerValue() : base() { }

	public override void CloneValuesFrom(IStatisticsValue value)
	{
		base.CloneValuesFrom(value);
		PlayerValue playerValue = value as PlayerValue;

		if (playerValue == null)
		{
			return;
		}

		maxJumpCount = playerValue.maxJumpCount;
		rolloverTime = playerValue.rolloverTime;
		rolloverDistance = playerValue.rolloverDistance;
		climbSpeed = playerValue.climbSpeed;
	}

	public int maxJumpCount = 1;
	public float rolloverTime = 0.6f;
	public float rolloverDistance = 4f;
	public float climbSpeed = 2;
}
