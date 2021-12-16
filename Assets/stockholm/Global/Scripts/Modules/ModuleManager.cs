using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 각종 모듈들을 초기화하고 관리하는 클래스입니다.
/// </summary>
public class ModuleManager : MonoBehaviour
{
	public List<GameObject> modules = new List<GameObject>();

	private List<IModule> moduleList_ = new List<IModule>();
	private static ModuleManager manager_;

	public static ModuleManager GetInstance()
	{
		return manager_;
	}	

	/// <summary>
	/// 등록된 모든 모듈에 대해 InitModule()을 호출합니다.
	/// </summary>
	public void InitModules()
	{
		modules = modules
				.Select(module => Instantiate(module, this.transform))
				.ToList();

		moduleList_ = modules
				.Select(module => module.GetComponent<IModule>())
				.Where(module => module != null)
				.ToList();
		//TODO : 모듈이 false 리턴시 Destroy하도록

		moduleList_ = moduleList_
				.Where(module => module.InitModule())
				.ToList();

		UpdateInspectorDisplayedModules();
	}

	/// <summary>
	/// 등록된 모든 모듈에 대해 StartModule()을 호출합니다.
	/// </summary>
	public void RunModules()
	{
		moduleList_ = moduleList_
				.Where(module => module.RunModule())
				.ToList();

		UpdateInspectorDisplayedModules();
	}

	/// <summary>
	/// 등록된 모든 모듈에 대해 StopModule()을 호출합니다.
	/// </summary>
	public void StopModules()
	{
		moduleList_.Reverse<IModule>().ToList().ForEach(module => module.StopModule());
	}

	/// <summary>
	/// 등록된 모든 모듈에 대해 DestroyModule()을 호출합니다.
	/// </summary>
	public void DestroyModules()
	{
		var reversedModuleList = moduleList_.Reverse<IModule>().ToList();

		reversedModuleList.ForEach(module => module.DestroyModule());
		reversedModuleList.Cast<MonoBehaviour>().ToList().ForEach(module => Destroy(module.gameObject));

		UpdateInspectorDisplayedModules();
	}

	private void UpdateInspectorDisplayedModules()
	{
		modules = moduleList_.Cast<MonoBehaviour>().ToList().Select(module => module.gameObject).ToList();
	}

	/// <summary>
	/// 조건을 만족하는 첫번째 모듈을 반환합니다.
	/// </summary>
	/// <typeparam name="T">모듈 타입</typeparam>
	/// <returns></returns>
	public T GetModule<T>() where T : class, IModule
	{
		return moduleList_.Find(module => module is T) as T;
	}

	/// <summary>
	/// 조건을 만족하는 모듈을 모두 반환합니다.
	/// </summary>
	/// <typeparam name="T">모듈 타입</typeparam>
	/// <returns></returns>
	public T[] GetModules<T>() where T : class, IModule
	{
		return moduleList_.FindAll(module => module is T).Cast<T>().ToArray();
	}

	// Unity Built-in Method
	void Awake()
	{
		if (manager_ != null)
		{
			Destroy(this.gameObject);
			return;
		}
		
		manager_ = this;
		DontDestroyOnLoad(this.gameObject);

		InitModules();
	}

	// Unity Built-in Method
	void Start()
	{
		RunModules();
	}

	// Unity Built-in Method
	void OnDestroy()
	{
		StopModules();
		DestroyModules();
	}
}
