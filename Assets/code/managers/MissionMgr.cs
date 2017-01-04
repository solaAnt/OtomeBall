using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class MissionMgr : BaseMgr {
	public void initBattleInfo(BattleMgr bMgr){
		foreach (MissionModel mModel in _missionModels.Values)
			mModel.initBattleInfo (bMgr);
	}

	public void initDialogueInfo(DialogueMgr dMgr){
		foreach (MissionModel mModel in _missionModels.Values)
			mModel.initDialogue (dMgr);
	}

	private JsonObject _missionData;
	private Dictionary<int,MissionModel> _missionModels;

	public override bool init(){
		SolaEngine engine = SolaEngine.getInstance ();

		List<MissionConfig> missionCfgs=(List<MissionConfig>)engine.getCfg (MissionConfigData.NAME);
		_missionModels = new Dictionary <int,MissionModel>();
		foreach (MissionConfig cfg in missionCfgs) {
			MissionModel model=new MissionModel();
			model.setCfg(cfg);
			_missionModels.Add(model.getId(),model);
		}

		return true;
	}
	
	public override bool inited ()
	{
		return true;
	}
	
	public override bool loadData(JsonObject data){
		string MGR_NAME = this.GetType ().Name;
		if (!data.ContainsKey (MGR_NAME)) {
			_missionData = new JsonObject ();
			data [MGR_NAME] = _missionData;

//			_missionData [MissionData.MISSION_STATUS]=new JsonObject();
			Debug.Log ("No " + MGR_NAME + " data");
		} else {
			_missionData = (JsonObject)data [MGR_NAME];

			if(!_missionData.ContainsKey(MissionData.MISSION_STATUS))
				return false;

			JsonObject status=(JsonObject)_missionData [MissionData.MISSION_STATUS];
			foreach (int missionId in _missionModels.Keys) {
				int curStatus=Convert.ToInt32( status[missionId.ToString()]);
				_missionModels[missionId].setStatus(curStatus);
			}
		}

		return true;
	}	

	public override bool saveData ()
	{
		JsonObject status = new JsonObject ();
		foreach (int missionId in _missionModels.Keys) {
			status[missionId.ToString()]=_missionModels[missionId].getStatus();
		}
		_missionData [MissionData.MISSION_STATUS] = status;
		return true;
	}

	public Dictionary<int,MissionModel> getMissions(){
		return _missionModels;
	}

}
