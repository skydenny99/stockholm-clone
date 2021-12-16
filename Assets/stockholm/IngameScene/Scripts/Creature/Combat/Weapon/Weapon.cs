using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기의 공격과 판정을 처리하는 추상클래스 입니다.
/// </summary>
[RequireComponent(typeof(HitboxManager), typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
	// 이 무기를 가지고 있는 Creature입니다.
	public Creature actor;

	// TODO: 공격속도 차후 구현
	[SerializeField]
	private float attackSpeed_ = 1f;
	public float attackSpeed { get { return attackSpeed_; } protected set { attackSpeed_ = value; } }

	protected bool isAttacking_ = false;
	public bool isAttacking { get { return isAttacking_; } }

	protected LayerMask hitLayer_;
	public LayerMask hitLayer { get { return hitLayer_; } }

	private WeaponActionSequence actionSequence_;
	public Animator animator_;
	private HitboxManager hitboxManager_;

	// Unity 내장 함수
	private void Awake()
	{
		animator_ = this.GetComponent<Animator>();
		hitboxManager_ = this.GetComponent<HitboxManager>();
		hitboxManager_.OnHitEvent += HitboxManager_OnHitEvent;

		ImplementWeapon(ref actionSequence_);
	}
	// Unity 내장 함수
	private void OnDestroy()
	{
		hitboxManager_.OnHitEvent -= HitboxManager_OnHitEvent;
	}
	// Unity 내장 함수
	private void Update()
	{
		actionSequence_.Tick();
	}

	/// <summary>
	/// HitboxManager에서 Hit가 감지될 때마다 실행되는 함수입니다.
	/// </summary>
	/// <param name="hittedObject">Hit된 IngameObject입니다.</param>
	private void HitboxManager_OnHitEvent(IngameObject hittedObject)
	{
		// MeleeWeaponAction에서만 무기에 붙어있는 Hitbox를 쓰기 때문에 현재 실행중인 WeaponAction이 MeleeWeaponAction인지 체크
		MeleeWeaponAction currentWeaponAction = actionSequence_.currentRunAction as MeleeWeaponAction;
		if (currentWeaponAction != null)
			currentWeaponAction.Hit(hittedObject);
	}

	/// <summary>
	/// 공격할 때 외부에서 실행되는 함수입니다.
	/// </summary>
	/// <param name="hitLayer"></param>
	public void Attack(LayerMask hitLayer)
	{
		hitLayer_ = hitLayer;
		hitboxManager_.layerMask = hitLayer_;

		if (actionSequence_.isPlaying == false)
		{
			isAttacking_ = true;
			actionSequence_.OnComplete(() => { isAttacking_ = false; });

			actionSequence_.Play();
		}
		else
		{
			actionSequence_.Command();
		}
	}

	/// <summary>
	/// 공격중에 캔슬하고 싶을 때 외부에서 실행되는 함수입니다.
	/// </summary>
	public void CancelAttack()
	{
		if (isAttacking_ == true)
		{
			actionSequence_.currentRunAction.CancelAction();
		}
	}

	/// <summary>
	/// WeaponAction들과 Condition들을 이용해 Weapon을 구현합니다.
	/// </summary>
	protected abstract void ImplementWeapon(ref WeaponActionSequence actionSequence);

	public void TriggerAnimation()
	{
		WeaponAction action = actionSequence_.currentRunAction as WeaponAction;
		if (action != null)
			action.TriggerAction();
	}

	/// <summary>
	/// 애니메이션이 끝날 때 AnimationEvent에 의해 실행되는 함수입니다.
	/// </summary>
	public void EndAnimation()
	{
		actionSequence_.currentRunAction.EndAction();
	}
}