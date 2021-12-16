using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterValue : CreatureValue, IStatisticsValue
{
	public MonsterValue() : base() { }

	public override void CloneValuesFrom(IStatisticsValue value)
	{
		base.CloneValuesFrom(value);
		MonsterValue monsterValue = value as MonsterValue;

		if(monsterValue == null)
		{
			return;
		}

		givingExp = monsterValue.givingExp;
	}

	public int givingExp = 10;
}
