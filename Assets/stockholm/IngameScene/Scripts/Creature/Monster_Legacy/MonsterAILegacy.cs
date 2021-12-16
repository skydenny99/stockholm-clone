using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legarcy
{
	[System.Obsolete("이 클래스는 더이상 사용하지 않습니다.")]
	public class MonsterAILegacy : MonoBehaviour
	{
		public MonsterAnimatorLegacy anim_;
		public MonsterLegacy control_;
		public GameObject npc_;

		//Idle

		//Move
		public bool goLeft = true;
		public float maxMoveRange = 5;
		private Vector3 spawnPoint;
		private float xAxis = 0;

		//Attack
		public float attackRange = 1f;
		public float attackInterval = 1f;
		private float attackTimer = 0f;

		//Night
		public bool isEogeuroToPlayer = false;
		public float eogeuroRange = 5f;

		private float distFromPlayer = 0f;

		private GameModule gameModule_;
		// Use this for initialization
		void Start()
		{
			gameModule_ = ModuleManager.GetInstance().GetModule<GameModule>();
			npc_ = gameModule_.campfire;
			spawnPoint = transform.position;
			attackTimer = attackInterval;
		}

		// Update is called once per frame
		void Update()
		{
			if (!gameModule_.isGameRunning)
				return;

			distFromPlayer = control_.Player.transform.position.x - transform.root.position.x;
			if (control_.isDead)
				return;

			//낮패턴
			if (!control_.isNight)
			{
				if (!control_.isAware)
				{
					control_.findingPlayer();
					checkWalkPath();
				}
				else
				{
					if (control_.sensor.findPlayer())
					{
						trackPlayer();
					}
					else
					{
						xAxis = 0;
					}
				}

			}
			//밤 패턴
			else
			{
				if (!control_.isAware)
				{
					control_.findingPlayer();
					checkCampfireDir();
				}
				else
				{
					if (Mathf.Abs(distFromPlayer) < eogeuroRange)
					{
						trackPlayer();
					}
					else
					{
						checkCampfireDir();
					}
				}
			}
			//        Debug.Log(xAxis);
			anim_.setSpeed(Mathf.Abs(xAxis));
			controlTimer();
		}

		void FixedUpdate()
		{
			if (!gameModule_.isGameRunning)
				return;

			if (!anim_.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.GetHit))
			{
				if (anim_.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.Usual_Walking) || anim_.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.Aware_Walking))
					control_.move(xAxis);
			}
			else
			{
				control_.knockback();
			}

		}

		void walk()
		{
			xAxis = (Vector2.left * Mathf.Clamp(control_.transform.root.localScale.x, -1, 1)).x;

		}

		void checkWalkPath()
		{
			if (!(goLeft ^ (transform.root.localScale.x > 0)))
			{
				walk();
			}
			else
			{
				control_.changeDirection();
			}

			if ((transform.position - spawnPoint).x >= maxMoveRange / 2)
			{
				goLeft = true;
			}

			if ((transform.position - spawnPoint).x <= -maxMoveRange / 2)
			{
				goLeft = false;
			}
		}

		void checkCampfireDir()
		{
			xAxis = 0;
			float dist = npc_.transform.position.x - transform.root.position.x;
			if (dist * transform.root.localScale.x > 0)
			{
				control_.changeDirection();
			}
			else
			{
				walk();
			}
		}

		void trackPlayer()
		{

			if (control_.sensor.findPlayer())
			{
				if (distFromPlayer * transform.root.localScale.x > 0)
				{
					control_.changeDirection();
				}

				if (Mathf.Abs(distFromPlayer) > attackRange)
				{
					if (!anim_.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.GetHit))
						walk();
				}
				else
				{
					xAxis = 0;
					attackPlayer();
				}
			}
		}

		void attackPlayer()
		{
			if (control_.sensor.findPlayer())
				if (attackTimer > attackInterval)
				{
					anim_.triggerAnimate(MonsterAnimatorLegacy.MonsterAnimateTrigger.Attack);
					attackTimer = 0;
				}

		}

		void controlTimer()
		{
			if (!anim_.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.Attacking))
				attackTimer += Time.deltaTime;
		}
	}
}