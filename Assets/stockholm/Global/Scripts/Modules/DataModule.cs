using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public interface IData
{
}

[Serializable]
public class PlayerData : IData
{
	public uint playTime = 0;
}

[Serializable]
public class OptionData : IData
{
	public bool isFullscreen = true;
	public uint screenWidth = 1920;
	public uint screenHeight = 1080;
}

/// <summary>
/// 유저 데이터의 세이브 및 로드를 관리하는 모듈입니다.
/// </summary>
public class DataModule : MonoBehaviour, IModule
{
	private string dataDirPath_ = "Data";

	private List<IData> datas_;


	// IModule구현
	public bool InitModule()
	{
		datas_ = new List<IData>();
		datas_.Add(new PlayerData());
		datas_.Add(new OptionData());
		datas_.Add(new KeymapData());

		return true;
	}
	// IModule구현
	public bool RunModule()
	{
		return true;
	}
	// IModule구현
	public void StopModule()
	{
	}
	// IModule구현
	public void DestroyModule()
	{
	}

	/// <summary>
	/// 데이터를 가져옵니다.
	/// </summary>
	/// <typeparam name="T">가져올 데이터 타입입니다.</typeparam>
	/// <returns>해당 타입의 데이터를 반환합니다.</returns>
	public T GetData<T>() where T : class, IData
	{
		return datas_.Find(d => d is T) as T;
	}

	/// <summary>
	/// 데이터를 저장소로부터 불러옵니다.
	/// </summary>
	/// <typeparam name="T">불러올 데이터 타입입니다.</typeparam>
	/// <returns>성공적으로 불러왔을 경우 <c>true</c>를, 실패했을경우 <c>false</c>를 반환합니다.</returns>
	public bool LoadDataFromStorage<T>() where T : class, IData
	{
		StringBuilder b = new StringBuilder();
		string typeName = typeof(T).Name;
		b.Append(dataDirPath_).Append("/").Append(typeName).Append(".d");
		string path = b.ToString();

		if (File.Exists(path))
		{
			string json;
			try
			{
				json = File.ReadAllText(path);
			}
			catch (Exception e)
			{
				Debug.Log("Cannot read file " + typeName);
				Debug.Log(e);
				return false;
			}

			IData data = datas_.Find(d => d is T);
			JsonUtility.FromJsonOverwrite(json, data);

			Debug.Log(JsonUtility.ToJson(data));
		} else {
			Debug.Log(typeName + " file not found. Create new one...");
			return SaveDataToStorage<T>();
		}
		return true;
	}

	/// <summary>
	/// 데이터를 저장소에 저장합니다.
	/// </summary>
	/// <typeparam name="T">저장할 데이터 타입입니다.</typeparam>
	/// <returns>성공적으로 불러왔을 경우 <c>true</c>를, 실패했을경우 <c>false</c>를 반환합니다.</returns>
	public bool SaveDataToStorage<T>() where T:class, IData
	{
		if(!Directory.Exists(dataDirPath_)) {
			Debug.Log("'Data' folder not found. Create new one...");
			try
			{
				Directory.CreateDirectory(dataDirPath_);
			}
			catch (Exception e)
			{
				Debug.Log("Cannot create directory 'Data'");
				Debug.Log(e);
				return false;
			}
			Debug.Log("Complete to create 'Data' folder");
		}

		StringBuilder b = new StringBuilder();
		string typeName = typeof(T).Name;
		b.Append(dataDirPath_).Append("/").Append(typeName).Append(".d");
		string path = b.ToString();

		try
		{
			File.WriteAllText(path, JsonUtility.ToJson(datas_.Find(d => d is T)));
		}
		catch (Exception e)
		{
			Debug.Log("Cannot write file " + typeName);
			Debug.Log(e);
			return false;
		}

		Debug.Log("Complete to save " + typeName);
		return true;
	}
}
