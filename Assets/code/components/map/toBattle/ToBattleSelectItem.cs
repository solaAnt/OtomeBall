using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToBattleSelectItem : SolaScrollItem {
	public delegate void ClickedDelegate(ToBattleSelectItem himeItem);
	public ClickedDelegate onClicked;

	public Text nameTxt;
	public Text levelTxt;

	public Text toBattleImg;

	public Image bgImg;
	public Transform headContainer;
	public Image headImg;
	public Image[] rarityImgs;

	public Text atkTxt;
	public Text hpTxt;
	public Text spdTxt;

	public SolaButtonUgui selectBtn;

	public void setModel(HeroModel heroModel){
		_heroModel = heroModel;
		_heroModel.BATTLE_STATUS_CHANGED += _onBattleStatusChanged;

		_updateView ();
	}

	public HeroModel getModel(){
		return _heroModel;
	}

	public void setSelected(bool isSelected){
		Color newColor = bgImg.color;

		if (isSelected == true)
			newColor.a = 0.5f;
		else
			newColor.a = 1f;

		bgImg.color = newColor;
	}

	public override float getHeight(){
		return 120f;
	}

	private HeroModel _heroModel;

	void Start(){
		selectBtn.onClicked+= _onSelectClicked;
	}

	void OnDestroy(){
		_heroModel.BATTLE_STATUS_CHANGED -= _onBattleStatusChanged;
	}

	private void _updateView(){
		HeroModel heroModel = _heroModel;

		nameTxt.text = heroModel.getName ();
		levelTxt.text = heroModel.getLevel ().ToString();

		atkTxt.text = heroModel.getAtk ().ToString();
		hpTxt.text = heroModel.getHp ().ToString();
		spdTxt.text = heroModel.getSpd ().ToString();

		int rarity = heroModel.getStart ();
		for (int i=0; i<rarityImgs.Length; i++)
			rarityImgs[i].gameObject.SetActive(i<rarity);

		Image curHeadImg = UITools.createStatusHead (heroModel);
		if (curHeadImg != null) {
			headImg.gameObject.SetActive (false);

			curHeadImg.transform.SetParent (headContainer);
		} else {
			headImg.gameObject.SetActive (true);
			string img = heroModel.getBodyImg ();
			Sprite sprite=UITools.loadSprite(img);
			headImg.sprite = sprite;
		}

		_onBattleStatusChanged ();
	}

	private void _onBattleStatusChanged(){
		HeroModel heroModel = _heroModel;
		bool isToBattle = heroModel.isToBattle ();

		toBattleImg.gameObject.SetActive (isToBattle);
	}

	private void _onSelectClicked(GameObject src){
		onClicked (this);
	}
}
