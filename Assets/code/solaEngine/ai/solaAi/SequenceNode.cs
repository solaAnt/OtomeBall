using UnityEngine;
using System.Collections;
using AIStatus;

public class SequenceNode : BaseCompositeNode
{
	public SequenceNode(string nodeName){
		name = nodeName;
	}

	public override void executeTick ()
	{
		if (isConditionSatisfied() == false)
			return;

		if (status == RunStatus.Ready)
			onEnter ();

		bool canGoOn;
		for (int i=0; i<_childrenNodes.Count; i++) {
			canGoOn=true;

			BaseBehaviourNode childrenNode = _childrenNodes [i];
			RunStatus nodeStatus = childrenNode.status;

			switch (nodeStatus) {
			case RunStatus.Ready:
			case RunStatus.Running:
				childrenNode.executeTick ();
				canGoOn=false;
				break;

			case RunStatus.Failure:
				canGoOn = false;
				status = RunStatus.Failure;
				break;
			}

			if(canGoOn==false)
				return;
		}

		status = RunStatus.Completed;
		return;
	}
}
