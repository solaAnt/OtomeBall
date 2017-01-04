using UnityEngine;
using System.Collections;

public class BezierBy : MonoBehaviour {
	public delegate void VoidDelegate ();
	public VoidDelegate MOVE_END;

	public void setParamas(BezierConfig config,float duration){
		_config = config;
		_duration = duration;
	}

	public void run () {
		_needToRun = true;
	}

	public void stop () {
		_needToRun = false;
	}

	void Update () {
		if (_needToRun == false)
			return;

		Vector3 curPos;
		BezierConfig config = _config;

		if (_curTime > _duration) {
			_needToRun = false;

			curPos= config.GetPointAtTime (_duration);
			gameObject.transform.localPosition = curPos;

			if(MOVE_END!=null)
				MOVE_END();

			return;
		}

		curPos= config.GetPointAtTime (_curTime/_duration);
		gameObject.transform.localPosition = curPos;

		_curTime += Time.deltaTime;
	}

	private float _curTime=0;
	private float _duration;
	private BezierConfig _config;

	private bool _needToRun = false;
}
