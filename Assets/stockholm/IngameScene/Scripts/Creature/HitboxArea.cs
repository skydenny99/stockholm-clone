using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[System.Obsolete("이 클래스는 차후 지워질 예정. HitBoxManager랑 Hitbox를 쓰자.")]
public class HitboxArea : MonoBehaviour
{
	public delegate void EnterGameObject(GameObject target);
	public event EnterGameObject EnterGameObjectEvent;
	public delegate void ExitGameObject(GameObject target);
	public event ExitGameObject ExitGameObjectEvent;

	public List<string> detectTagList;

	private Collider2D col_;
	private List<GameObject> gameObjectsInTrigger_ = new List<GameObject>();
	

	// Unity 내장 함수
	void Awake()
	{
		col_ = this.GetComponent<Collider2D>();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (detectTagList.Contains(collision.gameObject.tag))
		{
			gameObjectsInTrigger_.RemoveAll(obj => obj == null);
			if (!gameObjectsInTrigger_.Contains(collision.gameObject))
				gameObjectsInTrigger_.Add(collision.gameObject);
			if(EnterGameObjectEvent != null)
				EnterGameObjectEvent(collision.gameObject);
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (detectTagList.Contains(collision.gameObject.tag))
		{
			gameObjectsInTrigger_.RemoveAll(obj => obj == null);
			if (gameObjectsInTrigger_.Contains(collision.gameObject))
				gameObjectsInTrigger_.Remove(collision.gameObject);
			if(ExitGameObjectEvent != null)
				ExitGameObjectEvent(collision.gameObject);
		}
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		//Debug.Log(collision.gameObject.name);
	}

	public GameObject[] GetAllGameObjectsInArea()
	{
		return gameObjectsInTrigger_.ToArray();
	}
}
