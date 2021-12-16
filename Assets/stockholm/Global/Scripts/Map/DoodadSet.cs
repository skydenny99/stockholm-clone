using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장식 묶음을 저장하는 애셋입니다.
/// </summary>
public class DoodadSet : ScriptableObject
{
	public Transform[] prefabs = new Transform[0];
}
