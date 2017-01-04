using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
	public delegate void VoidDelegate ();
	public VoidDelegate MOVE_END;

	public void run(){
		_needToMove = true;
	}

	public void stop(){
		_needToMove = false;
	}

	public void setParamas(Vector3 targetPos,float duration){
		_targetPos = targetPos;

		Vector3 curPos = gameObject.transform.localPosition;
		float distance=Vector3.Distance (curPos,targetPos);
		float speed = distance / duration;

		_speed = speed;
	}

	void Update () {
		if (_needToMove == false)
			return;

		Vector3 curPos = gameObject.transform.localPosition;
		Vector3 nowPos = Vector3.MoveTowards (curPos,_targetPos,_speed*Time.deltaTime);
	
		if (_previousPos == nowPos) {
			_needToMove=false;
			gameObject.transform.localPosition =_targetPos;

			if(MOVE_END!=null)
				MOVE_END();

			return;
		}

		gameObject.transform.localPosition = nowPos;
		_previousPos = nowPos;
	}

	private float _speed;
	private Vector3 _previousPos;
	private Vector3 _targetPos;

	private bool _needToMove=false;
}
