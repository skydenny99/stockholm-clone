using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameModule : MonoBehaviour, IModule
{
	[SerializeField]
	private float redmoonPoint_ = 0;
	public float redmoonPoint
	{
		get
		{
			return redmoonPoint_;
		}
		set
		{
			redmoonPoint_ = Mathf.Clamp(value, 0, maxRedmoonPoint_);
			uiModule_.playerInfoUI.redMoonBar.currentValue = (int)redmoonPoint_;

			if(redmoonPoint_ >= maxRedmoonPoint_)
			{
				weatherModule_.dayNightCycle.setRedMoon();
				redmoonOnCurrentNight_ = true;
			}
		}
	}

	[SerializeField]
	private float maxRedmoonPoint_ = 10000;
	public float maxRedmoonPoint
	{
		get
		{
			return maxRedmoonPoint_;
		}
		set
		{
			maxRedmoonPoint_ = value;
			redmoonPoint_ = Mathf.Clamp(redmoonPoint_, 0, maxRedmoonPoint_);
			uiModule_.playerInfoUI.redMoonBar.currentValue = (int)redmoonPoint_;
			uiModule_.playerInfoUI.redMoonBar.maxValue = (int)maxRedmoonPoint_;
		}
	}

	[SerializeField]
	private bool redmoonOnCurrentNight_ = false;

	[SerializeField]
	private bool isRedMoon_ = false;
	public bool isRedMoon
	{
		get {
			return isRedMoon_;
		}
	}

	[SerializeField]
	private GameObject playerPrefab_;
	private GameObject currentPlayer_;
	public GameObject player { get { return currentPlayer_; } }

	[SerializeField]
	private GameObject companionPrefab_;
	private GameObject currentCompanion_;
	public GameObject companion { get { return currentCompanion_; } }

	[SerializeField]
	private GameObject campfirePrefab_;
	private GameObject currentCampfire_;
	public GameObject campfire { get { return currentCampfire_; } }

	[SerializeField]
	private bool isGameRunning_ = false;
	public bool isGameRunning { get { return isGameRunning_; } }

	public bool isDay { get { return isDay_; } }
	public bool isNight { get { return !isDay_; } }
	private bool isDay_;

	private float gamePlayTime_;
	public float gamePlayTime { get { return gamePlayTime_; } }
	private float stagePlayTime_;
	public float stagePlayTime { get { return stagePlayTime_; } }

	private float totalDayTime_;
	public float totalDayTime { get { return totalDayTime_; } }
	private float totalNightTime_;
	public float totalNightTime { get { return totalNightTime_; } }

	[SerializeField]
	private float currentDayTime_;
	public float currentDayTime { get { return currentDayTime_; } }
	[SerializeField]
	private float currentNightTime_;
	public float currentNightTime { get { return currentNightTime_; } }

	private int dayCount_;
	public int dayCount { get { return dayCount_; } }
	private int nightCount_;
	public int nightCount { get { return nightCount_; } }

	public delegate void DayNightStarted(int count);
	public event DayNightStarted dayStarted;
	public event DayNightStarted nightStarted;

	public string startStage;
	public bool debugging;

	private SceneModule sceneModule_;
	private IngameUIModule uiModule_;
	private WeatherModule weatherModule_;
	private BGMModule bgmModule_;

	private StageData currentStageData_;

	public List<Legarcy.MonsterLegacy> monsterList;

	// IModule definition
	public bool InitModule()
	{
		// TODO: 데이터모듈에서 게임 진행데이터 불러와야 함.

		sceneModule_ = ModuleManager.GetInstance().GetModule<SceneModule>();
		Debug.Assert(sceneModule_ != null);

		uiModule_ = ModuleManager.GetInstance().GetModule<IngameUIModule>();
		Debug.Assert(uiModule_ != null);

		weatherModule_ = ModuleManager.GetInstance().GetModule<WeatherModule>();
		Debug.Assert(weatherModule_ != null);

		bgmModule_ = ModuleManager.GetInstance().GetModule<BGMModule>();
		Debug.Assert(bgmModule_ != null);


		if (debugging)
		{
			//startStage = sceneModule_.stageDataList.Find(stage => stage.sceneName.Equals(sceneModule_.GetActiveScene().name)).name;
		}
		else if (String.IsNullOrEmpty(startStage))
		{
			startStage = sceneModule_.stageDataList[0].name;
		}

		uiModule_.playerInfoUI.redMoonBar.currentValue = (int)redmoonPoint_;
		uiModule_.playerInfoUI.redMoonBar.maxValue = (int)maxRedmoonPoint_;

		return true;
	}

	// IModule definition
	public bool RunModule()
	{
		sceneModule_.stageLoaded += OnStageLoaded;
		sceneModule_.LoadStage(startStage);
		return true;
	}

	// IModule definition
	public void StopModule()
	{
		sceneModule_.stageLoaded -= OnStageLoaded;
		if (isGameRunning_)
		{
			StopGame();
			DestroyGame();
		}
	}

	// IModule definition
	public void DestroyModule()
	{

	}

	private void OnStageLoaded(StageData stageData, Scene stageScene)
	{
		InitGame(stageData);
		StartGame();
	}

	// Unity Built-in Method
	void Update()
	{
		if (isGameRunning_)
		{
			#if DEBUG
			if (Input.GetKey (KeyCode.Slash)) 
			{
				Time.timeScale = 20;
			}
			else
			{
				Time.timeScale = 1;
			}
			#endif

			bool dayChangedCheck = !weatherModule_.dayNightCycle.isDayCycle(DayNight.DayCycle.night);
			if (isDay_ != dayChangedCheck)
			{
				isDay_ = dayChangedCheck;
				if (isDay_)
				{
					currentDayTime_ = 0;
					dayCount_++;
					if (dayStarted != null)
						dayStarted.Invoke(dayCount_);

					uiModule_.ShowDayStart();
					uiModule_.timerUI.ChangeState(TimerUI.State.Day);
					bgmModule_.PlayBGM("Stage1");

					if(isRedMoon_)
					{
						redmoonPoint_ = 0;
						isRedMoon_ = false;
					}
				}
				else
				{
					currentNightTime_ = 0;
					nightCount_++;
					if (nightStarted != null)
						nightStarted.Invoke(nightCount_);

					uiModule_.ShowNightStart();
					uiModule_.timerUI.ChangeState(TimerUI.State.Night);
					bgmModule_.PlayBGM("Night");
					if (redmoonOnCurrentNight_)
					{
						isRedMoon_ = true;
						redmoonOnCurrentNight_ = false;
					}
				}
			}

			if (isDay)
			{
				currentDayTime_ += Time.deltaTime;
				totalDayTime_ += Time.deltaTime;
				float daytime = (weatherModule_.dayNightCycle.nightStartTime - weatherModule_.dayNightCycle.morningStartTime) * weatherModule_.dayNightCycle.sec2hour;
				uiModule_.timerUI.ChangeStatePercent(currentDayTime_ / daytime);
			}
			else
			{
				currentNightTime_ += Time.deltaTime;
				totalNightTime_ += Time.deltaTime;
				float nighttime = ((24 -weatherModule_.dayNightCycle.nightStartTime) + weatherModule_.dayNightCycle.morningStartTime) * weatherModule_.dayNightCycle.sec2hour;
				uiModule_.timerUI.ChangeStatePercent(currentNightTime_ / nighttime);
			}
			gamePlayTime_ += Time.deltaTime;
			stagePlayTime_ += Time.deltaTime;
			
			if(!redmoonOnCurrentNight_ && !isRedMoon_)
				redmoonPoint += (currentStageData_.redmoonIncreasementPerSec + player.GetComponentInChildren<Backpack>().artifactItems.Sum(artifact => artifact.item.redMoonEnforcement)) * Time.deltaTime;

			uiModule_.playtimeUI.ChangeTime((int)gamePlayTime_);
		}
	}

	/// <summary>
	/// 게임을 초기화합니다.
	/// </summary>
	private void InitGame(StageData stageData)
	{
		Debug.Assert(!isGameRunning_);
		currentStageData_ = stageData;

		redmoonPoint_ = 0;

		totalDayTime_ = 0;
		totalNightTime_ = 0;
		currentDayTime_ = 0;
		currentNightTime_ = 0;
		stagePlayTime_ = 0;

		dayCount_ = 0;
		nightCount_ = 0;
		
		if(debugging)
		{
			currentPlayer_ = GameObject.Find("Player");
			currentCompanion_ = GameObject.Find("Companion");
			currentCampfire_ = GameObject.Find("Campfire");
		}
		
		if(currentPlayer_ == null)
			currentPlayer_ = Instantiate(playerPrefab_, stageData.startPosition + new Vector2(-1, 0), Quaternion.identity);

		if (currentCompanion_ == null)
			currentCompanion_ = Instantiate(companionPrefab_, stageData.startPosition + new Vector2(1, -0.6f), Quaternion.identity);

		if (currentCampfire_ == null)
			currentCampfire_ = Instantiate(campfirePrefab_, stageData.startPosition + new Vector2(1, -0.9f), Quaternion.identity);
	}

	/// <summary>
	/// 게임을 시작합니다.
	/// </summary>
	private void StartGame()
	{
		Debug.Assert(!isGameRunning_);

		bgmModule_.PlayBGM("Stage1");

		isDay_ = !weatherModule_.dayNightCycle.isDayCycle(DayNight.DayCycle.night);
		isDay_ = !isDay_;
		// Update함수의 isDay_ 분기 한번 타도록 하기위해 하드코딩

		isGameRunning_ = true;
	}

	/// <summary>
	/// 게임을 일시정지합니다.
	/// </summary>
	private void PauseGame()
	{
		Debug.Assert(isGameRunning_);
	}

	/// <summary>
	/// 게임을 중단합니다.
	/// </summary>
	private void StopGame()
	{
		Debug.Assert(isGameRunning_);

		isGameRunning_ = false;
	}

	/// <summary>
	/// 게임을 저장하고 해제합니다.
	/// </summary>
	public void DestroyGame()
	{
		Debug.Assert(!isGameRunning_);
	}

	public void RestartGame()
	{
		sceneModule_.LoadStage(sceneModule_.currentStage.name);
	}

	public void StageClear()
	{
		sceneModule_.LoadNextStage();
	}

	public void GameOver()
	{
		GameOver("");
	}

	public void GameOver(string reason)
	{
		Debug.Assert(isGameRunning_);

		bgmModule_.PlayBGM("GameOver");
		isGameRunning_ = false;
		ModuleManager.GetInstance().GetModule<IngameUIModule>().ShowGameOver();
		Invoke("RestartGame", 5f);
	}
}
