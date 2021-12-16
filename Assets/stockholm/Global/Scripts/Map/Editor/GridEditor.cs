using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

/// <summary>
/// Grid와 관련된 .
/// </summary>
[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
	private Grid grid_;
	private int oldIndex_ = 0;

	// Unity Built-in Method
	public void OnEnable()
	{
		grid_ = (Grid)target;
	}

	/// <summary>
	/// 타일셋 파일을 생성하는 메뉴 아이템입니다.
	/// </summary>
	[MenuItem("Assets/Create/TileSet")]
	public static void CreateTileSet()
	{
		TileSet asset = ScriptableObject.CreateInstance<TileSet>();
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (string.IsNullOrEmpty(path))
		{
			path = "Assets";
		}
		else if (Path.GetExtension(path) != "")
		{
			path = path.Replace(Path.GetFileName(path), "");
		}
		else
		{
			path += "/";
		}

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
		AssetDatabase.CreateAsset(asset, assetPathAndName);
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
		//asset.hideFlags = HideFlags.DontSave;
	}

	/// <summary>
	/// 장식 묶음 파일을 생성하는 메뉴 아이템입니다.
	/// </summary>
	[MenuItem("Assets/Create/DoodadSet")]
	public static void CreateDoodadSet()
	{
		DoodadSet asset = ScriptableObject.CreateInstance<DoodadSet>();
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);

		if (string.IsNullOrEmpty(path))
		{
			path = "Assets";
		}
		else if (Path.GetExtension(path) != "")
		{
			path = path.Replace(Path.GetFileName(path), "");
		}
		else
		{
			path += "/";
		}

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "DoodadSet.asset");
		AssetDatabase.CreateAsset(asset, assetPathAndName);
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
		asset.hideFlags = HideFlags.DontSave;
	}

	// Unity Built-in Method
	/// <summary>
	/// Grid에 대한 Inspector GUI입니다.
	/// </summary>
	public override void OnInspectorGUI()
	{
		grid_.width = CreateSlider("Width", grid_.width);
		grid_.height = CreateSlider("Height", grid_.height);

		if (GUILayout.Button("Open Grid Window"))
		{
			GridWindow window = EditorWindow.GetWindow<GridWindow>();
			window.Init();
		}

		EditorGUI.BeginChangeCheck();
		Transform newObjectPrefab = (Transform)EditorGUILayout.ObjectField("Selected Prefab", grid_.currentPrefab, typeof(Transform), false);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Grid Changed");
			grid_.currentPrefab = newObjectPrefab;
		}


		EditorGUI.BeginChangeCheck();
		var objectType = (Grid.ObjectType)EditorGUILayout.EnumPopup("Object Type", grid_.objectType);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Grid Changed");
			grid_.objectType = objectType;
			if (objectType == Grid.ObjectType.Tile)
			{
				if(grid_.tileSet.prefabs.Length > 0)
					grid_.currentPrefab = grid_.tileSet.prefabs[0];
			}
			else if (objectType == Grid.ObjectType.Doodad)
			{
				if (grid_.doodadSet.prefabs.Length > 0)
					grid_.currentPrefab = grid_.doodadSet.prefabs[0];
			}
		}

		if (objectType == Grid.ObjectType.Tile)
		{
			EditorGUI.BeginChangeCheck();
			var newTileSet = (TileSet)EditorGUILayout.ObjectField("Tileset", grid_.tileSet, typeof(TileSet), false);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Grid Changed");
				grid_.tileSet = newTileSet;
				grid_.currentPrefab = grid_.tileSet.prefabs[0];
			}

			if (grid_.tileSet != null)
			{
				EditorGUI.BeginChangeCheck();
				string[] names = new string[grid_.tileSet.prefabs.Length];
				int[] values = new int[names.Length];

				for (int i = 0; i < names.Length; ++i)
				{
					names[i] = grid_.tileSet.prefabs[i] != null ? grid_.tileSet.prefabs[i].name : "";
					values[i] = i;
				}

				int index = EditorGUILayout.IntPopup("Select Tile", oldIndex_, names, values);

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(target, "Grid Changed");
					if (oldIndex_ != index)
					{
						oldIndex_ = index;
						grid_.currentPrefab = grid_.tileSet.prefabs[index];
					}
				}
			}
		}
		else if (objectType == Grid.ObjectType.Doodad)
		{
			EditorGUI.BeginChangeCheck();
			var newDoodadSet = (DoodadSet)EditorGUILayout.ObjectField("DoodadSet", grid_.doodadSet, typeof(DoodadSet), false);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Grid Changed");
				grid_.doodadSet = newDoodadSet;
				grid_.currentPrefab = grid_.doodadSet.prefabs[0];
			}

			if (grid_.doodadSet != null)
			{
				EditorGUI.BeginChangeCheck();
				string[] names = new string[grid_.doodadSet.prefabs.Length];
				int[] values = new int[names.Length];

				for (int i = 0; i < names.Length; ++i)
				{
					names[i] = grid_.doodadSet.prefabs[i] != null ? grid_.doodadSet.prefabs[i].name : "";
					values[i] = i;
				}

				int index = EditorGUILayout.IntPopup("Select Doodad", oldIndex_, names, values);

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(target, "Grid Changed");
					if (oldIndex_ != index)
					{
						oldIndex_ = index;
						grid_.currentPrefab = grid_.doodadSet.prefabs[index];
					}
				}
			}
		}

	}

	/// <summary>
	/// UI상에 1~100 슬라이더를 생성합니다.
	/// </summary>
	/// <param name="label">슬라이더의 레이블입니다.</param>
	/// <param name="sliderPosition">슬라이더 위치 파라미터입니다.</param>
	/// <returns>슬라이더의 현재 위치를 반환합니다.</returns>
	public float CreateSlider(string label, float sliderPosition)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Grid " + label);
		float position = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
		GUILayout.EndHorizontal();

		return position;
	}

	// Unity Built-in Method
	/// <summary>
	/// Scene에 나타나는 GUI관련된 처리를 진행하는 메서드입니다.
	/// </summary>
	public void OnSceneGUI()
	{
		int controlId = GUIUtility.GetControlID(FocusType.Passive);
		Event e = Event.current;
		Ray ray;

#if UNITY_EDITOR_WIN
		ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
#elif UNITY_EDITOR_OSX
			ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x * 2, -e.mousePosition.y * 2 + Camera.current.pixelHeight));
#else
			Debug.Log("MapEditor doesn't support current OS");
			return;
#endif

		Vector3 mousePos = ray.origin;

		if (e.isMouse && e.type == EventType.MouseDown && e.button == 0)
		{
			GUIUtility.hotControl = controlId;
			e.Use();

			GameObject gameObject;
			Transform prefab = grid_.currentPrefab;

			if (prefab)
			{
				Undo.IncrementCurrentGroup();
				Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid_.width) * grid_.width + grid_.width / 2.0f, Mathf.Floor(mousePos.y / grid_.height) * grid_.height + grid_.height / 2.0f, 0.0f);
				if (GetTransformFromPosition(aligned) != null) return;

				gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
				gameObject.transform.position = aligned;
				gameObject.transform.parent = grid_.transform.GetChild((int)grid_.objectType);
				Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
			}
		}
		else if (e.isMouse & e.type == EventType.mouseDown && e.button == 1)
		{
			GUIUtility.hotControl = controlId;
			e.Use();
			Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid_.width) * grid_.width + grid_.width / 2.0f, Mathf.Floor(mousePos.y / grid_.height) * grid_.height + grid_.height / 2.0f, 0.0f);
			Transform transform = GetTransformFromPosition(aligned);
			if (transform != null)
			{
				Undo.DestroyObjectImmediate(transform.gameObject);
			}
		}

		if (e.isMouse && e.type == EventType.MouseUp && e.button < 2)
		{
			GUIUtility.hotControl = 0;
		}
	}

	/// <summary>
	/// 해당 위치에 있는 타일을 가져옵니다.
	/// </summary>
	/// <param name="aligned">찾고자 하는 위치입니다.</param>
	/// <returns>해당 위치에 존재하는 Transform객체를 반환합니다. 존재하지 않을경우 null을 반환합니다.</returns>
	public Transform GetTransformFromPosition(Vector3 aligned)
	{
		int i = 0;
		while (i < grid_.transform.GetChild((int)grid_.objectType).childCount)
		{
			Transform transform = grid_.transform.GetChild((int)grid_.objectType).GetChild(i);
			if (transform.position == aligned)
			{
				return transform;
			}
			++i;
		}
		return null;
	}
}
