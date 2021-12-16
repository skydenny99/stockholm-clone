using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Move")]
[Category("Creature/Movement")]
public class CreatureMove : ActionTask<MovableObject>
{
	// TODO: Y축 이동 구현
	public BBParameter<float> displacementX = 0f;

	private Transform agentTransform_;
	private float destinationPositionX_;
	private Vector2 moveDirection_;
	

	protected override string info
	{
		get { return "Move To " + displacementX; }
	}

	protected override void OnExecute()
	{
		agentTransform_ = agent.transform;
		
		destinationPositionX_ = agentTransform_.position.x + displacementX.value;
		moveDirection_ = displacementX.value > 0 ? Vector2.right : Vector2.left;
	}

	protected override void OnUpdate()
	{
		float currentDisplacement = destinationPositionX_ - agentTransform_.position.x;
		if (moveDirection_.x * currentDisplacement < 0) {
			agent.Move(Vector2.zero);
			EndAction(true);
			return;
		}

		agent.Move(moveDirection_);
		Debug.Log(moveDirection_);
	}

	protected override void OnStop()
	{
	}

	protected override void OnPause()
	{
	}

	public override void OnDrawGizmos()
	{

	}
}
