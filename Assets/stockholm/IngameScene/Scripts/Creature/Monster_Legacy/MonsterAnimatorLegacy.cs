using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legarcy
{
	[System.Obsolete("이 클래스는 더이상 사용하지 않습니다.")]
	public class MonsterAnimatorLegacy : MonoBehaviour
	{

		public enum MonsterAnimateTrigger
		{
			GetHit,
			Aware,
			Die,
			Attack
		}

		public enum CurrentMonsterAnimate
		{
			Usual_Idle,
			Usual_Walking,
			Aware_Idle,
			Aware_Walking,
			Attacking,
			Die,
			GetHit,
			Finding
		}

		public Animator anim;
		public MonsterLegacy _control;

		// Use this for initialization
		void Start()
		{
			anim = gameObject.GetComponent<Animator>();
		}

		public void attack()
		{
			_control.attack(_control.isMelee);
		}

		public void setSpeed(float speed)
		{
			anim.SetFloat("Speed", speed);
		}
		public void die()
		{
			Destroy(transform.root.gameObject, 1);
		}

		public void resetAllTrigger()
		{
			anim.ResetTrigger("Aware");
			anim.ResetTrigger("Get Hit");
			anim.ResetTrigger("Die");
			anim.ResetTrigger("Attack");
		}

		public void triggerAnimate(MonsterAnimateTrigger tri)
		{
			switch (tri)
			{
				case MonsterAnimateTrigger.Attack:
					anim.SetTrigger("Attack");
					break;
				case MonsterAnimateTrigger.Aware:
					anim.SetTrigger("Aware");
					break;
				case MonsterAnimateTrigger.Die:
					anim.SetTrigger("Die");
					break;
				case MonsterAnimateTrigger.GetHit:
					anim.SetTrigger("Get Hit");
					break;
			}
		}

		public bool isAnimating(CurrentMonsterAnimate ani)
		{
			switch (ani)
			{
				case CurrentMonsterAnimate.Attacking:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aware.Attack"))
						return true;
					break;
				case CurrentMonsterAnimate.Aware_Idle:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aware.Idle"))
						return true;
					break;
				case CurrentMonsterAnimate.Aware_Walking:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Aware.Walk"))
						return true;
					break;
				case CurrentMonsterAnimate.Die:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die"))
						return true;
					break;
				case CurrentMonsterAnimate.Finding:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Find"))
						return true;
					break;
				case CurrentMonsterAnimate.GetHit:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Get Hit"))
						return true;
					break;
				case CurrentMonsterAnimate.Usual_Idle:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Usual.Idle"))
						return true;
					break;
				case CurrentMonsterAnimate.Usual_Walking:
					if (anim.GetCurrentAnimatorStateInfo(0).IsName("Usual.Walk"))
						return true;
					break;
			}
			return false;
		}
	}
}