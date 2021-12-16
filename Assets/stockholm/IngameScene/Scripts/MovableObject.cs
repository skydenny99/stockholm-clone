using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovableObject : IngameObject, IMovable
{
	// TODO : 우선 클래스 세팅으로 숨겨둠. 따로 설정창 있으면 더 좋을듯
	private LayerMask groundMask_;
	private LayerMask lineGroundMask_;
	private uint horizontalRayNum_ = 8;
	private uint verticalRayNum_ = 3;

	// Collider끼리 딱 맞추면 벽을 타거나 하는 버그가 발생하기 때문에 Raycast를 할 때 간격을 줌
	private float gapBetweenEachCollider_ = 0.02f;

	private BoxCollider2D collider_;
	protected new BoxCollider2D collider
	{
		get { return collider_; }
	}

	private struct ColliderPos
	{
		public Vector2 topLeft;
		public Vector2 topRight;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
	}
	private ColliderPos colliderPosInRayTest_;

	public virtual Vector2 standingSide
	{
		get
		{
			return this.transform.localScale.x < 0f ? Vector2.right : Vector2.left;
		}
	}

	// Todo: 기획자 테스트를 위해 SerializeField 시켜둠. 나중에 전역설정으로 대체할것
	[SerializeField]
	private float gravity_ = -50;
	public float gravity
	{
		get
		{
			return gravity_;
		}
		set
		{
			gravity_ = value;
		}
	}

	[SerializeField]
	private bool useGravity_ = false;
	public bool useGravity
	{
		get
		{
			return useGravity_;
		}
		set
		{
			useGravity_ = value;
		}
	}

	[SerializeField]
	private bool groundCheck_ = true;
	public bool groundCheck
	{
		get { return groundCheck_; }
		set { groundCheck_ = value; }
	}

	[SerializeField]
	private bool isGrounded_ = false;
	public bool isGrounded
	{
		get
		{
			return isGrounded_;
		}
	}

	[SerializeField]
	private bool isStatic_ = false;
	/// <summary>
	/// 이동 가능한 오브젝트인지 여부.
	/// </summary>
	public bool isStatic
	{
		get
		{
			return isStatic_;
		}
		set
		{
			isStatic_ = value;
		}
	}

	[SerializeField]
	private bool blockControl_ = false;
	public bool blockControl
	{
		get
		{
			return blockControl_;
		}
		set
		{
			blockControl_ = value;
		}
	}

	/// <summary>
	/// Force함수를 이용해 컨트롤을 막은 경우 true이다.
	/// </summary>
	private bool blockControlForce_ = false;

	/// <summary>
	/// 오브젝트 조종이 막혀있는지 여부이다.
	/// </summary>
	public bool isControlBlocked
	{
		get { return blockControl_ || blockControlForce_ || isStatic_; }
	}

	protected Vector2 gravityVelocity_ = Vector2.zero;
	private Vector2 lastGravityVelocity_;
	public Vector2 gravityVelocity
	{
		get { return lastGravityVelocity_; }
	}

	protected Vector2 moveVelocity_ = Vector2.zero;
	private Vector2 lastMoveVelocity_;
	public Vector2 moveVelocity
	{
		get { return lastMoveVelocity_; }
	}

	private List<Force> forceList_ = new List<Force>();
	private Vector2 lastForceVelocity_;
	public Vector2 forceVelocity
	{
		get { return lastForceVelocity_; }
	}

	/// <summary>
	/// 가장 최근에 적용된 오브젝트의 속도를 반환합니다.
	/// </summary>
	public Vector2 velocity
	{
		get
		{
			return velocity_;
		}
	}
	private Vector2 velocity_ = Vector2.zero;



	public delegate void ForceCallback();

	/// <summary>
	/// MovableObject에 적용되는 Force에 대한 오브젝트입니다.
	/// </summary>
	private class Force
	{
		private bool blockMovement_;
		public bool blockMovement
		{
			get { return blockMovement_; }
		}

		private bool isFinished_ = false;
		public bool isFinished
		{
			get { return isFinished_; }
		}

		private ForceCallback finishCallback_;
		private Vector2 forceVelocity_;
		private float forceAcceleration_;

		public Force(Vector2 forceVelocity, float forceAcceleration, bool blockMovement = false, ForceCallback finishCallback = null)
		{
			forceVelocity_ = forceVelocity;
			forceAcceleration_ = forceAcceleration;
			blockMovement_ = blockMovement;
			finishCallback_ = finishCallback;
		}
		public Vector2 ApplyForce(float deltaTime)
		{
			// Force 만료시 로직
			if (forceVelocity_.x * forceAcceleration_ >= 0)
			{
				isFinished_ = true;
				if (finishCallback_ != null)
				{
					finishCallback_.Invoke();
				}
				return Vector2.zero;
			}

			forceVelocity_.x += forceAcceleration_ * deltaTime;
			return forceVelocity_;
		}
		public Vector2 velocity
		{
			get { return forceVelocity_; }
		}
	}

	protected enum ContactSide
	{
		None = 0,
		Top = 1 << 0,
		Bottom = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
		All = int.MaxValue
	}

	// Unity Built-in Method
	protected virtual void Awake()
	{
		collider_ = this.GetComponent<BoxCollider2D>();
		groundMask_ = LayerMask.GetMask("Ground");
		lineGroundMask_ = LayerMask.GetMask("LineGround");
	}

	private Vector2 UpdateGravityVelocity(float deltaTime)
	{
		if (!useGravity_)
		{
			gravityVelocity_ = Vector2.zero;
		}
		else
		{
			gravityVelocity_.y += gravity_ * deltaTime;
		}

		return gravityVelocity_;
	}

	private Vector2 UpdateForceVelocity(float deltaTime)
	{
		Vector2 forceAmount = Vector2.zero;

		forceList_.RemoveAll(force => force.isFinished);
		if (forceList_.Count > 0)
		{
			blockControlForce_ = forceList_.Where(force => force.blockMovement).Count() > 0;

			forceAmount = forceList_.Select(force => force.ApplyForce(deltaTime)).Aggregate((lforce, rforce) => lforce + rforce);
		}
		else
		{
			blockControlForce_ = false;
		}

		return forceAmount;
	}

	private Vector2 UpdateMoveVelocity(float deltaTime)
	{
		Vector2 result = moveVelocity_;
		moveVelocity_ = Vector2.zero;

		if (isControlBlocked)
		{
			return Vector2.zero;
		}
		return result;
	}

	protected virtual void Update()
	{
		if (isStatic_)
		{
			return;
		}

		Vector2 gravityVelocity = UpdateGravityVelocity(Time.deltaTime);
		lastGravityVelocity_ = gravityVelocity;

		Vector2 forceVelocity = UpdateForceVelocity(Time.deltaTime);
		lastForceVelocity_ = forceVelocity;

		Vector2 moveVelocity = UpdateMoveVelocity(Time.deltaTime);
		lastMoveVelocity_ = moveVelocity;

		velocity_ = gravityVelocity + forceVelocity + moveVelocity;
		MoveImmediately(velocity_ * Time.deltaTime);
	}

	/// <summary>
	/// 해당 방향으로 즉시 움직입니다.
	/// </summary>
	/// <param name="moveDisplacement">움직일 변위입니다.</param>
	public virtual void MoveImmediately(Vector2 moveDisplacement)
	{
		if (isStatic_)
		{
			return;
		}

		UpdateColliderPos();

		ContactSide side = ContactSide.None;

		if (groundCheck_)
		{
			if (moveDisplacement.x != 0f)
				side |= CorrectHorizontalMove(ref moveDisplacement);
			if (moveDisplacement.y != 0f)
				side |= CorrectVerticalMove(ref moveDisplacement);
		}

		transform.Translate(moveDisplacement);
		OnCheckContactWith(side);
		// yangju : 중력을 사용하지 않을경우를 생각해, 레이캐스팅을 한번 더 하는 방식으로 변경함.
		// isGrounded_ = (Mathf.Abs(moveDisplacement.y) <= Mathf.Epsilon);
	}

	/// <summary>
	/// 다음 프레임에 해당 방향으로 움직입니다.
	/// </summary>
	/// <param name="moveDisplacement">움직일 변위입니다.</param>
	public virtual void Move(Vector2 moveDisplacement)
	{
		moveVelocity_ += moveDisplacement;
	}


	/// <summary>
	/// 해당 방향으로 강제로 이동시킵니다. 강제로 이동되는 동안에는 조작이 불가능합니다.
	/// </summary>
	/// <param name="distance">강제로 이동되는 거리입니다.</param>
	/// <param name="duration">강제로 이동되는 시간입니다.</param>
	/// <param name="blockControl">강제로 이동되는 도중에 점프 같은 조작을 할 수 없게 합니다.</param>
	// TODO: 가속도도 사용자 지정 설정으로
	// TODO: 1초 이상 때 처리 (1초일때는 가속도의 크기와 속도의 크기가 같음)
	// TODO: 정확한 거리만큼보다 더 가는 것 수정
	public virtual void AddForce(Vector2 distance, float duration = 0.4f, bool blockControl = false, ForceCallback finishedCallback = null)
	{
		if (isStatic_)
		{
			return;
		}
		// 날아가는 시간은 x축 기준임(y축은 고정된 가속도인 중력이 있어서...)
		// knockbackTime시간동안 moveX로 날아갈 때 속도와 가속도
		// v = (-s)/(t(t-1))
		// a = (2s)/(t(t-1))
		Vector2 forceVelocity = new Vector2(-distance.x / (duration * (duration - 1f)), 0);

		float forceAcceleration = 2 * distance.x / (duration * (duration - 1f));
		gravityVelocity_.y += Mathf.Sqrt(2f * (-gravity) * distance.y);

		forceList_.Add(new Force(forceVelocity, forceAcceleration, blockControl, finishedCallback));
	}

	/// <summary>
	/// Collider의 위치를 갱신합니다.
	/// </summary>
	private void UpdateColliderPos()
	{
		// 실제 Raycast를 쏠 때는 원래 Collider 크기보다 조금 작은 크기를 씀

		Bounds bound = collider_.bounds;
		bound.Expand(2f * -gapBetweenEachCollider_);

		colliderPosInRayTest_.topLeft = new Vector2(bound.min.x, bound.max.y);
		colliderPosInRayTest_.topRight = new Vector2(bound.max.x, bound.max.y);
		colliderPosInRayTest_.bottomLeft = new Vector2(bound.min.x, bound.min.y);
		colliderPosInRayTest_.bottomRight = new Vector2(bound.max.x, bound.min.y);
	}

	/// <summary>
	/// 수평으로 움직이는 것을 보정합니다.
	/// </summary>
	/// <param name="moveDisplacement">움직일 변위입니다.</param>
	/// <returns>부딪힌 면을 ContactSide비트로 반환합니다.</returns>
	private ContactSide CorrectHorizontalMove(ref Vector2 moveDisplacement)
	{
		ContactSide side = ContactSide.None;
		float verticalGap = (colliderPosInRayTest_.topLeft.y - colliderPosInRayTest_.bottomLeft.y) / (horizontalRayNum_ - 1);

		Vector2 rayDirection;
		Vector2 firstRayInitPos;
		if (moveDisplacement.x < 0)
		{
			rayDirection = Vector2.left;
			firstRayInitPos = colliderPosInRayTest_.bottomLeft;
		}
		else
		{
			rayDirection = Vector2.right;
			firstRayInitPos = colliderPosInRayTest_.bottomRight;
		}

		for (int i = 0; i < horizontalRayNum_; i++)
		{
			Vector2 rayInitPos = new Vector2(firstRayInitPos.x, firstRayInitPos.y + verticalGap * i);

			RaycastHit2D raycastHit = Physics2D.Raycast(rayInitPos, rayDirection, Mathf.Abs(moveDisplacement.x) + gapBetweenEachCollider_, groundMask_ | lineGroundMask_);

			if (raycastHit)
			{
				moveDisplacement.x = raycastHit.point.x - rayInitPos.x;

				if (rayDirection == Vector2.left)
				{
					moveDisplacement.x += gapBetweenEachCollider_;
					side |= ContactSide.Left;
				}
				else
				{
					moveDisplacement.x -= gapBetweenEachCollider_;
					side |= ContactSide.Right;
				}
			}
		}
		return side;
	}

	/// <summary>
	/// 수직으로 움직이는 것을 보정합니다.
	/// </summary>
	/// <param name="moveDisplacement">움직일 변위입니다.</param>
	/// <returns>부딪힌 면을 ContactSide비트로 반환합니다.</returns>
	private ContactSide CorrectVerticalMove(ref Vector2 moveDisplacement)
	{
		ContactSide side = ContactSide.None;
		float horizontalGap = (colliderPosInRayTest_.bottomRight.x - colliderPosInRayTest_.bottomLeft.x) / (verticalRayNum_ - 1);

		Vector2 rayDirection;
		Vector2 firstRayInitPos;
		if (moveDisplacement.y < 0)
		{
			rayDirection = Vector2.down;
			firstRayInitPos = colliderPosInRayTest_.bottomLeft;
		}
		else
		{
			rayDirection = Vector2.up;
			firstRayInitPos = colliderPosInRayTest_.topLeft;
		}

		for (int i = 0; i < verticalRayNum_; i++)
		{
			Vector2 rayInitPos = new Vector2(firstRayInitPos.x + horizontalGap * i, firstRayInitPos.y);

			RaycastHit2D raycastHit;
			// 올라갈 때는 LineGround는 충돌계산에 포함하지 않음
			if (rayDirection == Vector2.up)
				raycastHit = Physics2D.Raycast(rayInitPos, rayDirection, Mathf.Abs(moveDisplacement.y) + gapBetweenEachCollider_, groundMask_);
			else
				raycastHit = Physics2D.Raycast(rayInitPos, rayDirection, Mathf.Abs(moveDisplacement.y) + gapBetweenEachCollider_, groundMask_ | lineGroundMask_);

			if (raycastHit)
			{
				moveDisplacement.y = raycastHit.point.y - rayInitPos.y;

				if (rayDirection == Vector2.down)
				{
					moveDisplacement.y += gapBetweenEachCollider_;
					side |= ContactSide.Bottom;
				}
				else
				{
					moveDisplacement.y -= gapBetweenEachCollider_;
					side |= ContactSide.Top;
				}
			}
		}

		return side;
	}

	/// <summary>
	/// Ground와 충돌이 일어났을 경우, 그 방향을 반환합니다.
	/// </summary>
	/// <param name="side">충돌한 방향입니다.</param>
	protected virtual void OnCheckContactWith(ContactSide side)
	{
		isGrounded_ = false;

		if ((side & ContactSide.Bottom) != 0)
		{
			isGrounded_ = true;
			if (gravityVelocity_.y < 0)
				gravityVelocity_.y = 0;
		}

		if ((side & ContactSide.Top) != 0)
		{
			if (gravityVelocity_.y > 0)
				gravityVelocity_.y = 0;
		}
	}
}
