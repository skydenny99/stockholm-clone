using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본적인 아이템의 추상 클래스입니다.
/// </summary>
//[RequireComponent(typeof(IItemEffect))]
[System.Serializable]
public abstract class Item:MonoBehaviour
{
	public string itemID;
	public string itemName;
	public string itemInfo;
	public int categoryNum;

	/// <summary>
	/// 오른쪽 아래 슬룻에 표시되는 작은 아이템 이미지
	/// </summary>
	public Sprite itemImageSmall;
	/// <summary>
	/// 그 이외에 표시되는 아이템 이미지
	/// </summary>
	public Sprite itemImage;

	public int maxHavingCount;

	public IItemEffect itemEffect;

	void Awake()
	{
		itemEffect = this.GetComponent<IItemEffect>();
	}
}
