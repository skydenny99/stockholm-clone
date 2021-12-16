using System;

[Serializable]
public class CreatureValue : IStatisticsValue
{
	public CreatureValue() { }

	public virtual void CloneValuesFrom(IStatisticsValue value)
	{
		CreatureValue creatureValue = value as CreatureValue;

		if (creatureValue == null)
		{
			return;
		}

		maxHp = creatureValue.maxHp;
		moveSpeed = creatureValue.moveSpeed;
		jumpHeight = creatureValue.jumpHeight;
	}

	public int maxHp = 100;
	public float moveSpeed = 5f;
	public float jumpHeight = 2.5f;
}
