using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.BehaviourTrees{

	/// <summary>
	/// Base class for BehaviourTree Composite nodes.
	/// </summary>
    abstract public class BTComposite : BTNode {

		sealed public override int maxOutConnections{ get {return -1;}}
		sealed public override bool showCommentsBottom{ get{return false;}}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override UnityEditor.GenericMenu OnContextMenu(UnityEditor.GenericMenu menu){
			menu = base.OnContextMenu(menu);
			menu = EditorUtils.GetTypeSelectionMenu(typeof(BTComposite), (t)=>{ ReplaceWith(t); }, menu, "Replace");
            if (outConnections.Count > 0){
				menu.AddItem(new GUIContent("Duplicate Branch"), false, ()=> { DuplicateBranch(this, graph); });
				menu.AddItem (new GUIContent ("Delete Branch"), false, ()=> { DeleteBranch(this); } );
			}
			return menu;
		}		

		//Replace the node with another composite
		void ReplaceWith(System.Type t){
			var newNode = graph.AddNode(t, this.nodePosition);
			foreach(var c in inConnections.ToArray()){
				c.SetTarget(newNode);
			}
			foreach(var c in outConnections.ToArray()){
				c.SetSource(newNode);
			}
			if (graph.primeNode == this){
				graph.primeNode = newNode;
			}
			graph.RemoveNode(this);
		}



		///Delete the whole branch of provided root node along with the root node
		static void DeleteBranch(BTNode root){
			var graph = root.graph;
			foreach ( var node in root.GetAllChildNodesRecursively(true).ToArray() ){
				graph.RemoveNode(node);
			}
		}

		//Duplicate a node along with all children hierarchy
		static Node DuplicateBranch(BTNode root, Graph targetGraph){
			
			if (targetGraph == null){
				return null;
			}

			var newNode = root.Duplicate(targetGraph);
			var dupConnections = new List<Connection>();
			for (var i = 0; i < root.outConnections.Count; i++){
				dupConnections.Add( root.outConnections[i].Duplicate(newNode, DuplicateBranch( (BTNode)root.outConnections[i].targetNode, targetGraph) ));
			}
			newNode.outConnections.Clear();
			foreach (var c in dupConnections){
				newNode.outConnections.Add(c);
			}
			return newNode;
		}

		#endif
	}
}