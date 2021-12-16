using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아티펙트 아이템 클래스입니다.
/// </summary>
[System.Serializable]
public class Artifact : Item
{
	public int redMoonEnforcement;

	public int probabilityWeight;

	public Artifact()
	{
		maxHavingCount = 1;
	}
	void Awake()
	{
		itemEffect = this.GetComponent<IItemEffect>();
	}
}
