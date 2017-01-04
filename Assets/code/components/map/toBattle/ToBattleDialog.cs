using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToBattleDialog : MonoBehaviour
{
	public Text mapNameTxt;
	public Text mapTypeTxt;
	public Text mapDegreeTxt;
	public SolaButtonUgui backBtn;
	public SolaButtonUgui comfirmBtn;
//	public Transform himeContainer;
	public SolaScroll himeScroll;
	public ToBattleSelectItem himeInstance;
	public ToBattlePartnerItem[] partnerItems;
	public MapDialog mapDialog;

	public void setModel (MapModel mapModel)
	{
		_mapModel = mapModel;

		_updateView ();
		_updateHimes ();
		_updatePartners ();
	}

	ToBattleDialog ()
	{
		_selectItems = new List<ToBattleSelectItem> ();
	}
	
	void Start ()
	{
		backBtn.onClicked += _onBackClicked;
		comfirmBtn.onClicked += _onComfirmClicked;

		PartnerEvent.PARTNERS_CHAGED+=_updatePartners;
	}

	void Update ()
	{
	
	}

	void OnDestroy(){
		PartnerEvent.PARTNERS_CHAGED-=_updatePartners;
	}
	
	private ToBattleSelectItem _selectedHeroItem;
	private MapModel _mapModel;
	private List<ToBattleSelectItem> _selectItems;

	private void _updateView ()
	{
		MapModel mapModel = _mapModel;

		mapNameTxt.text = mapModel.getName ();
		//Fix me
		mapTypeTxt.text = "冒险";
		mapDegreeTxt.text = "!!!!!!";
	}

	private void _updateHimes ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		HeroMgr hMgr = (HeroMgr)engine.getMgr (typeof(HeroMgr));

		List<HeroModel> totalHero = hMgr.getTotalHero ();
		himeScroll.reset ();
		foreach (HeroModel model in totalHero) {
			ToBattleSelectItem item = (ToBattleSelectItem)MonoBehaviour.Instantiate (himeInstance);

			item.setModel (model);
			himeScroll.addItem(item);

			_selectItems.Add (item);
			item.onClicked += _onSelectedHero;
		}
	}

	private void _updatePartners ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		PartnerMgr pMgr = (PartnerMgr)engine.getMgr (typeof(PartnerMgr));
	
		Dictionary<int,HeroModel> partners = pMgr.getPartners ();

		for (int i=0; i<partnerItems.Length; i++) {
			HeroModel partner=null;
			if(partners.ContainsKey(i))
				partner=partners[i];

			ToBattlePartnerItem partnerItem=partnerItems[i];
			partnerItem.setModel(partner);
			partnerItem.setPos(i);

			partnerItem.onClicked+=_onSelectedPartner;
		}
	}

	private void _onSelectedHero (ToBattleSelectItem selectedItem)
	{
		if (_selectedHeroItem != null)
			_selectedHeroItem.setSelected (false);

		_selectedHeroItem = selectedItem;

		HeroModel heroModel = selectedItem.getModel ();

		if (heroModel.isToBattle () == true) {
			SolaEngine engine = SolaEngine.getInstance ();
			PartnerMgr pMgr = (PartnerMgr)engine.getMgr (typeof(PartnerMgr));

			pMgr.removePartner(heroModel);
		}

		selectedItem.setSelected (true);
	}

	private void _onSelectedPartner(int partnerPos){
		ToBattleSelectItem selectedItem = _selectedHeroItem;

		if (selectedItem == null)
			return;

		SolaEngine engine = SolaEngine.getInstance ();
		PartnerMgr pMgr = (PartnerMgr)engine.getMgr (typeof(PartnerMgr));
		pMgr.setPartner (partnerPos,selectedItem.getModel());

		selectedItem.setSelected (false);
		_selectedHeroItem = null;
	}

	private void _onBackClicked (GameObject src)
	{
		foreach (ToBattleSelectItem item in _selectItems) {
			MonoBehaviour.Destroy (item.gameObject);
		}
		_selectItems.Clear ();

		gameObject.SetActive (false);
		mapDialog.gameObject.SetActive (true);
	}

	private void _onComfirmClicked (GameObject src)
	{
		SolaEngine engine = SolaEngine.getInstance ();

		DialogueModel[] dialogueModels=new DialogueModel[0];
		MapModel mapModel = _mapModel;

		List<HeroModel> monsterModels= mapModel.getRandomMonster ();
		int monsterSize = monsterModels.Count;
		BattleInfoModel[] battleInfos=new BattleInfoModel[3];

		for (int j=0; j<battleInfos.Length; j++) {
			List<BattleHeroModel> rdModels = new List<BattleHeroModel> ();

			for (int i=0; i<3; i++) {
				int monsterIndex = engine.randomInt (0, monsterSize);
				HeroModel monster=monsterModels [monsterIndex];

				Vector3 pos=new Vector3();
				pos.x=engine.randomInt(-200,200);
				pos.y=engine.randomInt(-300,300);
				pos.z=0;

				BattleHeroModel bhModel=new BattleHeroModel();
				bhModel.setModel(monster,true,pos);

				rdModels.Add (bhModel);
			}
			BattleInfoModel bInfoModel = new BattleInfoModel ();
			bInfoModel.mapBattleInfo (0, 3, rdModels);

			battleInfos[j]=bInfoModel;
		}

		MissionModel missionModel = new MissionModel ();
		missionModel.formatMapBattle (battleInfos,dialogueModels);

		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));
		bMgr.setMissionModel (missionModel);

		string sceneName = ScenesName.BATTLE;
		engine.enterScene (sceneName);
		Destroy (himeScroll.gameObject);
		Application.LoadLevel (sceneName);
	}
}
