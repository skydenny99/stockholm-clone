using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Move To Target")]
[Category("Creature/Movement")]
public class CreatureMoveToTarget : ActionTask<MovableObject>
{
	// TODO: Y축 이동 구현
	[RequiredField]
	public BBParameter<GameObject> target;
	public float endDistance = 0.1f;

	private Transform agentTransform_;
	private Vector2 moveDirection_;

	protected override string info
	{
		get { return "Move To " + target.ToString(); }
	}

	protected override void OnExecute()
	{
		agentTransform_ = agent.transform;
	}

	protected override void OnUpdate()
	{
		if(Mathf.Abs(target.value.transform.position.x - agentTransform_.position.x) <= endDistance) {
			agent.Move(Vector2.zero);
			EndAction(true);
			return;
		}

		if(target.value.transform.position.x - agentTransform_.position.x > 0) {
			moveDirection_ = Vector2.right;
		} else {
			moveDirection_ = Vector2.left;
		}

		agent.Move(moveDirection_);
	}

	protected override void OnStop()
	{
		agent.Move(Vector2.zero);
	}

	protected override void OnPause()
	{
	}

	public override void OnDrawGizmos()
	{
	}
}
