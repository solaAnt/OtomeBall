using UnityEngine;
using System.Collections;

public class GamePad : MonoBehaviour {
	public delegate void VoidDelegate ();
	public VoidDelegate onAtkClicked;

	public SolaButtonUgui atkBtn;

	// Use this for initialization
	void Start () {
		atkBtn.onClicked += _onAtkClicked;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void _onAtkClicked(GameObject src){
		if (onAtkClicked == null)
			return;

		onAtkClicked ();
	}
}
