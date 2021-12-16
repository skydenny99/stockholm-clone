using System;
using UnityEngine;
/// <summary>
/// 구르기를 할 때 무적이 되는 효과입니다.
/// </summary>
[Serializable]
public class ImmortalityInRollEffect : MonoBehaviour, IItemEffect
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
