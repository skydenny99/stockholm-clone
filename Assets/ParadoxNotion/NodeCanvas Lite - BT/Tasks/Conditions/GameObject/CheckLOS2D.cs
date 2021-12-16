using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{

	[Name("Target In Line Of Sight 2D")]
	[Category("GameObject")]
	[Description("Check of agent is in line of sight with target by doing a linecast and optionaly save the distance")]
	public class CheckLOS2D : ConditionTask<Transform>
	{

		[RequiredField]
		public BBParameter<GameObject> LOSTarget;
		public BBParameter<LayerMask> layerMask = (LayerMask)(-1);
		public Vector2 offset;
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		[GetFromAgent]
		protected Collider2D agentCollider;
		private RaycastHit2D hit;

		protected override string info
		{
			get { return "LOS with " + LOSTarget.ToString(); }
		}

		protected override bool OnCheck()
		{
			hit = Physics2D.Linecast((Vector2)agent.position + offset, LOSTarget.value.transform.position, layerMask.value);

			if (hit.collider != null)
			{
				Collider2D targetCollider = LOSTarget.value.GetComponent<Collider2D>();
				if (targetCollider == null || hit.collider != targetCollider)
				{
					return false;
				}
			}

			return true;
		}

		public override void OnDrawGizmosSelected()
		{
			if (agent && LOSTarget.value)
				Gizmos.DrawLine(agent.position + (Vector3)offset, LOSTarget.value.transform.position);
		}
	}
}