using System;
using UnityEngine;
/// <summary>
/// 아무 기능이 없는 효과입니다.
/// </summary>
[Serializable]
public class NoneEffect : MonoBehaviour, IItemEffect
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
