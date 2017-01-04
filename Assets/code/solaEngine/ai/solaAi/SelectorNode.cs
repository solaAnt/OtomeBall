using UnityEngine;
using System.Collections;
using AIStatus;

public class SelectorNode : BaseCompositeNode
{
	public SelectorNode(string nodeName){
		name = nodeName;
	}

	public bool isShowLog = false;

	public override void executeTick ()
	{
		if (isConditionSatisfied () == false)
			return;

		if (status == RunStatus.Ready)
			onEnter ();

		bool canGoOn;
		for (int i=0; i<_childrenNodes.Count; i++) {
			canGoOn = true;
			
			BaseBehaviourNode childrenNode = _childrenNodes [i];
			RunStatus nodeStatus = childrenNode.status;

			switch (nodeStatus) {
			case RunStatus.Ready:
			case RunStatus.Running:
				childrenNode.executeTick ();
				canGoOn = false;
				break;
				
			case RunStatus.Completed:
				canGoOn = false;
				status = RunStatus.Completed;

				return;
			}
			
			if (canGoOn == false)
				return;
		}
		
		status = RunStatus.Failure;
		return;
	}
}
