using UnityEngine;
using System.Collections;
using AIStatus;

public class BehaviourTree : MonoBehaviour
{
	private BaseCompositeNode _rootNode;

	public void setRootNode (BaseCompositeNode rootNode)
	{
		_rootNode = rootNode;
	}

	public void render(){
		_rootNode.render (new Vector2(500,0),new Vector2(500,0),0);
	}

	public void renderLine(){
		_rootNode.renderLine ();
	}

	void Update ()
	{
		if (_rootNode == null)
			return;

		RunStatus status = _rootNode.status;

		switch (status) {
		case RunStatus.Ready:
		case RunStatus.Running:
			_rootNode.executeTick ();
			break;

		case RunStatus.Completed:
//			Debug.Log("RunStatus.Completed");
			break;
		case RunStatus.Failure:
//			Debug.Log("RunStatus.Failure");
//			_rootNode.reset();
			break;
		}
	
	}
}
