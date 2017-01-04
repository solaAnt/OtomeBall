using UnityEngine;
using System.Collections;

public class TestConditionNode1 : BaseConditionNode {
	public TestConditionNode1(){
		name = "等级不满15，不执行";
	}

	public override bool isSatisfied ()
	{
		Debug.Log (name);
		return false;
	}
}

public class TestConditionNode2 : BaseConditionNode {
	public TestConditionNode2(){
		name = "等级少于15，可执行";
	}

	public override bool isSatisfied ()
	{
		Debug.Log (name);
		return true;
	}
}