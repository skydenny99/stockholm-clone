using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 맵 에디터 격자의 색상을 설정할 수 있는 윈도우입니다.
/// </summary>
public class GridWindow : EditorWindow
{
	public Grid grid;

	/// <summary>
	/// 윈도우를 초기화합니다.
	/// </summary>
	public void Init()
	{
		grid = FindObjectOfType<Grid>();
	}

	// Unity Built-in Method
	public void OnGUI()
	{
		grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
	}
}
