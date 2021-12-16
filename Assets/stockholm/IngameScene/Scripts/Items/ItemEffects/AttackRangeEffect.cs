using System;
using UnityEngine;
/// <summary>
/// 원격 무기 소모 아이템입니다. (효과는 없음)
/// </summary>
[Serializable]
public class AttackRangeEffect : MonoBehaviour, IItemEffect
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
