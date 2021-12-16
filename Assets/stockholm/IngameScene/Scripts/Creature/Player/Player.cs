using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Creature, IControlable
{
	[SerializeField]
	private Animator animator_;
	public new Statistics<PlayerValue> statistics;

	[SerializeField]
	private bool useGravityOnClimb_ = true;

	[SerializeField]
	private SwordWeapon meleeWeapon_;
	public SwordWeapon meleeWeapon { get { return meleeWeapon_; } }

	[SerializeField]
	private BowWeapon rangeWeapon_;
	public BowWeapon rangeWeapon { get { return rangeWeapon_; } }
	
	private bool isClimbing_ = false;
	private bool isAttacking_ { get { return animator_.GetCurrentAnimatorStateInfo(0).IsTag("Attack"); } }

	private int jumpCount_ = 0;

    protected override void Awake()
	{
		base.Awake();

		statistics = statisticsManager_.GetStatisticsInstance<PlayerValue>();
		Debug.Assert(statistics != null);
	}

	protected override void Update()
	{
		base.Update();

		animator_.SetBool("alive", isAlive);
		animator_.SetBool("grounded", isGrounded);
		animator_.SetBool("knockback", isGettingKnockback);

		Vector2 velocityWithoutForce = gravityVelocity + moveVelocity;
		animator_.SetFloat("vertical_velocity", velocityWithoutForce.y);
		animator_.SetFloat("horizontal_velocity", velocityWithoutForce.x);

		if (isGrounded)
			jumpCount_ = 0;
	}

	public virtual bool Rollover()
	{
		if (isAlive && !isControlBlocked && isGrounded)
		{
			AddForce(standingSide * statistics.currentStat.rolloverDistance, statistics.currentStat.rolloverTime, true);
			animator_.SetTrigger("rollover");
			return true;
		}
		return false;
	}


	private Collider2D GetClimbObject(Vector2 position)
	{
		Collider2D[] cols = Physics2D.OverlapPointAll(position, 1 << LayerMask.NameToLayer("Object"));
		Collider2D ladder = cols.ToList().Find(col => col.CompareTag("Ladder"));

		return ladder;
	}

	public override void Move(Vector2 moveVelocity)
	{
		if (!isClimbing_ && !meleeWeapon.isAttacking && !rangeWeapon.isAttacking)
			base.Move(moveVelocity);
	}

	public override bool Knockback(Vector2 force, float duration)
	{
		if (isClimbing_)
		{
			StopClimb();
		}
		return base.Knockback(force, duration);
	}

	public void Climb(Vector2 climbAxis)
	{
		if (!isAlive || isControlBlocked || (!isClimbing_ && climbAxis == Vector2.zero))
			return;

		climbAxis = new Vector2(0, Mathf.Clamp(climbAxis.y, -1, 1));

		Collider2D centerLadder = GetClimbObject(transform.position);
		Collider2D bottomLadder = GetClimbObject(transform.position - new Vector3(0, collider.bounds.size.y / 2, 0));
		bool isLadderHit = centerLadder != null || bottomLadder != null;

		if (!isClimbing_)
		{
			if (climbAxis.y > 0 && centerLadder != null)
			{
				StartClimb();
				transform.position = new Vector2(centerLadder.transform.position.x, transform.position.y);
			}
			else if (climbAxis.y < 0 && centerLadder == null && bottomLadder != null)
			{
				StartClimb();
				transform.position = new Vector2(bottomLadder.transform.position.x, transform.position.y);
			}
		}
		else
		{
			Collider2D nextBottomLadder = GetClimbObject(transform.position - new Vector3(0, collider.bounds.size.y / 2, 0));
			bool isNextLadderHit = centerLadder != null || bottomLadder != null;

			if ((climbAxis.y < 0 && bottomLadder == null) || !isLadderHit)
			{
				StopClimb();
			}
			else if (isLadderHit)
			{
				if (!useGravityOnClimb_ || climbAxis.y > 0)
				{
					base.Move(climbAxis * statistics.currentStat.climbSpeed * Time.deltaTime);
				}
				else if (useGravityOnClimb_)
				{
					useGravity = climbAxis.y < 0;
				}
			}
		}
	}

	private void StartClimb()
	{
		animator_.SetBool("climb", true);
		useGravity = false;
		groundCheck = false;
		isClimbing_ = true;
		jumpCount_ = 0;
	}
	private void StopClimb()
	{
		animator_.SetBool("climb", false);
		useGravity = true;
		groundCheck = true;
		isClimbing_ = false;
	}

	public void MeleeAttack()
	{
		if (!isClimbing_ && !isStatic && !isControlBlocked && !isGettingKnockback && isAlive)
		{
			//animator_.SetTrigger("melee_attack");
			//meleeWeapon.Attack(this, attackLayerMask);
			meleeWeapon.Attack(attackLayerMask);
		}
	}

	public void RangedAttack()
	{
		if (!isClimbing_ && !isStatic && !isControlBlocked && !isGettingKnockback && isAlive)
		{
			animator_.SetTrigger("range_attack");
			//rangeWeapon.Attack(this, attackLayerMask |  LayerMask.GetMask("Ground"));
			rangeWeapon.Attack(attackLayerMask | LayerMask.GetMask("Ground"));
		}
	}

	public override bool Jump()
	{
		if ((isAlive && !isControlBlocked && !isAttacking_ && (isGrounded || jumpCount_ < statistics.currentStat.maxJumpCount)) || isClimbing_)
		{
			if (isClimbing_)
			{
				StopClimb();
			}
			jumpCount_++;
			animator_.SetTrigger("jump");
			gravityVelocity_ = Vector2.zero;
			AddForce(Vector2.up * jumpHeight / jumpCount_);
			return true;
		}

		if (base.Jump())
		{
			animator_.SetTrigger("jump");
			return true;
		}
		return false;
	}
}
