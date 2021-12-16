using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사용자 입력과 출력을 관리하는 모듈입니다.
/// </summary>
public class InputModule : MonoBehaviour, IModule
{
	public List<Axis> axes;
	public enum AxisKeyType
	{
		Positive,
		Negative
	}

	private DataModule dataModule_;
	private bool isRunning = false;

	// IModule definition
	public bool InitModule()
	{
		dataModule_ = ModuleManager.GetInstance().GetModule<DataModule>();
		Debug.Assert(dataModule_);

		//dataModule_.LoadDataFromStorage<KeymapData>();
		//axes = dataModule_.GetData<KeymapData>().axes;
		// TODO 테스트를 위해 임시적으로 주석처리. 초기값 관련해서 다시한번 얘기 필요

		return (dataModule_ != null);
	}

	// IModule definition
	public bool RunModule()
	{
		isRunning = true;
		return true;
	}

	// IModule definition
	public void StopModule()
	{
		isRunning = false;
	}

	// IModule definition
	public void DestroyModule()
	{
		dataModule_.GetData<KeymapData>().axes = axes;
		Debug.Assert(dataModule_.SaveDataToStorage<KeymapData>());
	}

	/// <summary>
	/// 해당하는 입력 값을 -1.0f ~ 1.0f 내에서 가져옵니다.
	/// </summary>
	/// <param name="name">Axis의 name입니다.</param>
	/// <returns>Axis의 value를 -1.0f ~ 1.0f 사이로 반환합니다</returns>
	public float GetAxis(string name)
	{
		return GetAxisObject(name).value;
	}

	public bool GetKey(string name, AxisKeyType keyType = AxisKeyType.Positive)
	{
		Axis axis = GetAxisObject(name);
		if (keyType == AxisKeyType.Positive)
		{
			return Input.GetKey(axis.positiveButton) || Input.GetKey(axis.alternativePositiveButton);
		}
		else if (keyType == AxisKeyType.Negative)
		{
			return Input.GetKey(axis.negativeButton) || Input.GetKey(axis.alternativeNegativeButton);
		}

		return false;
	}

	public bool GetKeyDown(string name, AxisKeyType keyType = AxisKeyType.Positive)
	{
		Axis axis = GetAxisObject(name);
		if (keyType == AxisKeyType.Positive)
		{
			return Input.GetKeyDown(axis.positiveButton) || Input.GetKeyDown(axis.alternativePositiveButton);
		}
		else if (keyType == AxisKeyType.Negative)
		{
			return Input.GetKeyDown(axis.negativeButton) || Input.GetKeyDown(axis.alternativeNegativeButton);
		}

		return false;
	}

	public bool GetKeyUp(string name, AxisKeyType keyType = AxisKeyType.Positive)
	{
		Axis axis = GetAxisObject(name);
		if (keyType == AxisKeyType.Positive)
		{
			return Input.GetKeyUp(axis.positiveButton) || Input.GetKeyUp(axis.alternativePositiveButton);
		}
		else if (keyType == AxisKeyType.Negative)
		{
			return Input.GetKeyUp(axis.negativeButton) || Input.GetKeyUp(axis.alternativeNegativeButton);
		}

		return false;
	}
	
	private Axis GetAxisObject(string name)
	{
		return axes.Find(value => value.name.Equals(name));
		// TODO : Hashmap으로 변경 (프레임 단위 호출이 잦으므로, 성능상 문제가 생길 수 있음)
	}

	// Unity Built-in Method
	void Update()
	{
		if (isRunning)
			axes.ForEach(axis => axis.updateValue(Time.deltaTime));
	}
}

