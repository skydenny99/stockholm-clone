using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 아이템에 관한 처리를 하는 모듈입니댜.
/// </summary>
public class ItemModule : MonoBehaviour, IModule
{
	// TODO: 아이템 구조 리펙토링
	public List<ConsumeItem> consumeItems;
	public List<Artifact> artifactItems;
	public DropItem dropItem;

	private List<Artifact> remainArtifactItem_;

	private bool isRunning_ = false;

	// IModule 구현
	public bool InitModule()
	{
		remainArtifactItem_ = new List<Artifact>(artifactItems);
		return true;
	}
	// IModule 구현
	public bool RunModule()
	{
		isRunning_ = true;

		return true;
	}
	// IModule 구현 
	public void StopModule()
	{
		isRunning_ = false;
	}
	// IModule 구현
	public void DestroyModule()
	{
		//Destroy(this);
	}

	// 디버그 용도
	void Start()
	{
		StartCoroutine(Test());
	}

	IEnumerator Test()
	{
		yield return new WaitForSeconds(2f);
		Artifact a = GetRandomArtifact();
		ModuleManager.GetInstance().GetModule<GameModule>().player.GetComponentInChildren<Backpack>().AddArtifact(a);
	}

	/// <summary>
	/// 남아있는 아티펙트중 무작위로 하나 가져옵니다.
	/// </summary>
	/// <returns>무작위 아티펙트입니다. 만약 가져올 아티펙트가 없다면, null을 반환합니다.</returns>
	public Artifact GetRandomArtifact()
	{
		if (remainArtifactItem_.Count <= 0)
		{
			return null;
		}

		List<int> probabilityBalls = new List<int>();
		for (int i = 0; i < remainArtifactItem_.Count; i++)
		{
			for (int j = 0; j < remainArtifactItem_[i].probabilityWeight; j++)
				probabilityBalls.Add(i);
		}

		int selectedIndex = probabilityBalls[Random.Range(0, probabilityBalls.Count)];
		Artifact artifact = remainArtifactItem_[selectedIndex];
		remainArtifactItem_.Remove(artifact);

		return Instantiate(artifact);
	}

	/// <summary>
	/// 남아있는 아티펙트 중 해당 id의 아티펙트르 하나 가져옵니다.
	/// </summary>
	/// <param name="id">가져올 아티펙트 id입니다.</param>
	/// <returns>해당 id의 아티펙트입니다. 만약 해당 아티펙트가 없다면, null을 반환합니다.</returns>
	public Artifact GetArtifact(string id)
	{
		Artifact artifact = remainArtifactItem_.Find(x => x.itemID.Equals(id));

		if (artifact == null)
			return null;

		remainArtifactItem_.Remove(artifact);

		return Instantiate(artifact);
	}

	/// <summary>
	/// 해당 id의 소비아이템을 하나 가져옵니다.
	/// </summary>
	/// <param name="id">가져올 소비아이템의 id입니다.</param>
	/// <returns>해당 id의 소비아이템입니다. 만약 해당 소비아이템이 없다면, null을 반환합니다.</returns>
	public ConsumeItem GetConsumeItem(string id)
	{
		ConsumeItem cItem = consumeItems.Find(x => x.itemID.Equals(id));
		if (cItem == null)
			return null;

		return Instantiate(cItem);
	}

	public DropItem GetDropItem(Item item)
	{
		DropItem di = Instantiate(dropItem);
		di.GetItem(item);

		return di;
	}
}
