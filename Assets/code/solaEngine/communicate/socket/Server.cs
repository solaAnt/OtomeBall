using UnityEngine;  
using System.Collections;  
using System.Net;  
using System.IO;  
using System.Net.Sockets;  
using System.Text;
using AIStatus;

public class Server : MonoBehaviour
{  
	private TestActionNode2 _node;
	private BehaviourTree _bt;

	void Start ()
	{
//		InvokeRepeating ("sssstest", 1, 4);
		BehaviourTree testBt = gameObject.AddComponent<BehaviourTree> ();
		
		SelectorNode rootNode = new SelectorNode ("根节点");
		
		SequenceNode sqn = new SequenceNode ("连贯动作节点");
		SequenceNode actionNode1 = new SequenceNode ("连贯动作节点");
		TestActionNode1 acc1 = new TestActionNode1 ("逃跑动作");
		actionNode1.addChildren (acc1);

		TestActionNode2 actionNode2 = new TestActionNode2 ("跳跃动作");
		TestActionNode3 actionNode3 = new TestActionNode3 ("技能1");
		
		TestConditionNode1 tc1 = new TestConditionNode1 ();
		actionNode3.addCondition (tc1);
		
		sqn.addChildren (actionNode1);
		sqn.addChildren (actionNode2);
		sqn.addChildren (actionNode3);
		_node = actionNode2;
		
		//======
		SelectorNode snn = new SelectorNode ("选择节点");
		TestActionNode1 actionNode4 = new TestActionNode1 ("逃跑");
		SelectorNode actionNode5 = new SelectorNode ("选择是否全屏技能");
		TestActionNode1 ddd = new TestActionNode1 ("全屏技能");
		actionNode5.addChildren (ddd);

		TestActionNode3 actionNode6 = new TestActionNode3 ("技能2");
		
		snn.addChildren (actionNode4);
		snn.addChildren (actionNode5);
		snn.addChildren (actionNode6);
		
		actionNode4.addCondition (tc1);
		
		rootNode.addChildren (sqn);
		rootNode.addChildren (snn);

		TestActionNode3 rootAction = new TestActionNode3 ("根部动作");
		rootNode.addChildren (rootAction);
		StartCoroutine (ttt ());
		
		testBt.setRootNode (rootNode);
		
		_bt = testBt;

		_bt.renderLine ();
	}

//	void OnDrawGizmos ()
//	{
//		_bt.renderLine ();
////		GUI.Box (Rect (10,10,100,50), GUIContent("This is text", icon));
//	}
	void Update ()
	{
		_bt.renderLine ();
	}

	void OnGUI ()
	{
		_bt.render ();
	}

	IEnumerator ttt ()
	{
		yield return new WaitForSeconds (2f);
		_node.status = RunStatus.Completed;
	}
		
	void sssstest ()
	{
		Debug.Log ("调用了");
		SolaEngine engine = SolaEngine.getInstance ();

		TestPacket packet = new TestPacket ();
//		packet.testInt = 123;
//		packet.testString = "客户端发送到服务端";
//		packet.testIntList = new int[3];
//		packet.testIntList [0] = 10;
//		packet.testIntList [1] = 20;
//		packet.testIntList [2] = 310;
//
//		packet.testStringList = new string[3];
//		packet.testStringList [0] = "ddd";
//		packet.testStringList [1] = "20";
//		packet.testStringList [2] = "310";
//
//		packet.testBool = true;

//		packet.end = new TestPacket_end ();

		engine.send (OpCode.TEST_OPCODE, packet);
	}

	void OnApplicationQuit ()
	{
		print ("OnApplicationQuit");
		SolaEngine engine = SolaEngine.getInstance ();
		engine.closeSocket ();
	}
}