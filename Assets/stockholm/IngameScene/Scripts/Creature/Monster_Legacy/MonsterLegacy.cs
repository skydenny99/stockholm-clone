using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legarcy
{
	[System.Obsolete("이 클래스는 더이상 사용하지 않습니다.")]
	public class MonsterLegacy : MonoBehaviour, ILegacyCreature
	{

		//기본 정보
		public string mobName = "Name";
		public float currentHP = 100f;
		public float maxHP = 100f;
		public float moveSpeedNormal = 0.5f;
		public float moveSpeedAware = 0.5f;

		//공격관련 정보
		public bool isMelee = true;
		public GameObject attackType;
		//private Weapon weapon;
		public GameObject arrow;

		//밀려나는 거리의 배율
		public float knockBackDist = 0.2f;

		//센서 관련 정보
		public MonsterSensorLegacy sensor;

		public bool isAware = false;
		public GameObject Player;
		public GameObject mobAnim;
		private MonsterAnimatorLegacy _anim;
		private Rigidbody2D _rig;
		private Vector3 _lookDirection;
		private Vector2 _currentKnockback;
		public bool isDead = false;
		public bool isNight = false;

		//RedMoon
		public bool isRedMoonMob = false;
		public float reinforceRate = 1.2f;

		public int level = 1;
		public float damageIncreasement = 0;
		public float hpIncreasement = 0;

		// 가지고 있는 아이템
		private Item havingItem_;
		public Item havingItem
		{
			set
			{
				havingItem_ = value;
				value.transform.parent = this.transform;
			}
		}

		private GameModule gameModule_;

		// Use this for initialization
		void Start()
		{
			gameModule_ = ModuleManager.GetInstance().GetModule<GameModule>();
			gameModule_.monsterList.Add(this);
			if (Player == null)
				Player = gameModule_.player;
			//weapon = attackType.GetComponent<Weapon>();

			//weapon.damageCalculate = damage => (damage + (level - 1) * damageIncreasement) * (isRedMoonMob ? reinforceRate : 1);
			// 공격력 계산 방식

			maxHP += (level - 1) * hpIncreasement;
			currentHP += (level - 1) * hpIncreasement;
			// todo 몬스터 레벨이 시작시에 정해진다는 가정하에 짜인 코드, 변경필요

			_anim = mobAnim.GetComponent<MonsterAnimatorLegacy>();
			_lookDirection = transform.root.localScale;
			_rig = transform.root.GetComponent<Rigidbody2D>();
			Debug.Assert(_anim);
			Debug.Assert(sensor);


			if (gameModule_.isDay && isNight)
				die();

			gameModule_.dayStarted += OnDayStarted;

			//Debug
			//havingItem = ModuleManager.GetInstance().GetModule<ItemModule>().GetConsumeItem("item_healing_01");
		}

		void OnDestroy()
		{
			gameModule_.monsterList.Remove(this);
			gameModule_.dayStarted -= OnDayStarted;
		}

		private void OnDayStarted(int count)
		{
			if (isNight)
				die();
		}

		public void attack(bool isMelee = true)
		{
		/*
			if (isMelee)
				weapon.Attack();
			else
			{
				(weapon as RangeWeapon).attack(arrow, (Vector2.left * transform.root.localScale.x).normalized);
			}*/
		}

		public void die()
		{
			isDead = true;
			//transform.root.GetComponent<BoxCollider2D>().
			_anim.triggerAnimate(MonsterAnimatorLegacy.MonsterAnimateTrigger.Die);

			if (havingItem_ != null)
			{
				DropItem dropItem = ModuleManager.GetInstance().GetModule<ItemModule>().GetDropItem(havingItem_);
				dropItem.ShowDropItem(this.transform.position);
			}
		}

		/// <summary>
		/// TODO : 몬스터 자살 구현
		/// </summary>
		/// <param name="t"></param>
		/// <param name="dmg"></param>
		/// <param name="knockBack"></param>
		public void getDamaged(Transform t, float dmg, float knockBack)
		{
			if (dmg == -1)
			{
				Debug.Log("모닥불 주거라 으어어어어");
				die();
				return;
			}

			currentHP -= dmg;

			if (currentHP <= 0)
			{
				if (isDead)
					return;
				die();
				return;
			}

			if (!isAware)
				foundPlayer();
			if (!_anim.isAnimating(MonsterAnimatorLegacy.CurrentMonsterAnimate.Attacking))
				playKnockBack(t, knockBack);
		}

		public void knockback()
		{
			_rig.velocity = _currentKnockback;
		}

		public void playKnockBack(Transform t, float knockBack)
		{
			float dist = t.position.x - transform.root.position.x;
			if (transform.root.localScale.x * dist > 0)
			{
				changeDirection();
			}
			_anim.triggerAnimate(MonsterAnimatorLegacy.MonsterAnimateTrigger.GetHit);
			_currentKnockback = (Vector2.left * dist).normalized * knockBack * knockBackDist;
		}

		public void move(float axis)
		{

			if (!isAware)
				_rig.velocity = new Vector2(moveSpeedNormal * axis, _rig.velocity.y);
			else
				_rig.velocity = new Vector2(moveSpeedAware * axis, _rig.velocity.y);
		}

		public void changeDirection()
		{
			if (transform.parent.localScale.x > 0)
				transform.parent.localScale = new Vector3(-_lookDirection.x, _lookDirection.y, _lookDirection.z);
			else
				transform.parent.localScale = _lookDirection;

		}

		public void findingPlayer()
		{
			if (Player == null)
				Player = ModuleManager.GetInstance().GetModule<GameModule>().player;// GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;

			if (sensor.findPlayer())
			{
				if (transform.root.localScale.x * (Player.transform.position.x - transform.position.x) > 0)
				{
					changeDirection();
				}
				if (!isAware)
				{
					foundPlayer();
				}
			}
		}

		public bool checkFrontGround()
		{
			return sensor.checkFrontGround();
		}
		/// <summary>
		/// TODO : 다음 낮이 되고 눈에 사람들이 없으면 isAware를 false로 만듬.
		/// </summary>
		public void foundPlayer()
		{
			isAware = true;
			sensor.detectRange = Mathf.Infinity;
			sensor.detectBehindMinRange *= 2;
			_anim.triggerAnimate(MonsterAnimatorLegacy.MonsterAnimateTrigger.Aware);
		}

		public void setRedMoonReinforce()
		{
			if (isRedMoonMob)
				return;

			maxHP *= reinforceRate;
			currentHP *= reinforceRate;
			moveSpeedAware *= reinforceRate;
			moveSpeedNormal *= reinforceRate;
			// todo 이거 강화옵션마다 값이 다름, movespeed = 1.1

			isRedMoonMob = true;
		}

		public void resetRedMoonReinforce()
		{
			if (!isRedMoonMob)
				return;

			maxHP /= reinforceRate;
			currentHP /= reinforceRate;
			moveSpeedAware /= reinforceRate;
			moveSpeedNormal /= reinforceRate;

			isRedMoonMob = false;
		}

	}
}