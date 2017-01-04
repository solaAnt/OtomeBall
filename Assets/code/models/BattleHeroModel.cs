using UnityEngine;
using System.Collections;

public class BattleHeroModel {
	public delegate void HpReduced (int reduceHp);
	public HpReduced HP_REDUCED;

	public delegate void ActionChanged (bool isAction);
	public ActionChanged ACTION_CHANGED;

	public delegate void HeroEventDelegate(BattleHeroModel model);
	public HeroEventDelegate AUTO_ATK;
	public HeroEventDelegate HERO_DIED;
	public HeroEventDelegate MOVE_END;

	public delegate void HeroPushingDelegate(BattleHeroModel model,bool canPush);
	public HeroPushingDelegate HERO_PUSHING_CHANGED;

	private HeroModel _heroModel;

	private int _atk;
	private int _hp;
	private int _spd;

	private Vector3 _basePos;
	private Vector3 _pos;
	private int _actionNeedTime;
	private int _actionRestTime;

	private bool _isMonster;

	public void formatModel(){
		HP_REDUCED = null;
		HERO_DIED = null;
		MOVE_END = null;
		setModel (_heroModel, _isMonster,_basePos);
	}

	public void setModel(HeroModel heroModel,bool isMonster,Vector3 basePos){
		_heroModel = heroModel;
		_isMonster = isMonster;

		_atk = heroModel.getAtk ();
		_hp = heroModel.getHp ();
		_spd = heroModel.getSpd ();

		_actionNeedTime = 200 / _spd;
		_actionRestTime = _actionNeedTime;

		_basePos = basePos;
		_pos = _basePos;
	}

	public bool isMonster(){
		return _isMonster;
	}

	public int getAtk(){
		return _atk;
	}

	public int getHp(){
		return _hp;
	}

	public int getSpd(){
		return _spd;
	}

	public void setPos(Vector3 pos){
		_pos = pos;
	}

	public Vector3 getPos(){
		return _pos;
	}

	public bool isDied(){
		return _hp <= 0;
	}

	public void reduceRestTime(int time){
		int excessTime = _actionRestTime - time;

		if (excessTime >= 0)
			_actionRestTime = excessTime;
	}

	public void resetActionTime(){
		_actionRestTime = _actionNeedTime;
	}

	public int getRestTime(){
		return _actionRestTime;
	}

	public int getNeedTime(){
		return _actionNeedTime;
	}

	public HeroModel getHeroModel(){
		return _heroModel;
	}

	public void attacked(BattleHeroModel attacker){
		int reduceHp = attacker.getAtk ();
		_hp -= reduceHp;

		HP_REDUCED (reduceHp);
		if (_hp < 0) {
			_hp=0;
		}
	}

	public void setAction(bool isAction){
		ACTION_CHANGED (isAction);
	}

	public void setPushable(bool canPush){
		HERO_PUSHING_CHANGED (this, canPush);
	}

	public void autoAtk(){
		AUTO_ATK (this);
	}

	public void die(){
		HERO_DIED (this);
	}

	public void moveEnd(Vector3 pos){
		_pos = pos;
		MOVE_END (this);
	}
}
