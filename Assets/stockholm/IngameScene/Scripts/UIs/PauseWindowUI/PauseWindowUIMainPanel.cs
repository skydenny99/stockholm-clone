using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// PauseWindow를 열었을 때 제일 처음으로 나오는 패널입니다.
/// </summary>
public class PauseWindowUIMainPanel : WindowUIPanel
{
	public PauseWindowUIMainPanelArtifactSlot[] artifactSlots;
	public RectTransform selector;

	private enum CurrentSelect { Resume, Option, Mainmenu, Exit, Count};
	private CurrentSelect currentSelect_;

	// 유니티 내장 함수
	void Update()
	{
		if (parentWindowUI_.isActive && isActive_)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				currentSelect_ = (CurrentSelect)Mathf.Clamp((int)currentSelect_ + 1, 0, (int)(CurrentSelect.Count) - 1);

				MoveSelector();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				currentSelect_ = (CurrentSelect)Mathf.Clamp((int)currentSelect_ - 1, 0, (int)(CurrentSelect.Count) - 1);
				
				MoveSelector();
			}

			if(Input.GetKeyDown(KeyCode.Space))
			{
				Select();
			}
		}
	}

	/// <summary>
	/// 패널을 보여줍니다.
	/// </summary>
	/// <param name="duration">보이는데 걸리는 시간입니다.</param>
	public override void Show(float duration = 0.3f)
	{
		base.Show(duration);

		currentSelect_ = 0;
		selector.anchoredPosition = Vector2.zero;
		
		Backpack backpack = ModuleManager.GetInstance().GetModule<GameModule>().player.GetComponentInChildren<Backpack>();
		int i = 0;
		foreach (Backpack.ItemSlot<Artifact> artifact in backpack.artifactItems)
		{
			artifactSlots[i].ChangeArtifact(artifact.item.itemName, artifact.item.itemInfo, artifact.item.itemImage);
			i++;
		}
		for (; i < artifactSlots.Length; i++) // 나머지 슬룻들은 비워줌
		{
			artifactSlots[i].ChangeArtifact("None", "", null);
		}
	}

	/// <summary>
	/// Selector를 이동시킵니다.
	/// </summary>
	private void MoveSelector()
	{
		selector.DOAnchorPosY((int)currentSelect_ * -300, 0.3f).SetEase(Ease.OutQuart);
	}

	/// <summary>
	/// 선택을 합니다.
	/// </summary>
	private void Select()
	{
		// TODO: 차후 구현
		switch (currentSelect_)
		{
			case CurrentSelect.Resume:
				parentWindowUI_.Hide();
				break;

			case CurrentSelect.Option:
				break;

			case CurrentSelect.Mainmenu:
				break;

			case CurrentSelect.Exit:
				break;

			case CurrentSelect.Count:
				break;

			default:
				break;
		}
	}
}
