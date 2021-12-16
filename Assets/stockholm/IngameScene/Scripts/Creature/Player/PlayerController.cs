using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController<Player>
{
	[SerializeField]
	private Player target_;
	public Player targetMonster
	{
		get; set;
	}

	private InputModule inputModule_;
	private IngameUIModule uiModule_;

	// Unity Built-in Method
	protected void Awake()
	{
		ModuleManager moduleManager = ModuleManager.GetInstance();
		Debug.Assert(moduleManager != null);

		inputModule_ = ModuleManager.GetInstance().GetModule<InputModule>();
		Debug.Assert(inputModule_ != null);

		uiModule_ = ModuleManager.GetInstance().GetModule<IngameUIModule>();
		Debug.Assert(uiModule_ != null);
	}

	protected void UpdateUI()
	{
		if (uiModule_ == null)
			return;

		uiModule_.ChangePlayerHPValue(target_.hp);
		uiModule_.ChangePlayerMaxHPValue(target_.maxHp);
	}

	protected void UpdateControl()
	{
		if (inputModule_ == null)
			return;

		//if (inputModule_.GetKey("Climb", InputModule.AxisKeyType.Positive) || inputModule_.GetKey("Climb", InputModule.AxisKeyType.Negative))
		//{
			target_.Climb(new Vector2(0, inputModule_.GetAxis("Climb")));
		//}

		target_.Move(new Vector2(inputModule_.GetAxis("Horizontal"), 0));

		if (inputModule_.GetKeyDown("Jump", InputModule.AxisKeyType.Positive))
		{
			target_.Jump();
		}

		if (inputModule_.GetKeyDown("Melee Attack"))
		{
			target_.MeleeAttack();
		}
		if (inputModule_.GetKeyDown("Ranged Attack"))
		{
			target_.RangedAttack();
		}
		if (inputModule_.GetKeyDown("Rollover"))
		{
			target_.Rollover();
		}
	}

	// Unity Built-in Method
	protected void Update()
	{
		UpdateControl();
		UpdateUI();
	}
}