using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

[Category("Creature/GameObject")]
[Description("A combination of line of sight and view angle check in 2D")]
public class CanSeeTarget2D : ConditionTask<MovableObject>
{
	[RequiredField]
	public BBParameter<GameObject> target;
	public BBParameter<LayerMask> layerMask = (LayerMask)(-1);
	public BBParameter<float> maxDistance = 50;
	[SliderField(1, 180)]
	public BBParameter<float> viewAngle = 70f;
	public Vector2 offset;

	[GetFromAgent]
	protected Collider2D agentCollider;
	[GetFromAgent]
	protected Transform agentTransform_;
	private RaycastHit2D hit;

	protected override string info
	{
		get { return "Can See " + target.ToString() + " in 2D"; }
	}

	protected override bool OnCheck()
	{
		var targetTransform = target.value.transform;
		if ((agentTransform_.position + (Vector3)offset - targetTransform.position).magnitude > maxDistance.value)
			return false;

		hit = Physics2D.Linecast((Vector2)agentTransform_.position + offset, (Vector2)targetTransform.position + offset, layerMask.value);

		if (hit.collider != null)
		{
			Collider2D targetCollider = target.value.GetComponent<Collider2D>();
			if (targetCollider == null || hit.collider != targetCollider)
			{
				return false;
			}
		}

		return Vector2.Angle(targetTransform.position - agentTransform_.position, agent.standingSide) <= viewAngle.value / 2f;
	}

	public override void OnDrawGizmosSelected()
	{
		if (agent != null)
		{
			Vector3 offset3 = (Vector3)offset;
			Gizmos.DrawLine(agent.transform.position, agent.transform.position + offset3);
			Gizmos.DrawLine(agent.transform.position + offset3, agent.transform.position + offset3 + ((Vector3)agent.standingSide * maxDistance.value));

			for (float angle = -viewAngle.value / 2; angle <= viewAngle.value / 2; angle += viewAngle.value / 10f)
			{
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				Gizmos.DrawRay(agent.transform.position + offset3, q * (agent.standingSide) * maxDistance.value);
			}
		}
	}
}
