using System;
using UnityEngine;
/// <summary>
/// 체력을 회복하는 효과입니다.
/// </summary>
[Serializable]
public class HealingHPEffect : MonoBehaviour, IItemEffect
{
	[SerializeField]
	private float effectValue_;
	public float effectValue { get { return effectValue_; } }

	public Player target { get; set; }

	// IItenEffet 구현
	public void ApplyEffect()
	{
		target.hp += (int)effectValue_;
	}
	// IItenEffet 구현
	public void DiscardEffect()
	{
	}
}
