using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMModule : MonoBehaviour, IModule
{
	[Serializable]
	public struct BGM
	{
		public string key;
		public AudioClip audioClip;
	}

	public BGM[] BGMs;
	private Dictionary<string, AudioClip> BGMs_ = new Dictionary<string, AudioClip>();

	private AudioSource audio_;

	// IModule 구현
	public bool InitModule()
	{
		audio_ = this.GetComponent<AudioSource>();

		for (int i = 0; i < BGMs.Length; i++)
		{
			BGMs_[BGMs[i].key] = BGMs[i].audioClip;
		}
		return true;
	}
	// IModule 구현
	public bool RunModule()
	{
		return true;
	}
	// IModule 구현
	public void StopModule()
	{
	}
	// IModule 구현
	public void DestroyModule()
	{
	}

	/// <summary>
	/// 기존에 재생되던 BGM을 멈추고 새로운 BGM을 재생합니다.
	/// </summary>
	/// <param name="name">새로 재생할 BGM의 이릅입니다.</param>
	public void PlayBGM(string name, bool changeImmediately = false)
	{
		// TODO: 서서히 바뀌도록 구현
		if (!BGMs_.ContainsKey(name))
		{
			Debug.LogError("Cannot find BGM " + name);
			return;
		}

		if(audio_.isPlaying)
			audio_.Stop();

		audio_.clip = BGMs_[name];
		audio_.Play();
	}
}
