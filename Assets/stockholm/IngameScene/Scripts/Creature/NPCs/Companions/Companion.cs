//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

// TODO: 전제적으로 리펙토링 (변수들은 그냥 Animator에만 넣기)
/// <summary>
/// 맨 처음에 campfirePosX를 설정해 줘야 됨.
/// </summary>
public class Companion : MonoBehaviour, INPC
{
	public CompanionAnimator animator;
	public CompanionCraftingWindowUI craftingWindowUI;
	public CompanionProgressWindowUI progressWindowUI;

	public float speed = 1f;

	private bool isShowCraftUI = false;
	private bool isShowProgressUI = false;
	private bool isNight = false;
	private bool isCrafting = false;
	private bool isWalking = false;
	public bool isStanding = false;
	private bool isDead = false;
	private bool isHandWaving = false;
	public bool isUseBlowSkill = false;
	private int currentCraftNum = 0;

	public float campfirePosX;
	private float sittingPosX;
	private float progressCraft;
	private float nightMovePos;

	private GameModule gameModule_;

	// ICreature 구현
	public void attack(bool isMelee)
	{
	}
	// ICreature 구현
	public void die()
	{
	}
	// ICreature 구현
	public void getDamaged(Transform t, float dmg, float knockBack)
	{
	}
	// ICreature 구현
	public void playKnockBack(Transform t, float knockBack)
	{
	}
	// ICreature 구현
	public void move(float axis)
	{
		this.transform.Translate(new Vector2(axis, 0));
		if (axis < 0)
			this.GetComponent<SpriteRenderer>().flipX = false;
		else
			this.GetComponent<SpriteRenderer>().flipX = true;
	}

	// IInteractable 구현
	public void Interact(InteractMessage message, params object[] param)
	{
		switch (message)
		{
			case InteractMessage.EnterNPC:
				ShowCraftUI();
				break;

			case InteractMessage.ExitNPC:
				HideCraftUI();
				break;

			default:
				Debug.LogError("Cannot read message " + message.ToString());
				break;
		}
	}

	private void MoveAtPos(float posX)
	{
		if (this.transform.position.x != posX)
		{
			float distance = posX - this.transform.position.x;

			if (Mathf.Abs(distance) <= speed * Time.deltaTime)
			{
				this.transform.position = new Vector2(posX, this.transform.position.y);
			}
			else
			{
				move(distance / Mathf.Abs(distance) * speed * Time.deltaTime);
			}

			isWalking = true;
			animator.SetBool(CompanionAnimator.AnimationBool.isWalking, true);
		}
		else
		{
			isWalking = false;
			animator.SetBool(CompanionAnimator.AnimationBool.isWalking, false);
		}
	}

	// Debug 용도
	void Start()
	{
		gameModule_ = ModuleManager.GetInstance().GetModule<GameModule>();
		if(gameModule_ != null)
		{
			gameModule_.dayStarted += OnDayStarted;
			gameModule_.nightStarted += OnNightStarted;
		}
		//campfirePosX = 0f;
		sittingPosX = campfirePosX - 2f;
	//	StartNight();

	}

	void OnDestroy()
	{
		if (gameModule_ != null)
		{
			gameModule_.dayStarted -= OnDayStarted;
			gameModule_.nightStarted -= OnNightStarted;
		}
	}

	private void OnNightStarted(int count)
	{
		StartNight();
	}

	private void OnDayStarted(int count)
	{
		StartDay();
	}

	// 유니티 내장 함수
	void Update()
	{
		if (isNight == false)
			UpdateDay();
		else
			UpdateNight();

		if (isShowCraftUI)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				MoveUpItem();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				MoveDownItem();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartCrafting();
			}
		}
		
		//TODO 디버그용 코드
		if (Input.GetKeyDown(KeyCode.Q))
		{
			UseBlowSkill();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (isHandWaving)
				StopHandWaving();
			else
				StartHandWaving();
		}
	}
	private void UpdateDay()
	{
		MoveAtPos(campfirePosX);

		if (isWalking == false)
		{
			if (isHandWaving == false)
			{
				isStanding = false;
				animator.SetBool(CompanionAnimator.AnimationBool.isStanding, false);

				ProgressCrafting();
			}
			else
			{
				isStanding = true;
				animator.SetBool(CompanionAnimator.AnimationBool.isStanding, true);
			}
		}
	}
	private void ProgressCrafting()
	{
		if (isCrafting == true && isNight == false && isWalking == false)
		{
			progressCraft += 0.1f;
			craftingWindowUI.progressBarUI.ChangeValue((int)progressCraft);
			progressWindowUI.progressBarUI.ChangeValue((int)progressCraft);

			if (progressCraft >= 100)
			{
				if (isShowCraftUI == false)
					HideCraftUI();

				HideProgressUI();
				isCrafting = false;
				animator.SetBool(CompanionAnimator.AnimationBool.isCrafting, false);
			}
		}
	}
	private void UpdateNight()
	{
		// 주변 방황
		if (isStanding == true && isUseBlowSkill == false)
		{
			if(setNightMovePosCoroutine  == null)
				setNightMovePosCoroutine = StartCoroutine(SetNightMovePos());
			MoveAtPos(nightMovePos);
		}
	}
	private Coroutine setNightMovePosCoroutine;
	private IEnumerator SetNightMovePos()
	{
		while(true) {
			nightMovePos = Random.Range(-1.5f, 1.5f) + campfirePosX;
			yield return new WaitForSeconds(Random.Range(5f, 5f));
		}
	}

	/// <summary>
	/// 제작 UI를 보여줍니다.
	/// </summary>
	public void ShowCraftUI()
	{
		HideProgressUI();
		craftingWindowUI.Show(0.3f);
		isShowCraftUI = true;
	}

	/// <summary>
	/// 제작 UI를 숨깁니다.
	/// </summary>
	public void HideCraftUI()
	{
		if (isCrafting == true && isNight == false)
			ShowProgressUI();
		craftingWindowUI.Hide(0.3f);
		isShowCraftUI = false;
	}

	private void ShowProgressUI()
	{
		progressWindowUI.Show(0.3f);
	}

	private void HideProgressUI()
	{
		progressWindowUI.Hide(0.3f);
	}

	/// <summary>
	/// 선택한 것으로 제작을 시작합니다.
	/// </summary>
	public void StartCrafting()
	{
		if (isCrafting == false)
		{
			progressCraft = 0;
			animator.SetBool(CompanionAnimator.AnimationBool.isCrafting, true);
			isCrafting = true;
		}
	}

	private void CompleteCraft()
	{
		isCrafting = false;
		animator.SetBool(CompanionAnimator.AnimationBool.isCrafting, false);
	}

	/// <summary>
	/// 낮이 시작될 때 이 함수를 호출하세요.
	/// </summary>
	public void StartDay()
	{
		if (isCrafting == true)
			ShowProgressUI();

		if(setNightMovePosCoroutine != null) {
			StopCoroutine(setNightMovePosCoroutine);
			setNightMovePosCoroutine = null;
		}

		animator.SetTrigger(CompanionAnimator.AnimationTrigger.dayStart);
		isNight = false;
	}

	/// <summary>
	/// 밤이 시작될 때 이 함수를 호출하세요.
	/// </summary>
	public void StartNight()
	{
		HideProgressUI();

		animator.SetBool(CompanionAnimator.AnimationBool.isWavingHand, false);
		isHandWaving = false;

		animator.SetTrigger(CompanionAnimator.AnimationTrigger.nightStart);
		isNight = true;
	}

	/// <summary>
	/// 플레이어가 돌아와서 손을 흔들기 위해 이 함수를 실행하세요.
	/// </summary>
	public void StartHandWaving()
	{
		animator.SetBool(CompanionAnimator.AnimationBool.isWavingHand, true);
		isHandWaving = true;
		animator.SetTrigger(CompanionAnimator.AnimationTrigger.startWavingHand);
	}
	
	/// <summary>
	/// 플레이어가 떠나서 손을 그만 흔들고 싶을 때 이 함수를 실행하세요.
	/// </summary>
	public void StopHandWaving()
	{
		animator.SetBool(CompanionAnimator.AnimationBool.isWavingHand, false);
		isHandWaving = false;
	}

	/// <summary>
	/// 제작 아이템 선택을 위로 한 칸 올립니다.
	/// </summary>
	public void MoveUpItem()
	{
		if (isShowCraftUI == true)
		{
			currentCraftNum = Mathf.Clamp(currentCraftNum-1, 0, 1);
			craftingWindowUI.selector.DOAnchorPosY(-22 - 40 * currentCraftNum, 0.3f);
		}
	}

	/// <summary>
	/// 제작 아이템 선택을 아래로 한 칸 내립니다.
	/// </summary>
	public void MoveDownItem()
	{
		if (isShowCraftUI == true)
		{
			currentCraftNum = Mathf.Clamp(currentCraftNum + 1, 0, 1);
			craftingWindowUI.selector.DOAnchorPosY(-22 - 40 * currentCraftNum, 0.3f);
		}
	}

	/// <summary>
	/// 스킬을 쓰려 할 때 이 함수를 호출하세요.
	/// </summary>
	public void UseBlowSkill()
	{
		animator.SetTrigger(CompanionAnimator.AnimationTrigger.useBlowSkill);
		isUseBlowSkill = true;
		animator.SetBool(CompanionAnimator.AnimationBool.isUseBlowSkill, true);
	}
}
