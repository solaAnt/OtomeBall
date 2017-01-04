using UnityEngine;
using System.Collections;
using AIStatus;

public class TestActionNode1 : ActionNode {
	public TestActionNode1(string src){
		name = src;
	}
	public override void executeTick ()
	{
		if (isConditionSatisfied () == false) {
			status = RunStatus.Failure;
			return;
		}
		
		if (status == RunStatus.Running)
			return;
		
		base.executeTick ();

		Debug.Log ("exe TestActionNode1");
		status = RunStatus.Completed;
	}
}

public class TestActionNode2 : ActionNode {
	public TestActionNode2(string src){
		name = src;
	}

	public override void executeTick ()
	{
		if (isConditionSatisfied () == false) {
			status = RunStatus.Failure;
			return;
		}

		if (status == RunStatus.Running)
			return;

		base.executeTick ();

		Debug.Log ("exe TestActionNode2");
		status = RunStatus.Running;

		return;
	}
}

public class TestActionNode3 : ActionNode {
	public TestActionNode3(string src){
		name = src;
	}
//	public TestActionNode3(){
//		TestConditionNode1 c2 = new TestConditionNode1 ();
//		addCondition (c2);
//	}

	public override void executeTick ()
	{
		if (isConditionSatisfied () == false) {
			status = RunStatus.Failure;
			return;
		}
		
		if (status == RunStatus.Running)
			return;
		
		base.executeTick ();

		Debug.Log ("exe TestActionNode3");
		status = RunStatus.Completed;
		return;
	}
}
