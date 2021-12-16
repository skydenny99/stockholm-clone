using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Backpack : MonoBehaviour
{
	public class ItemSlot<T>
	{
		public T item;
		public int count;

		public ItemSlot(T item_)
		{
			item = item_;
			count = 1;
		}
	}

	public List<ItemSlot<Artifact>> artifactItems = new List<ItemSlot<Artifact>>();
	public List<ItemSlot<ConsumeItem>> consumeItems = new List<ItemSlot<ConsumeItem>>();

	public Player player;

	/// <summary>
	/// 소비 아이템을 쓴다.
	/// </summary>
	/// <param name="itemId">쓸 소비아이템의 ID</param>
	/// <param name="amount">쓸 양</param>
	public void UseConsumeItem(string itemId, int amount = 1)
	{
		ItemSlot<ConsumeItem> temp = consumeItems.Find(slot => slot.item.itemID == itemId);

		if (temp != null && temp.count >= amount)
		{
			for (int i = 0; i < amount; i++)
			{
				temp.item.itemEffect.ApplyEffect();
			}

			if (temp.count == amount)
			{
				consumeItems.Remove(temp);
			}
			else
			{
				temp.count -= amount;
			}
		}
	}

	/// <summary>
	/// 소비 아이템을 가방에 추가한다.
	/// </summary>
	/// <param name="consumeItem">추가할 소비 아이템</param>
	public void AddConsumeItem(ConsumeItem consumeItem)
	{
		ItemSlot<ConsumeItem> temp = consumeItems.Find(slot => slot.item.itemID == consumeItem.itemID);

		if (temp != null)
		{
			temp.count++;
			Destroy(consumeItem);
		}
		else
		{
			consumeItem.itemEffect.target = player;
			consumeItem.transform.parent = this.transform;
			consumeItems.Add(new ItemSlot<ConsumeItem>(consumeItem));
		}
	}

	/// <summary>
	/// 아티펙트를 가방에 추가한다.
	/// </summary>
	/// <param name="artifact">추가할 아티펙트</param>
	public void AddArtifact(Artifact artifact)
	{
		ItemSlot<Artifact> temp = artifactItems.Find(slot => slot.item.itemID == artifact.itemID);

		if (temp != null)
		{
			temp.count++;
		}
		else
		{
			artifact.itemEffect.target = player;
			artifact.transform.parent = this.transform;
			artifactItems.Add(new ItemSlot<Artifact>(artifact));
			artifact.itemEffect.ApplyEffect();
		}
	}
}
