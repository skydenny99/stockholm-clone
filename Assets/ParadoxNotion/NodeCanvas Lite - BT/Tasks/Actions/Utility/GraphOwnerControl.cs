using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;

namespace NodeCanvas.Tasks.Actions{

	[Name("Control Graph Owner")]
	[Category("✫ Utility")]
	[Description("Start, Resume, Pause, Stop a GraphOwner's behaviour")]
	public class GraphOwnerControl : ActionTask<GraphOwner> {

		public enum Control
		{
			StartBehaviour,
			StopBehaviour,
			PauseBehaviour
		}

		public Control control = Control.StartBehaviour;
		public bool waitActionFinish = true;

		protected override string info{
			get {return agentInfo + "." + control.ToString();}
		}

		protected override void OnExecute(){
			StartCoroutine(Do());
		}

		//these should be done after graph has updated.
		//EndAction is on purpose called before start/pause/stop.
		IEnumerator Do(){
			yield return null;

			if (control == Control.StartBehaviour){
				if (waitActionFinish){
					agent.StartBehaviour( (s)=> { EndAction(s); } );
				} else {
					EndAction();
					agent.StartBehaviour();
				}
			}

			if (control == Control.StopBehaviour){
				EndAction();
				agent.StopBehaviour();
			}

			if (control == Control.PauseBehaviour){
				EndAction();
				agent.PauseBehaviour();
			}
		}

		protected override void OnStop(){
			if (waitActionFinish && control == Control.StartBehaviour){
				agent.StopBehaviour();
			}
		}
	}
}