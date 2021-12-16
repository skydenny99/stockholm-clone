using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hitbox영역 정보를 가지고 있는 클래스입니다. 여기서 실질적으로 충돌 검사를 하진 않습니다.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class Hitbox : MonoBehaviour
{
	public int id;
	[HideInInspector]
	public new PolygonCollider2D collider;

	private void Awake()
	{
		collider = this.GetComponent<PolygonCollider2D>();
		Debug.Assert(collider != null);

		collider.enabled = false;
	}
}