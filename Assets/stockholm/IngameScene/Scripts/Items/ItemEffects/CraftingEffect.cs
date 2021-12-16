using System;
using UnityEngine;
/// <summary>
/// 제작 아이템 효과입니다. (아무 효과 없음)
/// </summary>
[Serializable]
public class CraftingEffect : MonoBehaviour, IItemEffect
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
