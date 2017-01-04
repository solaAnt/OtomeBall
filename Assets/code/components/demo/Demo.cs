using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {
	public GameObject[] test;
	public SolaButtonUgui lBtn;
	public SolaButtonUgui rBtn;
	public Hero hero;
	public MonsterA monster;

	private bool _isL=false;
	private bool _isR=false;

	private int diffX=20;
	// Use this for initialization
	void Start () {
		lBtn.onDown+=_onLd;
		rBtn.onDown+=_onRd;

		lBtn.onUp+=_onLp;
		rBtn.onUp+=_onRp;

		InvokeRepeating("createMonster", 2, 2);  //2秒后，没0.3f调用一次
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curPos = hero.transform.localPosition;
		if (_isL == true) {
			curPos.x-=diffX;
		}

		if (_isR == true)
			curPos.x += diffX;

		hero.transform.localPosition = curPos;
	
	}

	private void _onLd(GameObject gameObject){
		_isL=true;
	}

	private void _onLp(GameObject gameObject){
		_isL=false;
	}
	
	private void _onRd(GameObject gameObject){
		_isR=true;
	}

	private void _onRp(GameObject gameObject){
		_isR=false;
	}

	private void createMonster(){
		MonsterA mobj=Instantiate(monster) as MonsterA;

		mobj.transform.SetParent(gameObject.transform);
		mobj.transform.localScale=new Vector3(1f,1f,1f);

		int x=Random.Range(-720/2,720/2);
		Vector3 pos = new Vector3 (x, -1280/2, 0);
		mobj.transform.localPosition = pos;
	}
}
