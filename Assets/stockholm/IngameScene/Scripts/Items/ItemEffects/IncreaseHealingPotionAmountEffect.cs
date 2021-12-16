using System;
using UnityEngine;
/// <summary>
/// 회복 포션 효과가 증가하는 효과입니다.
/// </summary>
[Serializable]
public class IncreaseHealingPotionAmountEffect : MonoBehaviour, IItemEffect
{
	[SerializeField]
	private float effectValue_;
	public float effectValue { get { return effectValue_; } }

	public Player target { get; set; }

	// IItenEffet 구현
	public void ApplyEffect()
	{
		//target.HealingPotionAmound += effectValue_;
	}
	// IItenEffet 구현
	public void DiscardEffect()
	{
		//target.HealingPotionAmound -= effectValue_;
	}
}
