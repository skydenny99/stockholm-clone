using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 에디터상에서 볼 수 있는 격자를 그려줍니다.
/// </summary>
public class Grid : MonoBehaviour
{
	public float width = 32f;
	public float height = 32f;

	public Color color = Color.white;

	public Transform currentPrefab;

	[System.Serializable]
	public enum ObjectType {
		Tile = 0,
		Doodad = 1
	}

	public ObjectType objectType;

	public TileSet tileSet;
	public DoodadSet doodadSet;

	// Unity Built-in Method
	void OnDrawGizmos()
	{
		Vector3 pos = Camera.current.transform.position;
		Gizmos.color = this.color;

		for (float y = pos.y - 800f; y < pos.y + 800f; y += height)
		{
			Gizmos.DrawLine(new Vector3(pos.y - 800f, Mathf.Floor(y / height) * height, 0f),
							new Vector3(pos.y + 800f, Mathf.Floor(y / height) * height, 0f));
		}

		for (float x = pos.x - 1200f; x < pos.x + 1200f; x += width)
		{
			Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, pos.x - 1200f, 0f),
							new Vector3(Mathf.Floor(x / width) * width, pos.x + 1200f, 0f));
		}
	}
}
