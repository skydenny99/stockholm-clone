using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Creature : MovableObject, ICreature, IActionable
{
	[SerializeField]
	protected StatisticsManager statisticsManager_;

	public Statistics<CreatureValue> statistics;
	
	[SerializeField]
	protected int hp_ = 100;
	public virtual int hp
	{
		get { return hp_; }
		set
		{
			if (!isAlive)
			{
				return;
			}

			hp_ = Mathf.Clamp(value, 0, maxHp);
			if (hp_ == 0)
			{
				Kill();
			}
		}
	}

	public virtual int maxHp
	{
		get
		{
			return statistics.currentStat.maxHp;
		}
	}

	public virtual float moveSpeed
	{
		get
		{
			return statistics.currentStat.moveSpeed;
		}
	}

	public virtual float jumpHeight
	{
		get
		{
			return statistics.currentStat.jumpHeight;
		}
	}
	
	[SerializeField]
	private bool isAlive_ = true;
	public virtual bool isAlive
	{
		get
		{
			return isAlive_;
		}
		set
		{
			isAlive_ = value;
			if (isAlive_ == false)
			{
				Kill();
			}
		}
	}

	[SerializeField]
	private LayerMask attackLayerMask_ = 0;
	public LayerMask attackLayerMask { get { return attackLayerMask_; } set { attackLayerMask_ = value; } }

	private int currentKnockbackCount_ = 0;
	public bool isGettingKnockback { get { return currentKnockbackCount_ > 0; } }
	 
	// Unity Built-in Method
	protected override void Awake()
	{
		base.Awake();

		statistics = statisticsManager_.GetStatisticsInstance<CreatureValue>();
		Debug.Assert(statistics != null);
	}

	public virtual void Damage(int damage)
	{
		if (damage > 0 && isAlive_)
		{
			hp -= damage;
		}
	}

	public virtual void Heal(int amount)
	{
		if (amount > 0 && isAlive_)
		{
			hp += amount;
		}
	}

	public override void Move(Vector2 moveVelocity)
	{
		if (!isAlive_ || isControlBlocked)
			return;

		float axis = Mathf.Clamp(moveVelocity.x, -1, 1);
		if (((transform.localScale.x > 0 && axis > 0) || (transform.localScale.x < 0 && axis < 0)))
		{
			transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
		}

		base.Move(moveVelocity * moveSpeed);
	}

	public virtual void Kill()
	{
		if (isAlive_)
		{
			hp_ = 0;
			isAlive_ = false;
			this.gameObject.layer = LayerMask.NameToLayer("DeadLayer");
		}
	}

	public virtual bool Jump()
	{
		if (isAlive_ && isGrounded && !isControlBlocked)
		{
			AddForce(Vector2.up * jumpHeight);
			return true;
		}
		return false;
	}

	public virtual bool Knockback(Vector2 force, float duration)
	{
		if (!isAlive_)
			return false;

		AddForce(force, duration, true, () => currentKnockbackCount_--);
		currentKnockbackCount_++;
		return true;
	}

	public virtual void Action(IAction action)
	{
		if (isAlive_)
		{
			action.Act(this);
		}
	}
}
