using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
	public GameObject monster;
	public int spawnTime;
}

[Serializable]
public class Night
{
	public List<Wave> waves;
}

public class MonsterSpawner : MonoBehaviour, ISpawner {
	[SerializeField]
	private List<Night> pattern_;

	private Night currentNight_;
	private GameModule gameModule_;
	private bool isSpawning_ = false;

	void Start () {
		pattern_.ForEach(night => night.waves.Sort((lwave, rwave) => lwave.spawnTime - rwave.spawnTime));
		gameModule_ = ModuleManager.GetInstance().GetModule<GameModule>();
		gameModule_.nightStarted += OnNightStarted;
		gameModule_.dayStarted += OnDayStarted;

		this.GetComponent<Renderer>().enabled = false;
	}

	private void OnDayStarted(int count)
	{
		isSpawning_ = false;
	}

	private void OnNightStarted(int count)
	{
		if (pattern_.Count <= 0)
			return;

		currentNight_ = pattern_[Mathf.Clamp(count-1, 0, pattern_.Count-1)];
		// 패턴이 끝났을 경우, 마지막 패턴을 반복한다.
		if (currentNight_.waves.Count <= 0)
			return;

		currentSpawnIdx_ = 0;
		isSpawning_ = true;
	}

	private int currentSpawnIdx_;

	void Update () {
		if(isSpawning_)
		{
			if(gameModule_.currentNightTime > currentNight_.waves[currentSpawnIdx_].spawnTime)
			{
				Legarcy.MonsterLegacy monster = Instantiate(currentNight_.waves[currentSpawnIdx_].monster, this.transform.position, Quaternion.identity).GetComponentInChildren<Legarcy.MonsterLegacy>();
				if (gameModule_.isRedMoon)
					monster.setRedMoonReinforce();

				currentSpawnIdx_++;
				if (currentSpawnIdx_ >= currentNight_.waves.Count)
				{
					isSpawning_ = false;
				}
			}
		}
	}

	void OnDestroy()
	{
		gameModule_.nightStarted -= OnNightStarted;
		gameModule_.dayStarted -= OnDayStarted;
	}
}
