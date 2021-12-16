using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

/// <summary>
/// MonoBehavior를 상속받는 SceneManager의 Wrapper 클래스입니다.
/// </summary>
public class SceneManagerWrapper : MonoBehaviour
{
	public int sceneCount { get { return SceneManager.sceneCount; } }
	public int sceneCountInBuildSettings { get { return SceneManager.sceneCountInBuildSettings; } }
	
	public event UnityAction<Scene, LoadSceneMode> sceneLoaded;

	public Scene CreateScene(string sceneName) { return SceneManager.CreateScene(sceneName); }
	public Scene GetActiveScene() { return SceneManager.GetActiveScene(); }
	public Scene GetSceneAt(int index) { return SceneManager.GetSceneAt(index); }
	public Scene GetSceneByBuildIndex(int buildIndex) { return SceneManager.GetSceneByBuildIndex(buildIndex); }
	public Scene GetSceneByName(string name) { return SceneManager.GetSceneByName(name); }
	public Scene GetSceneByPath(string scenePath) { return SceneManager.GetSceneByPath(scenePath); }
	public void LoadScene(string sceneName) { SceneManager.LoadScene(sceneName); }
	public void LoadScene(int sceneBuildIndex) { SceneManager.LoadScene(sceneBuildIndex); }
	public void LoadScene(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode) { SceneManager.LoadScene(sceneName, mode); }
	public void LoadScene(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode) { SceneManager.LoadScene(sceneBuildIndex, mode); }
	public AsyncOperation LoadSceneAsync(string sceneName) { return SceneManager.LoadSceneAsync(sceneName); }
	public AsyncOperation LoadSceneAsync(int sceneBuildIndex) { return SceneManager.LoadSceneAsync(sceneBuildIndex); }
	public AsyncOperation LoadSceneAsync(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode) { return SceneManager.LoadSceneAsync(sceneBuildIndex, mode); }
	public AsyncOperation LoadSceneAsync(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode) { return SceneManager.LoadSceneAsync(sceneName, mode); }
	public void MergeScenes(Scene sourceScene, Scene destinationScene) { SceneManager.MergeScenes(sourceScene, destinationScene); }
	public void MoveGameObjectToScene(GameObject go, Scene scene) { SceneManager.MoveGameObjectToScene(go, scene); }
	public bool SetActiveScene(Scene scene) { return SceneManager.SetActiveScene(scene); }
	public AsyncOperation UnloadSceneAsync(string sceneName) { return SceneManager.UnloadSceneAsync(sceneName); }
	public AsyncOperation UnloadSceneAsync(Scene scene) { return SceneManager.UnloadSceneAsync(scene); }
	public AsyncOperation UnloadSceneAsync(int sceneBuildIndex) { return SceneManager.UnloadSceneAsync(sceneBuildIndex); }

	/// <summary>
	/// Scene 관련 이벤트 입력을 받습니다.
	/// </summary>
	public void AttatchEvent()
	{
		SceneManager.sceneLoaded += (arg0, arg1) => sceneLoaded.Invoke(arg0, arg1);
	}

	/// <summary>
	/// Scene 관련 이벤트 입력을 더이상 받지 않습니다.
	/// </summary>
	public void DettatchEvent()
	{
		SceneManager.sceneLoaded -= (arg0, arg1) => sceneLoaded.Invoke(arg0, arg1);
	}
}
