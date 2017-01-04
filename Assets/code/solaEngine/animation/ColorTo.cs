using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorTo : MonoBehaviour {
	public delegate void VoidDelegate ();
	public VoidDelegate COLOR_END;
	
	public void run(){
		_needToMove = true;
	}
	
	public void stop(){
		_needToMove = false;
	}
	
	public void setParamas(
		Image spriteRender,
		Color targetColor,
		float duration){

		_image = spriteRender;
		_targetColor = targetColor;
		_duration = duration;

		_beginColor = spriteRender.color;
	}
	
	void Update () {
		if (_needToMove == false)
			return;

		if (_curTime > _duration) {
			_needToMove = false;
			
			if(COLOR_END!=null)
				COLOR_END();
			
			return;
		}

		Image image = _image;
		image.color = Color.Lerp (
			_beginColor,_targetColor,_curTime / _duration
		);

		_curTime += Time.deltaTime;
	}
	
	private float _curTime=0;
	private float _duration;

	private Color _beginColor;
	private Color _targetColor;
	private Image _image;
	
	private bool _needToMove=false;
}
