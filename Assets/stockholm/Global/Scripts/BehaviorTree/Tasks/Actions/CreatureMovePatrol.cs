using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Name("Move Patrol")]
[Category("Creature/Movement")]
public class CreatureMovePatrol : ActionTask<MovableObject>
{
	// TODO: Y축 이동 구현
	[RequiredField]
	public BBParameter<Vector2> originPosition;
	public BBParameter<float> patrolAreaRadius = 3f;
	public BBParameter<float> minWaitingTime = 3f;
	public BBParameter<float> maxWaitingTime = 3f;
	[BlackboardOnly]
	public BBParameter<bool> isMoving;

	private Transform agentTransform_;
	private Vector2 moveDirection_;

	private float waitingTime_;
	private float remainWaitingTime_;
	private Vector2 currentMovePosition_;

	protected override string info
	{
		get {
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Patrol {0} in Radius {1:F2}", originPosition.ToString(), patrolAreaRadius);
			
			if(Application.isPlaying) {
				sb.Append("\n   ");
				if (isMoving.value)
					sb.AppendFormat("Current Moving at {0}", currentMovePosition_.ToString());
				else
					sb.AppendFormat("Current Waiting in {0:F2} / {1:F2}", remainWaitingTime_, waitingTime_);
			}

			return sb.ToString();
		}
	}

	protected override void OnExecute()
	{
		agentTransform_ = agent.transform;
		isMoving.value = false;
		remainWaitingTime_ = 0f;
	}

	protected override void OnUpdate()
	{
		if (isMoving.value == true)
		{
			Move();
			return;
		}
		
		if (isMoving.value == false && remainWaitingTime_ <= 0f)
		{
			currentMovePosition_ = new Vector2(originPosition.value.x + Random.Range(-patrolAreaRadius.value, patrolAreaRadius.value), originPosition.value.y);
			isMoving.value = true;

			Move();
		} else {
			remainWaitingTime_ -= Time.deltaTime;
		}
	}
	private void Move() // CreatureMoveToPosition에 있는 것 가져옴
	{
		if (Mathf.Abs(currentMovePosition_.x - agentTransform_.position.x) <= 0.05f)
		{
			agent.Move(Vector2.zero);
			SetWaitingTime();
			return;
		}

		if (currentMovePosition_.x - agentTransform_.position.x > 0)
		{
			moveDirection_ = Vector2.right;
		}
		else
		{
			moveDirection_ = Vector2.left;
		}

		agent.Move(moveDirection_);
	}
	private void SetWaitingTime()
	{
		isMoving.value = false;
		remainWaitingTime_ = waitingTime_ = Random.Range(minWaitingTime.value, maxWaitingTime.value);
	}

	protected override void OnStop()
	{
		agent.Move(Vector2.zero);
	}

	protected override void OnPause()
	{
	}

	#if UNITY_EDITOR
	public override void OnDrawGizmosSelected()
	{
		Vector2 originPos = originPosition.value;

		Gizmos.DrawWireSphere(originPos, 0.2f);
		DrawBox(originPos - new Vector2(patrolAreaRadius.value, 0.2f), originPos + new Vector2(patrolAreaRadius.value, 0.2f));

		if(Application.isPlaying && agent != null && isMoving.value == true) {
			Handles.color = Color.red;
			Handles.DrawLine(agentTransform_.position, currentMovePosition_);
		}
	}
	private void DrawBox(Vector2 topLeft, Vector2 bottomRight)
	{
		float top = topLeft.y;
		float bottom = bottomRight.y;
		float left = topLeft.x;
		float right = bottomRight.x;

		Gizmos.DrawLine(new Vector2(left, top), new Vector2(right, top));
		Gizmos.DrawLine(new Vector2(right, top), new Vector2(right, bottom));
		Gizmos.DrawLine(new Vector2(right, bottom), new Vector2(left, bottom));
		Gizmos.DrawLine(new Vector2(left, bottom), new Vector2(left, top));
	}
#endif
}
