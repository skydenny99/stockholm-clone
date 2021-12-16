using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
	[Serializable]
	public struct SFX
	{
		public string name;
		public AudioClip clip;
	}

	public SFX[] SFXs;
	private Dictionary<string, AudioClip> SFXs_ = new Dictionary<string, AudioClip>();

	void Awake()
	{
		for (int i = 0; i < SFXs.Length; i++)
		{
			SFXs_[SFXs[i].name] = SFXs[i].clip;
		}
	}

	public void PlaySFX(string name)
	{
		if (!SFXs_.ContainsKey(name))
		{
			Debug.LogError("Cannot find SFX " + name);
			return;
		}
		
		AudioSource.PlayClipAtPoint(SFXs_[name], this.transform.position);
	}
}
