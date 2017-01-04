using UnityEngine;
using System.Collections;
using AIStatus;

public abstract class BaseBehaviourNode
{
	private RunStatus _status = RunStatus.Ready;

	public RunStatus status { get { return _status; } set { _status = value; } }

	private string _name;

	public string name { get { return _name; } set { _name = value; } }

	private BaseBehaviourNode _parentNode;

	public BaseBehaviourNode parentNode { get { return _parentNode; } set { _parentNode = value; } }

	public virtual void onEnter ()
	{
		status = RunStatus.Running;
	}

	public virtual void onChildFinished(){

	}

	public virtual bool isConditionSatisfied ()
	{
		return true;
	}

	public virtual void executeTick ()
	{
//		return true;
	}

	public virtual void reset(){
		status = RunStatus.Ready;
	}

	public virtual void renderLine(){
		
	}

	public virtual void render(Vector2 renderNodePos,Vector2 parentNodePos,int parentIndex){

	}

//	public abstract BaseBehaviourNode clone();
}
