using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleStatusBar : MonoBehaviour {
	public Image frontGround;
	public Text valueTxt;

	private float _maxValue;
	private float _curValue;

	public void setMaxValue(float maxValue){
		_maxValue = maxValue;

		_updateView ();
	}

	public void setValue(float value){
		_curValue = value;

		_updateView ();
	}

	private void _updateView(){
		valueTxt.text = _curValue.ToString ();

		float precent = _curValue / _maxValue;
		frontGround.transform.localScale = new Vector3 (precent, 1f, 1f);
	}
}
