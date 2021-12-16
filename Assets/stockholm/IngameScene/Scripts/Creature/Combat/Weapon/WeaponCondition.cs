using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WeaponAction 사이에 딜레이를 주며, Command를 해야 다음 WeaponAction으로 넘어가는 Combo기능을 가집니다.
/// </summary>
public class WeaponCondition : IWeaponAction
{
	public string actorAnimationName { get; set; }
	public string weaponAnimationName { get; set; }
	public Weapon weapon { get; set; }

	private EndActionCallback endCallback_;
	public EndActionCallback endCallback { get { return endCallback_; } set { endCallback_ = value; } }

	public float waitTime = 0.2f;
	public bool isComboCondition = false;

	private float currentWaitTime_ = -1f;

	/// <summary>
	/// 새로운 WeaponCondition을 만듭니다.
	/// </summary>
	/// <param name="weapon">이 WeaponCondition을 실행할 Weapon입니다.</param>
	/// <param name="actorAnimationName">이 WeaponCondition을 실행할 때, 재생할 Actor의 애니메이션 이릅입니다.</param>
	/// <param name="waitTime">딜레이 시간입니다.</param>
	/// <param name="isComboCondition">이 WeaponCondition에 Combo기능을 넣습니다.</param>
	public WeaponCondition(Weapon weapon, string actorAnimationName, float waitTime, bool isComboCondition)
	{
		this.weapon = weapon;
		this.actorAnimationName = actorAnimationName;
		this.waitTime = waitTime;
		this.isComboCondition = isComboCondition;
	}

	/// <summary>
	/// 해당 Action이 처음 실행될 때 한 번 실행되는 함수입니다.
	/// </summary>
	public void OnExecute()
	{
		currentWaitTime_ = 0f;
	}

	/// <summary>
	/// 해당 Action이 실행될 때 매 프레임마다 실행되는 함수입니다.
	/// </summary>
	public void OnUpdate()
	{
		currentWaitTime_ += Time.deltaTime;
		if (currentWaitTime_ >= waitTime)
		{
			if (isComboCondition == true)
				CancelAction();
			else
				EndAction();
		}
	}

	/// <summary>
	/// 해당 Action이 끝났을 때 한 번 실행되는 함수입니다.
	/// </summary>
	public void OnEnd()
	{
	}

	/// <summary>
	/// 해당 Action을 실행합니다.
	/// </summary>
	public void ExecuteAction()
	{
		OnExecute();
	}

	/// <summary>
	/// Action을 업데이트합니다. 매 프레임마다 실행해주세요.
	/// </summary>
	public void UpdateAction()
	{
		OnUpdate();
	}

	/// <summary>
	/// 현재 실행중인 Action을 중지합니다. Sequence에 연결된 다음 Action을 실행하지 않습니다.
	/// </summary>
	public void CancelAction()
	{
		endCallback_.Invoke(false);
	}

	/// <summary>
	/// 현재 실행중인 Action을 끝냅니다. Sequence에 연결된 다음 Action을 실행합니다.
	/// </summary>
	public void EndAction()
	{
		OnEnd();
		endCallback_.Invoke(true);
	}

	/// <summary>
	/// Command를 합니다. (현재 실행중인 WeaponCondition을 캔슬하고 다음 WeaponAction으로 넘어감)
	/// Combo기능이 있을 경우에만 작동합니다.
	/// </summary>
	public void CommandAction()
	{
		if (isComboCondition == true)
			EndAction();
	}
}