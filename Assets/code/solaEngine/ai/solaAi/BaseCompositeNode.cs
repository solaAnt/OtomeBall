using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AIStatus;

public class BaseCompositeNode:BaseBehaviourNode
{
	protected List<BaseBehaviourNode> _childrenNodes = new List<BaseBehaviourNode> ();
	protected List<BaseConditionNode> _conditionNodes = new List<BaseConditionNode> ();
	
	public void addChildren (BaseBehaviourNode children)
	{
		_childrenNodes.Add (children);
	}

	public void addCondition (BaseConditionNode condition)
	{
		_conditionNodes.Add (condition);
	}

	public override void reset ()
	{
		base.reset ();

		foreach (BaseBehaviourNode children in _childrenNodes)
			children.reset ();
	}

	public override bool isConditionSatisfied ()
	{
		foreach (BaseConditionNode conditionNode in _conditionNodes) {
			if (conditionNode.isSatisfied () == false) {
				status = RunStatus.Failure;
				return false;
			}
		}

		return true;
	}
	
	protected Vector2 _renderNodePos;
	protected Vector2 _parentNodePos;
	protected int _index;
	public static Vector2 rectSize = new Vector2 (100, 100);

	public override void renderLine ()
	{
		Vector3 aa = new Vector3 ();
		aa.x = _renderNodePos.x+rectSize.x/2;
		aa.y = Screen.height - _renderNodePos.y-rectSize.y/2;

		Vector3 st = Camera.main.ScreenToWorldPoint (aa);
		st.z = 0;

		Vector3 bb = new Vector3 ();
		bb.x = _parentNodePos.x+rectSize.x/2;
		bb.y = Screen.height - _parentNodePos.y-rectSize.y/2;

		Vector3 end =  Camera.main.ScreenToWorldPoint (bb);

		end.z = 0;

		Debug.DrawLine (st,end);

		for (int i=0; i<_childrenNodes.Count; i++)
			_childrenNodes [i].renderLine ();
	}

	public override void render (Vector2 renderNodePos, Vector2 parentNodePos, int parentIndex)
	{
		_renderNodePos = renderNodePos;
		_parentNodePos = parentNodePos;
		_index = parentIndex + 1;

		List<string> desc = new List<string> ();
		desc.Add (name);

		for (int j=0; j<_conditionNodes.Count; j++) {
			desc.Add (_conditionNodes [j].name);
		}


		GUIContent content;
		if (desc.Count > 2)
			content = new GUIContent (desc [0], desc [1]);
		else
			content = new GUIContent (desc [0]);

		GUI.Box (new Rect (renderNodePos.x, renderNodePos.y,
		                  rectSize.x, rectSize.y), content);

//		Gizmos.DrawCube (_tranformV3(renderNodePos),rectSize);
		int gapX = 1;
		switch (_index) {
		case 1:
			gapX = 250;
			break;
		case 2:
			gapX = 10;
			break;
		case 3:
			gapX = 2;
			break;
		case 4:
			gapX = 1;
			break;
		case 5:
			gapX = 1;
			break;
		}

		int totalX = (int)(_childrenNodes.Count * (gapX + BaseCompositeNode.rectSize.x));

		for (int i=0; i<_childrenNodes.Count; i++) {
			Vector2 childrenPos = new Vector2 ();
			childrenPos.x = (_renderNodePos.x + i * (BaseCompositeNode.rectSize.x + gapX)) - totalX / 4;
			childrenPos.y = _renderNodePos.y + BaseCompositeNode.rectSize.y + 50;

			_childrenNodes [i].render (childrenPos, _renderNodePos, _index);
		}
	}

	private Vector3 _tranformV3 (Vector2 src)
	{
		Vector3 pos = new Vector3 ();
		pos.x = src.x;
		pos.y = src.y;
		pos.z = 0;

		return pos;
	}
//	public override void executeTick ()
//	{
//		return true;
//	}

//	public override BaseBehaviourNode clone ()
//	{
//		BaseCompositeNode cloneNode = new BaseCompositeNode ();
//		for (int i=0; i<_conditionNodes.Count; i++) 
//			cloneNode.addCondition (_conditionNodes [i]);
//
//		for (int i=0; i<_childrenNodes.Count; i++) 
//			cloneNode.addChildren (_childrenNodes [i].clone ());
//
//		return cloneNode;
//	}
}
