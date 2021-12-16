using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

/// <summary>
/// IngameScene에 있는 UI를 관리하는 모듈입니다.
/// </summary>
public class IngameUIModule : MonoBehaviour, IModule
{
	// TODO : gameOverText를 Window로 만들어서 관리
	public PauseBlurShader pauserBlur;

	public PlayerInfoUI playerInfoUI;
	public PlaytimeUI playtimeUI;
	public BarUI bossHPBarUI;
	public TimerUI timerUI;
	public MonsterTrackingUI monsterTrackingUI;
	public RemainEnemyUI remainEnemyUI;

	public SlotUI[] consumeItemSlots;
	public SlotUI[] artifactSlots;

	public WindowUI pauseWindowUI;
	public PopupTextWindowUI popupTextWindowUI;

	private bool isRunning_ = false;
	private Player player_;
	private GameModule gameModule_;

	// IModule 구현
	public bool InitModule()
	{
		pauserBlur.DiscardPauseBlur(0f);

		gameModule_ = ModuleManager.GetInstance().GetModule<GameModule>();

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
	}

	// Unity 내장 함수
	void Update()
	{
		if (gameModule_.isGameRunning)
		{
			if (player_ == null)
				player_ = gameModule_.player.GetComponentInChildren<Player>();
			// 플레이어 체력 / 보스 체력은 직접 관련된 코드에서 IngameUIModule함수를 실행해서 갱신
			// TODO: 계속해서 정보를 가져오고 UI에 표시(시간, 남아있는 적...)

			Backpack backpack = gameModule_.player.GetComponentInChildren<Backpack>();
			int i = 0;
			foreach (Backpack.ItemSlot<Artifact> artifact in backpack.artifactItems)
			{
				artifactSlots[i].ChangeSlotImage(artifact.item.itemImageSmall);
				artifactSlots[i].ChangeSlotNumber(artifact.count);
				i++;
			}
			for (; i < artifactSlots.Length; i++) // 나머지 슬룻들은 비워줌
			{
				artifactSlots[i].ChangeSlotImage(null);
				artifactSlots[i].ChangeSlotNumber(0);
			}

			i = 0;
			foreach (Backpack.ItemSlot<ConsumeItem> consumeItem in backpack.consumeItems)
			{
				consumeItemSlots[i].ChangeSlotImage(consumeItem.item.itemImageSmall);
				consumeItemSlots[i].ChangeSlotNumber(consumeItem.count);
				i++;
			}
			for (; i < consumeItemSlots.Length; i++) // 나머지 슬룻들은 비워줌
			{
				consumeItemSlots[i].ChangeSlotImage(null);
				consumeItemSlots[i].ChangeSlotNumber(0);
			}


			// 몬스터 위치도 함께
			// TODO: 차후 구현될 GameModule에 있는 몬스터 리스트로 가져와서 하기
			Legarcy.MonsterLegacy closestMonster = null;
			float minDistance = 0f;
			GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
			
			remainEnemyUI.ChangeRemainNum(gameModule_.monsterList.Count);

			if (gameModule_.monsterList.Count != 0)
			{
				gameModule_.monsterList.ForEach(monster =>
				{
					float distance = Vector3.Distance(monster.transform.position, player_.transform.position);
					if (closestMonster == null || distance < minDistance)
					{
						closestMonster = monster;
						minDistance = distance;
					}
				});

				Vector3 dir = (closestMonster.transform.position - player_.transform.position).normalized;
				monsterTrackingUI.RotateUI(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
			}
		}
	}

	/// <summary>
	/// 일시정지 버튼을 누를 때 실행되는 함수입니다.
	/// </summary>
	public void ClickPauseBtn()
	{
		pauseWindowUI.Show();
	}

	// ----- 외부에서 갱신되는 값에 관한 함수 -----

	public void ShowBossHPBar()
	{
		bossHPBarUI.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
	}

	public void HideBossHPBar()
	{
		bossHPBarUI.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
	}

	/// <summary>
	/// UI에 표시되는 플레이어의 HP값을 바꿉니다.
	/// </summary>
	/// <param name="value">바꿀 플레이어의 HP값입니다.</param>
	public void ChangePlayerHPValue(int value)
	{
		playerInfoUI.hpBar.ChangeValue(value);
	}

	/// <summary>
	/// UI에 표시되는 플레이어의 HP 최대값을 바꿉니다.
	/// </summary>
	/// <param name="value">바꿀 플레이어의 HP 최대값입니다.</param>
	public void ChangePlayerMaxHPValue(int value)
	{
		playerInfoUI.hpBar.ChangeMaxValue(value);
	}

	/// <summary>
	/// UI에 표시되는 보스의 HP값을 바꿉니다.
	/// </summary>
	/// <param name="value">바꿀 보스의 HP값입니다.</param>
	public void ChangeBossHPValue(int value)
	{
		bossHPBarUI.ChangeValue(value);
	}

	/// <summary>
	/// UI에 표시되는 보스의 HP 최대값을 바꿉니다.
	/// </summary>
	/// <param name="value">바꿀 보스의 HP 최대값입니다.</param>
	public void ChangeBossMaxHPValue(int value)
	{
		bossHPBarUI.ChangeMaxValue(value);
	}

	/// <summary>
	///  게임 오버 화면을 띄웁니다.
	/// </summary>
	public void ShowGameOver()
	{
		popupTextWindowUI.ShowGameOverText();
	}

	/// <summary>
	/// Day Start 텍스트를 띄웁니다.
	/// </summary>
	public void ShowDayStart()
	{
		popupTextWindowUI.ShowDayStartText();
	}

	/// <summary>
	/// Night Start 텍스트를 띄웁니다.
	/// </summary>
	public void ShowNightStart()
	{
		popupTextWindowUI.ShowNightStartText();
	}
}