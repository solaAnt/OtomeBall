using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class BattleMgr : BaseMgr {
	public BattleInfoModel getBattleInfo(int id){
		return _infoModels [id];
	}

	private const int ACTION_PREVIEW_COUNT = 5;

	private List<BattleHeroModel> _monsters;
	private Dictionary<int, BattleHeroModel> _partners;
	private List<BattleHeroModel> _totalModel;

	private MissionModel _missionModel;
	private Dictionary<int,BattleInfoModel> _infoModels;
	private BattleInfoModel[] _curBattleInfos;

	private int _curRound;
	private int _actionTime;
	private bool _isWin;

	private int _star;
	private int _score;

	private int _gold;
	private int _paper;

	public bool isWin(){
		return _isWin;
	}

	public int getStar(){
		return _star;
	}

	public int getScore(){
		return _score;
	}

	public int getRound(){
		return _curRound;
	}

	public int getActionTime(){
		return _actionTime;
	}

	public int getGold(){
		return _gold;
	}

	public int getPaper(){
		return _paper;
	}

	public DialogueModel getDialogueModel(){
		return _missionModel.getDialogud (_curRound);
	}

	public override bool init ()
	{
		return true;
	}

	public override bool inited ()
	{
		_infoModels = new Dictionary<int, BattleInfoModel> ();
		
		SolaEngine engine = SolaEngine.getInstance ();
		List<BattleInfoConfig> infoCfgs=(List<BattleInfoConfig>)engine.getCfg (BattleInfoConfigData.NAME);
		HeroMgr hMgr = (HeroMgr)engine.getMgr (typeof(HeroMgr));
		
		foreach (BattleInfoConfig cfg in infoCfgs) {
			BattleInfoModel model=new BattleInfoModel();
			model.setCfg(cfg,hMgr);

			int id=model.getId();
			_infoModels.Add(id,model);
		}

		MissionMgr mMgr = (MissionMgr)engine.getMgr (typeof(MissionMgr));
		mMgr.initBattleInfo (this);
		return true;
	}

	public override bool loadData (JsonObject data)
	{
		return true;
	}

	public override bool saveData ()
	{
		return true;
	}

	public void setMissionModel(MissionModel model){
		_missionModel = model;
	}

	public void readyForBattle(){
		_curRound = -1;
		_actionTime = 0;
		_isWin = false;

		_star = 0;
		_score = 0;

		_gold = 0;
		_paper = 0;

		SolaEngine engine = SolaEngine.getInstance ();
		PartnerMgr pMgr =(PartnerMgr) engine.getMgr (typeof(PartnerMgr));

		_partners = new Dictionary<int, BattleHeroModel> ();
		Dictionary<int,HeroModel> partner = pMgr.getPartners ();

		foreach (int pos in partner.Keys) {
			HeroModel model=partner[pos];
			BattleHeroModel bhModel=new BattleHeroModel();
			bhModel.setModel(model,false,new Vector3(0,0,0));
			bhModel.formatModel();

			_partners.Add(pos,bhModel);
			bhModel.HERO_DIED+=_onPartnerDeath;
		}

		MissionModel missionModel=_missionModel;
		_curBattleInfos = missionModel.getBattleInfos ();
	}

	public bool nextRound(){
		_curRound++;

		if (_curRound >= _curBattleInfos.Length) {
			_isWin=true;
			_star=3;

			_missionModel.setStatus(MissionModel.status.FINISHED);
			return true;
		}
			
		BattleInfoModel infoModel = _curBattleInfos [_curRound];
		List<BattleHeroModel> monster = infoModel.getMonsters ();

		_monsters = new List<BattleHeroModel> ();
		for (int i=0; i<monster.Count; i++) {
			BattleHeroModel model=monster [i];
			model.formatModel();
			_monsters.Add (model);
		}

		foreach (BattleHeroModel model in _monsters)
			model.HERO_DIED+=_onMonsterDeath;

		List<BattleHeroModel> totalModel = new List<BattleHeroModel> ();
		
		foreach (BattleHeroModel model in _monsters)
			totalModel.Add (model);
		
		foreach (BattleHeroModel model in _partners.Values)
			totalModel.Add (model);

		_totalModel = totalModel;

		BattleEvent.NEXT_ROUND ();
		return false;
	}

	public List<BattleHeroModel> getActionSeq(){
		List<BattleHeroModel> totalModel = _totalModel;
		List<BattleHeroModel> actionSeq = new List<BattleHeroModel> ();
		List<int> restTime=new List<int>();
		
		for(int i=0;i<totalModel.Count;i++) {
			int curRestTime=totalModel[i].getRestTime();
			restTime.Add(curRestTime);
		}
		
		int nextActionIndex=-1;
		for (int previewIndex=0; previewIndex<ACTION_PREVIEW_COUNT; previewIndex++) {
			for (int i=0; i<restTime.Count; i++) {
				
				if (nextActionIndex == -1) {
					nextActionIndex = i;
					continue;
				}
				
				int curRestTime = restTime [i];
				int nextRestTime = restTime [nextActionIndex];
				
				if (curRestTime < nextRestTime)
					nextActionIndex = i;
			}
			
			BattleHeroModel nextModel = totalModel [nextActionIndex];
			actionSeq.Add (nextModel);
			
			int useTime = restTime [nextActionIndex];
			for (int i=0; i<restTime.Count; i++) {
				restTime [i] = restTime [i] - useTime;
			}
			
			restTime [nextActionIndex] = nextModel.getNeedTime ();
		}
		
		return actionSeq;
	}

	public void resetAction(){
		foreach(BattleHeroModel model in _totalModel)
			model.resetActionTime();
	}

	public void applyAction(BattleHeroModel actionModel){
		List<BattleHeroModel> totalModel = _totalModel;

		int restTime = actionModel.getRestTime ();
		actionModel.resetActionTime ();

		foreach(BattleHeroModel model in totalModel){
			model.setAction(false);

			if(actionModel!=model)
				model.reduceRestTime(restTime);
		}

		_actionTime++;
	}

	public Dictionary<int, BattleHeroModel> getPartners(){
		return _partners;
	}

	private void _onPartnerDeath(BattleHeroModel model){
		int removeKey=-1;

		foreach (int key in _partners.Keys) {
			if(_partners[key]==model)
				removeKey=key;
		}
		if (removeKey == -1)
			return;

		_partners.Remove (removeKey);
		_totalModel.Remove (model);

		if (_partners.Count == 0)
			_isWin=false;

		_score -= 200;
		if (_score < 0)
			_score = 0;
	}

	public List<BattleHeroModel> getMonsters(){
		return _monsters;
	}

	private void _onMonsterDeath(BattleHeroModel model){
		_monsters.Remove (model);
		_totalModel.Remove (model);

		_score += 100;
	}
}
