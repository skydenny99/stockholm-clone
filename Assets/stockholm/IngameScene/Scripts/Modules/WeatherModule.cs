using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherModule : MonoBehaviour, IModule
{
	[SerializeField]
	private GameObject dayNightCyclePrefab_;
	private GameObject dayNightCycle_;
	public DayNight dayNightCycle
	{
		get { return dayNightCycle_.GetComponent<DayNight>(); }
	}

	[SerializeField]
	private GameObject climatePrefab_;
	private GameObject climate_;
	public Climate climate
	{
		get { return climate_.GetComponent<Climate>(); }
	}

	public bool InitModule()
    {
		return true;
    }
    
    public bool RunModule()
    {
		ModuleManager.GetInstance().GetModule<SceneModule>().stageLoaded += OnStageLoad;
		return true;
    }

	public void StopModule()
    {
		ModuleManager.GetInstance().GetModule<SceneModule>().stageLoaded -= OnStageLoad;
	}

	public void DestroyModule()
	{

	}

	private void OnStageLoad(StageData stageData, UnityEngine.SceneManagement.Scene stageScene)
	{
		climate_ = Instantiate(climatePrefab_);
		dayNightCycle_ = Instantiate(dayNightCyclePrefab_);
		dayNightCycle_.GetComponent<DayNight>().climateObject = climate_;
	}
}
