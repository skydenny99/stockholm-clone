using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneModule : SceneManagerWrapper, IModule
{
	[SerializeField]
	private List<StageData> stageDataList_;
	public List<StageData> stageDataList { get { return stageDataList_; } }

	private StageData currentStage_;
	public StageData currentStage { get { return currentStage_; } }

	public delegate void StageLoaded(StageData stageData, Scene stageScene);
	public event StageLoaded stageLoaded;

	// IModule definition
	public bool InitModule()
	{
		return true;
	}

	// IModule definition
	public bool RunModule()
	{
		AttatchEvent();
		sceneLoaded += OnSceneLoaded;
		return true;
	}

	// IModule definition
	public void StopModule()
	{
		DettatchEvent();
		sceneLoaded -= OnSceneLoaded;
	}

	// IModule definition
	public void DestroyModule()
	{
	}


	private StageData nowLoadingStageData_;
	public void LoadStage(int index)
	{
		Debug.Assert(stageDataList_.Count > index && index >= 0);
		nowLoadingStageData_ = stageDataList_[index];
		
		LoadScene(nowLoadingStageData_.sceneName);
	}

	public void LoadStage(string stageName)
	{
		int index = stageDataList_.FindIndex(stage => stage.name.Equals(stageName));
		LoadStage(index);
	}

	public void LoadNextStage()
	{
		int nextStageIndex = stageDataList_.FindIndex(stage => stage.name.Equals(currentStage_.name)) + 1;
		LoadStage(nextStageIndex);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (nowLoadingStageData_ == null)
			return;

		currentStage_ = nowLoadingStageData_;
		stageLoaded.Invoke(nowLoadingStageData_, scene);
		nowLoadingStageData_ = null;
	}

	public void ReloadStage()
	{
		LoadStage(currentStage_.sceneName);
	}
}
