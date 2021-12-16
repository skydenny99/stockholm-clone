using System;
using UnityEngine;
/// <summary>
/// 포션 최대 보유 개수 증가 효과입니다.
/// </summary>
[Serializable]
public class AddMaxHavingPotionEffect : MonoBehaviour, IItemEffect
{
	[SerializeField]
	private float effectValue_;
	public float effectValue { get { return effectValue_; } }

	public Player target { get; set; }

	// IItenEffet 구현
	public void ApplyEffect()
	{
	}
	// IItenEffet 구현
	public void DiscardEffect()
	{
	}
}
