using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hitbox들을 관리하고 판정을 처리하는 곳입니다.
/// </summary>
public class HitboxManager : MonoBehaviour
{
	public Hitbox[] hitBoxes;
	[HideInInspector]
	public LayerMask layerMask;

	public delegate void OnHit(IngameObject hittedIngameObject);
	public event OnHit OnHitEvent;

	private PolygonCollider2D hitbox_;

	private int currentHitboxIndex_ = -1;
	private int currentHitboxID_ = -1;

	private List<IngameObject> hittedIngameObject_ = new List<IngameObject>();

	// Unity 내장 함수
	private void Awake()
	{		
		hitbox_ = this.gameObject.AddComponent<PolygonCollider2D>();
		hitbox_.isTrigger = true;
		hitbox_.pathCount = 0;
	}

	/// <summary>
	/// Hitbox를 바꿔줍니다.
	/// </summary>
	/// <param name="index">바꿔줄 Hitbox의 인덱스입니다. -1을 넣으면 Hitbox 판정을 하지 않습니다.</param>
	public void ChangeHitbox(int index)
	{
		if (index != -1)
		{
			currentHitboxIndex_ = index;
			hitbox_.SetPath(0, hitBoxes[index].collider.GetPath(0));

			// ID가 다르면 새로운 판정의 Hitbox이기 때문에 hittedCreature를 지움
			if (hitBoxes[index].id != currentHitboxID_)
			{
				hittedIngameObject_.Clear();
				currentHitboxID_ = hitBoxes[index].id;
			}
		} else {
			hittedIngameObject_.Clear();
			currentHitboxIndex_ = -1;
			currentHitboxID_ = -1;
			hitbox_.pathCount = 0;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (((1 << collider.gameObject.layer) & layerMask) == 0)
			return;
		
		IngameObject colliderIngameObject = collider.GetComponent<IngameObject>();

		if (colliderIngameObject != null)
		{
			if (!hittedIngameObject_.Exists(e => e.Equals(colliderIngameObject)))
			{
				hittedIngameObject_.Add(colliderIngameObject);

				OnHitEvent(colliderIngameObject);
			}
		}
	}
}
