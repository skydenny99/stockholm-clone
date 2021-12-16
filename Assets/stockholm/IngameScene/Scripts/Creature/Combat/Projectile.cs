using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitboxManager))]
public class Projectile : MonoBehaviour, IProjectile
{
	public bool checkingByHitbox = false;
	public int damage = 5;

	protected float speed_ = 10;
	protected float range_ = 10;
	protected bool isFired = false;
	protected LayerMask hitLayer_;
	protected Creature caller_;
	private HitboxManager hitboxManager_;
	protected ProjectileHitCallback hitCallback_;

	protected float movingDistance_ = 0;
	protected Vector2 currentDirection_;

	protected virtual void Awake()
	{
		hitboxManager_ = this.GetComponent<HitboxManager>();
		hitboxManager_.OnHitEvent += OnHitEvent;
	}

	private void Start()
	{
		if(checkingByHitbox == false)
			hitboxManager_.ChangeHitbox(0);
	}
	private void OnDestroy()
	{
		hitboxManager_.OnHitEvent -= OnHitEvent;
	}

	private void OnHitEvent(IngameObject hittedIngameObject)
	{
		HitTarget(caller_, hittedIngameObject);
	}

	protected virtual void Update()
	{
		if (!isFired)
			return;
		Move(currentDirection_ * speed_ * Time.deltaTime);

		if (movingDistance_ >= range_)
			Destroy(gameObject);
	}

	public virtual void Fire(float speed, float range, Vector2 direction, LayerMask hitLayer, Creature caller, ProjectileHitCallback hitCallBack)
	{
		speed_ = speed;
		range_ = range;
		hitLayer_ = hitLayer;
		hitCallback_ = hitCallBack;
		caller_ = caller;
		isFired = true;

		hitboxManager_.layerMask = hitLayer_;

		if (direction == Vector2.right)
			this.transform.localScale = new Vector3(-1, 1, 1);
		currentDirection_ = direction;
	}

	public void Move(Vector2 offset)
	{
		if (checkingByHitbox == true)
		{
			Vector2 origin = transform.position;
			Vector2 direction = offset.normalized;
			float distance = offset.magnitude;

			RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, hitLayer_);

			// TODO: 죽어있는 것은 GetComponent도 안 하도록
			if (hit.collider != null)
			{
				IngameObject hittedObject = hit.collider.GetComponent<IngameObject>();
				if (hittedObject != null && hittedObject != caller_)
				{
					HitTarget(caller_, hittedObject);
					return;
				}
			}
		}

		transform.Translate(offset);
		movingDistance_ += offset.magnitude;
	}

	private void HitTarget(Creature caller, IngameObject target)
	{
		if (caller != target)
		{
			hitCallback_.Invoke(caller, target);

			Destroy(this.gameObject);
		}
	}
}
