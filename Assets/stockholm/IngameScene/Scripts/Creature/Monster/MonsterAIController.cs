using NodeCanvas.BehaviourTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MonsterAIController : MonoBehaviour, IController<Monster>
{
	[SerializeField]
	private Monster targetMonster_;
	public Monster targetMonster { get { return targetMonster_; } set { targetMonster_ = value; } }

	[SerializeField]
	private Creature player_;
	public Creature player { get { return player_; } set { player_ = value; } }

	public Vector2 initPosition;

	private void Awake()
	{
		initPosition = this.transform.position;

		var moduleManager = ModuleManager.GetInstance();
		if (moduleManager == null)
			return;

		var gameModule = moduleManager.GetModule<GameModule>();
		if (gameModule == null)
			return;

		if (gameModule.player == null)
			return;

		player_ = gameModule.player.GetComponent<Player>();
	}
}
