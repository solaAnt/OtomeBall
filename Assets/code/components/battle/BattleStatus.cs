using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleStatus : MonoBehaviour {

	public Image headImg;
	public BattleStatusBar hpBar;
	public BattleStatusBar mpBar;

	private BattleHeroModel _bhModel;

//	void OnDestroy(){
//		_bhModel.HP_REDUCED -= _updateHp;
//	}

	public void setHeroModel(BattleHeroModel model){
		model.HP_REDUCED += _updateHp;
		model.HERO_DIED += _onDied;
		_bhModel = model;

		mpBar.gameObject.SetActive (false);

		HeroModel heroModel = model.getHeroModel ();
		hpBar.setMaxValue (heroModel.getHp ());

		string img = heroModel.getBodyImg ();
		Sprite bodySprite = Resources.Load<Sprite> (img);
		headImg.sprite = bodySprite;

		_updateView ();
	}

	private void _updateView(){
		_updateHp (0);
	}

	private void _updateHp(int reduceHp){
		BattleHeroModel model = _bhModel;
		
		hpBar.setValue (model.getHp());
	}

	private void _onDied(BattleHeroModel model){
		hpBar.setValue (0);
	}
}
