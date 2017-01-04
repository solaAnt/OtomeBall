using UnityEngine;
using System.Collections;
using AIStatus;

public class ActionNode : BaseCompositeNode {
	public ActionNode(){
	}

	public ActionNode(string nodeName){
		name = nodeName;
	}

	public override void executeTick ()
	{
		if (status == RunStatus.Ready)
			onEnter ();
	}
}
