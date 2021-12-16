using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CompanionAnimator : MonoBehaviour
{
	public enum AnimationTrigger
	{
		dayStart,
		nightStart,
		useBlowSkill,
		startWavingHand,
		deadStart
	};
	public enum AnimationBool
	{
		isCrafting,
		isWalking,
		isStanding,
		isDead,
		isWavingHand,
		isUseBlowSkill
	};

	private Animator animator_;

	protected void Awake()
	{
		animator_ = this.GetComponent<Animator>();
	}

	public void FinishStanding()
	{
		animator_.SetBool("isStanding", true);
		this.transform.parent.GetComponent<Companion>().isStanding = true;
	}

	public void FinishBlowSkill()
	{
		animator_.SetBool("isUseBlowSkill", false);
		this.transform.parent.GetComponent<Companion>().isUseBlowSkill = false;
	}

	public void SetTrigger(AnimationTrigger trigger)
	{
		switch (trigger)
		{
			case AnimationTrigger.dayStart:
				animator_.SetTrigger("dayStart");
				break;

			case AnimationTrigger.nightStart:
				animator_.SetTrigger("nightStart");
				break;

			case AnimationTrigger.useBlowSkill:
				animator_.SetTrigger("useBlowSkill");
				break;

			case AnimationTrigger.startWavingHand:
				animator_.SetTrigger("startWavingHand");
				break;

			case AnimationTrigger.deadStart:
				animator_.SetTrigger("deadStart");
				break;

			default:
				break;
		}
	}

	public void SetBool(AnimationBool aniBool, bool value)
	{
		switch (aniBool)
		{
			case AnimationBool.isCrafting:
				animator_.SetBool("isCrafting", value);
				break;

			case AnimationBool.isWalking:
				animator_.SetBool("isWalking", value);
				break;

			case AnimationBool.isStanding:
				animator_.SetBool("isStanding", value);
				break;

			case AnimationBool.isDead:
				animator_.SetBool("isDead", value);
				break;

			case AnimationBool.isWavingHand:
				animator_.SetBool("isWavingHand", value);
				break;

			case AnimationBool.isUseBlowSkill:
				animator_.SetBool("isUseBlowSkill", value);
				break;

			default:
				break;
		}
	}
}
