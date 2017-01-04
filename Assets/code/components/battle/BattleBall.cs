using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BattleBall : MonoBehaviour
{
	public Rigidbody2D rBody;
	public Animator animator;
	public Animator atkAnimator;

	public Image bodyImgContainer;
	public Image bodyImg;

	public Image selectedImg;
	public Text reduceHpTxt;
	public Slider hpSlider;

	private const int SKIP_FRAME = 5;
	private int _curFrame;
	private bool _needCheck;

	private Vector3 _prePos;
	private BattleHeroModel _model;
	private bool _canMove = false;

	BattleBall(){
		_curFrame = 0;
		_needCheck = false;
		
		_prePos = new Vector3 (-11111, -11111, 0);
	}

	public void setBattleModel (BattleHeroModel model)
	{
		_model = model;

		_model.HP_REDUCED += _hitAni;
		_model.ACTION_CHANGED += _actionChanged;
		_model.HERO_DIED += _heroDie;
		_model.AUTO_ATK += _autoAtk;
		_model.HERO_PUSHING_CHANGED += _onPushChanged;

		_updateView ();
	}
	
	public BattleHeroModel getBattleModel ()
	{
		return _model;
	}

	void OnDestroy(){
		_model.HP_REDUCED -= _hitAni;
		_model.ACTION_CHANGED -= _actionChanged;
		_model.HERO_DIED -= _heroDie;
		_model.AUTO_ATK -= _autoAtk;
		_model.HERO_PUSHING_CHANGED -= _onPushChanged;
	}

	void Start ()
	{
		EventTriggerListener.Get (gameObject).onUp += _onUp;
	}

	void Update ()
	{
		if (_needCheck == false)
			return;

		if (_curFrame < SKIP_FRAME) {
			_curFrame++;
			return;
		}
		_curFrame = 0;

		Vector3 curPos = transform.localPosition;
		if (curPos == _prePos) {
			_needCheck = false;
			
			_model.moveEnd (transform.localPosition);
			return;
		}
		
		_prePos = curPos;
	}


	void OnCollisionExit2D (Collision2D other)
	{
		if (_needCheck) {
			BattleBall targetBall = other.gameObject.GetComponent<BattleBall> ();
			
			if (targetBall == null) {
				return;
			}

			BattleHeroModel target = targetBall.getBattleModel ();

			if(_model.isMonster()!=target.isMonster())
				target.attacked (_model);
		}
	}
	
	private void _onUp (GameObject go, PointerEventData eventData, BaseEventData baseEventData)
	{
		Vector3 wp = eventData.pressEventCamera.GetComponent<Camera>().ScreenToWorldPoint (eventData.position);
		Vector3 np = gameObject.transform.InverseTransformPoint (wp);

		_atk (np);
	}
	
	private void _autoAtk(BattleHeroModel model){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));
		
		List<BattleHeroModel> targets = new List<BattleHeroModel> ();
		bool isMonster = _model.isMonster ();
		if (!isMonster) {
			targets = bMgr.getMonsters ();
		} else {
			foreach(BattleHeroModel pModel in bMgr.getPartners().Values)
				targets.Add(pModel);
		}
		
		BattleHeroModel target = null;
		float minDistance = 0;
		Vector3 pos = transform.localPosition;
		
		foreach (BattleHeroModel tModel in targets) {
			Vector3 tPos=tModel.getPos();
			float distance=Vector3.Distance(pos,tPos);
			
			if(target==null ||distance<minDistance){
				minDistance=distance;
				target=tModel;
			}
		}

		pos =(pos- target.getPos ());

		_atk (pos);
	}

	private void _atk(Vector3 upPoint){
		if (_canMove == false)
			return;
		
		_canMove = false;
		_needCheck = true;

		BattleHeroModel model = _model;
		bool isMonster = model.isMonster ();
		
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));

		List<BattleHeroModel> monsters=bMgr.getMonsters();
		foreach(BattleHeroModel monster in monsters)
			monster.setPushable(isMonster);
		
		Dictionary<int,BattleHeroModel> partners=bMgr.getPartners();
		foreach(BattleHeroModel parnter in partners.Values)
			parnter.setPushable(!isMonster);

//		selectedImg.gameObject.SetActive (false);

		float xF = -upPoint.x;
		float yF = -upPoint.y;

		float forces = Mathf.Sqrt (xF * xF + yF * yF);
		float multipleX = xF / forces;
		float multipleY = yF / forces;

		float maxF = 3f;
		if (forces > maxF) {
			forces = maxF;
			xF=multipleX*forces;
			yF=multipleY*forces;
		}
			
		rBody.AddForce (new Vector2 (xF*1000, yF*1000));
	}

	private void _updateView ()
	{
		BattleHeroModel model = _model;
		HeroModel heroModel = model.getHeroModel ();

		Image head = UITools.createBallImg (model);

		if (head == null) {
			string img = heroModel.getBodyImg ();
			
			Sprite bodySprite = Resources.Load<Sprite> (img);
			bodyImg.sprite = bodySprite;
		} else {
			bodyImg.gameObject.SetActive(false);
			
			bodyImg=head;
			bodyImg.transform.SetParent(bodyImgContainer.transform);
		}

		hpSlider.maxValue = heroModel.getHp ();
		hpSlider.value = _model.getHp ();
	}

	private void _hitAni (int reduceHp)
	{
		reduceHpTxt.text = "-" + reduceHp;
		animator.SetTrigger ("Hit");
		atkAnimator.SetTrigger ("nomal_atk");
		hpSlider.value = _model.getHp ();
	}

	private void _checkDie(){
		if (_model.isDied ())
			_model.die ();
	}

	private void _actionChanged (bool isAction)
	{
		_canMove=isAction;
		selectedImg.gameObject.SetActive (isAction);
		rBody.isKinematic = !isAction;
	}

	private void _heroDie(BattleHeroModel model){
		gameObject.SetActive (false);
		Destroy (gameObject);
	}

	private void _onPushChanged(BattleHeroModel model,bool canPush){
		rBody.isKinematic = !canPush;
	}
}
