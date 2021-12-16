using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Campfire : MonoBehaviour, INPC
{
	public HitboxArea healArea;
	public HitboxArea detectArea;
	public HitboxArea attackArea;

	private bool isEnterPlayer_ = false;
	private int hp = 5;
	private bool isAlive_ = true;
	private GameObject player_;
	public GameObject fireEffect;
	private CampfireEffect effect_;

	// ICreature 구현
	public void attack(bool isMelee)
	{
	}
	// ICreature 구현
	public void die()
	{
		isAlive_ = false;
		ModuleManager.GetInstance().GetModule<GameModule>().GameOver("모닥불이 꺼졌습니다.");
	}
	// ICreature 구현
	public void getDamaged(Transform t, float dmg, float knockBack)
	{
	}
	// ICreature 구현
	public void playKnockBack(Transform t, float knockBack)
	{
	}
	// ICreature 구현
	public void move(float axis)
	{
	}

	// IInteractable 구현
	public void Interact(InteractMessage message, params object[] param)
	{
	}

	// Unity 내장 함수
	void Start()
	{
		healArea.EnterGameObjectEvent += DetectPlayer;
		healArea.ExitGameObjectEvent += ExitPlayer;
		detectArea.EnterGameObjectEvent += DetectMonster;

		player_ = ModuleManager.GetInstance().GetModule<GameModule>().player;
		effect_ = fireEffect.GetComponent<CampfireEffect>();
	}

	// Unity 내장 함수
	void OnDestroy()
	{
		healArea.EnterGameObjectEvent -= DetectPlayer;
		healArea.ExitGameObjectEvent -= ExitPlayer;
		detectArea.EnterGameObjectEvent -= DetectMonster;
	}

	// Unity 내장 함수
	private float healTimer_ = 0;
	void Update()
	{
		if (!isAlive_)
			return;

		// TODO: 낮에만 힐되게
		if (isEnterPlayer_ == true)
		{
			healTimer_ += Time.deltaTime;
			if(healTimer_ > 1)
			{
				player_.GetComponentInChildren<Player>().hp += 10;
				healTimer_ = 0;
			}
		}
	}

	private void DetectPlayer(GameObject target)
	{
		isEnterPlayer_ = true;
		GameModule gameModule = ModuleManager.GetInstance().GetModule<GameModule>();
		if (gameModule.monsterList.Count == 0)
		{
			gameModule.StageClear();
		}
	}
	private void ExitPlayer(GameObject target)
	{
		isEnterPlayer_ = false;
	}

	private void DetectMonster(GameObject target)
	{
		if (!isAlive_)
			return;

		GameObject[] monsters = attackArea.GetAllGameObjectsInArea();

		monsters.Select(monster => monster.GetComponentInChildren<Legarcy.MonsterLegacy>()).ToList().ForEach(monster => monster.die());

		hp -= 1;
		if (hp == 0)
			die();

		effect_.ShowAttackMotion();
		effect_.SetFireStrength((float)hp / 5);
	}
}
