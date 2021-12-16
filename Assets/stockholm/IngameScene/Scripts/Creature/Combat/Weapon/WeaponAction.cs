using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기에서 실질적인 공격 모션과 판정을 처리하는 추상클래스입니다.
/// </summary>
public abstract class WeaponAction : IWeaponAction
{
	public Weapon weapon { get; set; }
	public string actorAnimationName { get; set; }
	public string weaponAnimationName { get; set; }

	private EndActionCallback endCallback_;
	public EndActionCallback endCallback { get { return endCallback_; } set { endCallback_ = value; } }

	// WeaponAction을 실행할 때 발생하는 반동. 왼쪽을 바라보는 것을 기준으로 한다.
	private Vector2 recoilForceOffset_;

	/// <summary>
	/// 새로은 WeaponAction을 만듭니다.
	/// </summary>
	/// <param name="weapon">이 WeaponAction을 실행할 Weapon입니다.</param>
	/// <param name="actorAnimationName">이 WeaponAction을 실행할 때, 재생할 Actor의 애니메이션 이릅입니다.</param>
	/// <param name="weaponAnimationName">이 WeaponAction을 실행할 때, 재생할 무기의 애니메이션 이릅입니다.</param>
	/// <param name="recoilForceOffset">이 WeaponAction을 실행할 때, 발생하는 반동입니다.</param>
	public WeaponAction(Weapon weapon, string actorAnimationName, string weaponAnimationName, Vector2 recoilForceOffset)
	{
		this.weapon = weapon;
		this.actorAnimationName = actorAnimationName;
		this.weaponAnimationName = weaponAnimationName;
		this.recoilForceOffset_ = recoilForceOffset;
	}

	/// <summary>
	/// 해당 Action이 처음 실행될 때 한 번 실행되는 함수입니다.
	/// </summary>
	public abstract void OnExecute();

	/// <summary>
	/// 해당 Action이 실행될 때 매 프레임마다 실행되는 함수입니다.
	/// </summary>
	public abstract void OnUpdate();

	/// <summary>
	/// AnimationEvent에 의해서 특정 상황에 실행되는 함수입니다.
	/// </summary>
	public abstract void OnTrigger();

	/// <summary>
	/// 해당 Action이 끝났을 때 한 번 실행되는 함수입니다.
	/// </summary>
	public abstract void OnEnd();

	/// <summary>
	/// 해당 Action을 실행합니다.
	/// </summary>
	public void ExecuteAction()
	{
		if(recoilForceOffset_ != Vector2.zero)
			weapon.actor.AddForce(new Vector2(recoilForceOffset_.x * (-weapon.actor.standingSide.x), recoilForceOffset_.y));

		weapon.animator_.CrossFade(weaponAnimationName, 0);

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
	/// AnimationEvent에 의해서 특정 상황에 실행되는 함수입니다.
	/// </summary>
	public void TriggerAction()
	{
		OnTrigger();
	}

	/// <summary>
	/// 현재 실행중인 Action을 중지합니다. Sequence에 연결된 다음 Action을 실행하지 않습니다.
	/// </summary>
	public virtual void CancelAction()
	{
		weapon.animator_.CrossFade("Idle", 0);

		endCallback_.Invoke(false);
	}

	/// <summary>
	/// 현재 실행중인 Action을 끝냅니다. Sequence에 연결된 다음 Action을 실행합니다.
	/// </summary>
	public void EndAction()
	{
		weapon.animator_.CrossFade("Idle", 0);

		OnEnd();

		endCallback_.Invoke(true);
	}

	/// <summary>
	/// 무기가 적에 닿았을 때 실행되는 합수입니다.
	/// </summary>
	public abstract void Hit(IngameObject hittedObject);
}