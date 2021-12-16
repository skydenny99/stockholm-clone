using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeCanvas.BehaviourTrees
{
	[Name("Timeout")]
	[Category("Decorators")]
	[Description("Custom Decorator (Cooldown) 안녕?")]
	[Icon("Timeout")]
	public class Cooldown : BTDecorator
	{
		public BBParameter<float> timeout = 1f;

		private float remainTime_ = 0f;

		private Coroutine timerCoroutine_;

		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{

			if (decoratedConnection == null)
				return Status.Resting;
			
			status = decoratedConnection.Execute(agent, blackboard);

			if (timerCoroutine_ == null)
			{
				remainTime_ = timeout.value;
				timerCoroutine_ = StartCoroutine(Timer());
				return status;
			}
			else
			{
				if (remainTime_ <= 0)
				{
					timerCoroutine_ = null;
					decoratedConnection.Reset();
					return Status.Failure;
				}
				else
				{
					return status;
				}
			}

		}

		private IEnumerator Timer()
		{
			while (remainTime_ > 0)
			{
				remainTime_ -= Time.deltaTime;
				yield return null;
			}
		}

#if UNITY_EDITOR
		protected override void OnNodeGUI()
		{
			Rect r = EditorGUILayout.BeginVertical();

			r.width -= 10f;
			r.x += 5f;

			if(remainTime_ <= 0)
				EditorGUI.ProgressBar(r, 1f, "Time out");
			else
				EditorGUI.ProgressBar(r, remainTime_ / timeout.value, string.Format("Remain ({0:F2})", remainTime_));

			GUILayout.Space(18);
			EditorGUILayout.EndVertical();
			GUILayout.Space(4);
		}
#endif
	}
}