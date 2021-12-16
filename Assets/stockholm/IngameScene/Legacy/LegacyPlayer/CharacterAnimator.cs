using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
	public enum PlayerAnimateTrigger
	{
		GetFrontHit,
		GetBackHit,
		Interaction,
		RollOver,
		Jump,
		MeleeAttack,
		RangeAttack,
		Die
	}

	public enum CurrentPlayerAnimate
	{
		Idle,
		Interacting,
		Walking,
		Attacking,
		Jumping,
		Falling,
		RollingOver,
		Climbing,
		GetHit,
		Die
	}

	public Animator anim;
	public GameObject cController;
	public Character _control;

	// Use this for initialization
	void Start()
	{
		anim = gameObject.GetComponent<Animator>();
		_control = cController.GetComponent<Character>();
		Debug.Assert(_control);
		Debug.Assert(anim);
	}

	public void meleeAttack()
	{
		_control.attack(true);
	}

	public void rangeAttack()
	{
		_control.attack(false);
	}

	public void setClimbing(bool isClimbing)
	{
		anim.SetBool("IsClimbing", isClimbing);
	}

	public void setGrounded(bool isGrounded)
	{
		anim.SetBool("IsGrounded", isGrounded);
	}


	public void setSpeed(float speed)
	{
		anim.SetFloat("Speed", speed);
	}

	public void setStopClimbing(float stop)
	{
		anim.SetFloat("Stop Climbing", stop);
	}


	public void resetAllTrigger()
	{
		anim.ResetTrigger("Melee Attack");
		anim.ResetTrigger("Range Attack");
		anim.ResetTrigger("Die");
		anim.ResetTrigger("Interaction");
		anim.ResetTrigger("Roll Over");
		anim.ResetTrigger("Jump");
		anim.ResetTrigger("Get Front Hit");
		anim.ResetTrigger("Get Back Hit");
	}

	public void triggerAnimate(PlayerAnimateTrigger tri)
	{
		switch (tri)
		{
			case PlayerAnimateTrigger.MeleeAttack:
				anim.SetTrigger("Melee Attack");
				break;
			case PlayerAnimateTrigger.RangeAttack:
				anim.SetTrigger("Range Attack");
				break;
			case PlayerAnimateTrigger.Jump:
				anim.SetTrigger("Jump");
				break;
			case PlayerAnimateTrigger.RollOver:
				anim.SetTrigger("Roll Over");
				break;
			case PlayerAnimateTrigger.Interaction:
				anim.SetTrigger("Interaction");
				break;
			case PlayerAnimateTrigger.GetFrontHit:
				anim.SetTrigger("Get Front Hit");
				break;
			case PlayerAnimateTrigger.GetBackHit:
				anim.SetTrigger("Get Back Hit");
				break;
			case PlayerAnimateTrigger.Die:
				anim.SetTrigger("Die");
				break;
		}
	}

	public bool isAnimating(CurrentPlayerAnimate ani)
	{
		switch (ani)
		{
			case CurrentPlayerAnimate.Attacking:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.Melee") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack.Range"))
					return true;
				break;
			case CurrentPlayerAnimate.Climbing:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Climbing"))
					return true;
				break;
			case CurrentPlayerAnimate.Falling:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Falling"))
					return true;
				break;
			case CurrentPlayerAnimate.Idle:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
					return true;
				break;
			case CurrentPlayerAnimate.Interacting:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Interaction"))
					return true;
				break;
			case CurrentPlayerAnimate.Jumping:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Jumping"))
					return true;
				break;
			case CurrentPlayerAnimate.RollingOver:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Rolling Over"))
					return true;
				break;
			case CurrentPlayerAnimate.Walking:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walking"))
					return true;
				break;
			case CurrentPlayerAnimate.GetHit:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Hit Front") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Hit Back"))
					return true;
				break;
			case CurrentPlayerAnimate.Die:
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die"))
					return true;
				break;
		}

		return false;
	}


}
