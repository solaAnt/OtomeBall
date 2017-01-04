using UnityEngine;
using System.Collections;

public class BattlePlayer : MonoBehaviour {
	public Animator playerAnimator;
	public GamePad gamePad;

	// Use this for initialization
	void Start () {
		gamePad.onAtkClicked += _onAtkClicked;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void _onAtkClicked(){
		playerAnimator.SetTrigger("ATK");
	}
}
