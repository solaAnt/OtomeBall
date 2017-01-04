using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToBattlePartnerItem : MonoBehaviour {
	public delegate void ClickedDelegate(int partnerPos);
	public ClickedDelegate onClicked;

	public Image headImg;
	public SolaButtonUgui selectBtn;
	
	public void setPos(int pos){
		_pos = pos;
	}
	
	public void setModel(HeroModel heroModel){
		_heroModel = heroModel;

		_updateView ();
	}

	private HeroModel _heroModel;
	private int _pos;

	private void _updateView(){
		HeroModel heroModel = _heroModel;

		if (heroModel == null) {
			headImg.gameObject.SetActive(false);
			return;
		}

		headImg.gameObject.SetActive(true);
		string img = heroModel.getBodyImg ();
		Sprite bodySprite = Resources.Load<Sprite> (img);
		headImg.sprite = bodySprite;
	}

	private void _onSelectClicked(GameObject src){
		onClicked (_pos);
	}

	void Start () {
		selectBtn.onClicked+=_onSelectClicked;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
