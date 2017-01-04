using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class DialogueMgr : BaseMgr {
	private Dictionary<int,DialogueModel> _dialogueModels;
	private Dictionary<int,DialogueInfoModel> _dialogueInfoModels;
	
	public DialogueModel getModel(int id){
		return _dialogueModels [id];
	}
	
	public DialogueInfoModel getInfoModel(int id){
		return _dialogueInfoModels [id];
	}

	public override bool init(){
		_dialogueModels = new Dictionary<int, DialogueModel> ();
		_dialogueInfoModels = new Dictionary<int, DialogueInfoModel> ();

		return true;
	}

	public override bool inited ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		HeroMgr hMgr =(HeroMgr) engine.getMgr (typeof(HeroMgr));
		
		List<DialogueInfoConfig> infoCfgs=(List<DialogueInfoConfig>)engine.getCfg (DialogueInfoConfigData.NAME);
		for (int i=0; i<infoCfgs.Count; i++) {
			DialogueInfoConfig infoCfg=infoCfgs[i];
			DialogueInfoModel infoModel=new DialogueInfoModel();
			infoModel.setCfg(infoCfg,hMgr);
			
			int id=infoModel.getId();
			_dialogueInfoModels.Add(id,infoModel);
		}

		List<DialogueConfig> cfgs=(List<DialogueConfig>)engine.getCfg (DialogueConfigData.NAME);
		for (int i=0; i<cfgs.Count; i++) {
			DialogueConfig cfg=cfgs[i];
			DialogueModel model=new DialogueModel();
			model.setCfg(cfg,this);
			
			int id=model.getId();
			_dialogueModels.Add(id,model);
		}

		MissionMgr mMgr = (MissionMgr)engine.getMgr (typeof(MissionMgr));
		mMgr.initDialogueInfo (this);
		return true;
	}

	public override bool loadData(JsonObject data){
		return true;
	}

	public override bool saveData ()
	{
		return true;
	}
}
