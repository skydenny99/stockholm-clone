using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Name("Run Function")]
[Category("Script")]
public class RunFunction : ActionTask
{
	SerializedMethodInfo method;
	protected override string info
	{
		get { return "Run Function (Name)"; }
	}

	private MethodInfo method_;
	private List<BBParameter> parameters_ = new List<BBParameter>();

	protected override void OnExecute()
	{
		object[] arguments = parameters_.Select(p => p.value).ToArray();
		method_.Invoke(agent, arguments);
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnStop()
	{
	}

	protected override void OnPause()
	{
	}

#if UNITY_EDITOR
	protected override void OnTaskInspectorGUI()
	{
		base.OnTaskInspectorGUI();

		//Add variable button
		if (GUILayout.Button("Select Function"))
		{
			System.Action<MethodInfo> AddBoundMethod = (f) =>
			{
				method_ = f;

				parameters_.Clear();
				ParameterInfo[] paramsInfo = f.GetParameters();

				for(int i=0; i<paramsInfo.Length; i++) {
					ParameterInfo p = paramsInfo[i];
					BBParameter bbP = BBParameter.CreateInstance(p.ParameterType, blackboard);

					parameters_.Add(bbP);
				}
			};

			GenericMenu menu = new GenericMenu();

			foreach (Component comt in agent.gameObject.GetComponents<Component>().Where(c => c.hideFlags != HideFlags.HideInInspector))
			{
				menu = EditorUtils.GetMethodSelectionMenu(comt.GetType(), typeof(object), typeof(object), AddBoundMethod, 10, false, false, menu, "Method");
				menu = EditorUtils.GetStaticMethodSelectionMenu(comt.GetType(), typeof(object), typeof(object), AddBoundMethod, 10, false, false, menu, "Static Method");
			}

			menu.ShowAsContext();
		}

		if (method_ != null)
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Class Name", "A");
			EditorGUILayout.LabelField("Method Name", method_.Name);
			EditorGUILayout.LabelField("Return Type", method_.ReturnType.Name);
			EditorGUILayout.EndVertical();

			string[] paramsName = method_.GetParameters().Select(p => p.Name).ToArray();

			for (int i = 0; i < paramsName.Length; i++)
			{
				EditorUtils.BBParameterField(paramsName[i], parameters_[i]);
			}
		}
	}
#endif
}
