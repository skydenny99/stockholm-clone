using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData {
	[SerializeField]
	private string name_;
	public string name
	{
		get { return name_; }
	}

	[SerializeField]
	private Vector2 startPosition_;
	public Vector2 startPosition
	{
		get { return startPosition_; }
	}

	[SerializeField]
	private int redmoonIncreasementPerSec_;
	public int redmoonIncreasementPerSec
	{
		get { return redmoonIncreasementPerSec_; }
	}

	[SerializeField]
	private bool isBossStage_;
	public bool isBossStage
	{
		get { return isBossStage; }
	}

	[SerializeField]
	private string sceneName_;
	public string sceneName
	{
		get { return sceneName_; }
	}
}
