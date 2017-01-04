using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class SkillMgr : BaseMgr {
	public SkillModel getSkillModel(int skillId){
		return _skillModels [skillId];
	}

	private Dictionary<int,SkillModel> _skillModels;

	public override bool init(){
		_skillModels = new Dictionary<int, SkillModel> ();

		SolaEngine engine = SolaEngine.getInstance ();
		List<SkillConfig> skillCfgs = (List<SkillConfig>) engine.getCfg (SkillConfigData.NAME);

		for (int i=0; i<skillCfgs.Count; i++) {
			SkillConfig skillCfg=skillCfgs[i];

			SkillModel skillModel=new SkillModel();
			skillModel.setCfg(skillCfg);

			int skillId=skillModel.getId();
			_skillModels.Add(skillId,skillModel);
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
//			_missionData = new JsonObject ();
//			data [MGR_NAME] = _missionData;

//			_missionData [MissionData.MISSION_STATUS]=new JsonObject();
			Debug.Log ("No " + MGR_NAME + " data");
		} else {
//			_missionData = (JsonObject)data [MGR_NAME];
//
//			if(!_missionData.ContainsKey(MissionData.MISSION_STATUS))
//				return false;
//
//			JsonObject status=(JsonObject)_missionData [MissionData.MISSION_STATUS];
//			foreach (int missionId in _missionModels.Keys) {
//				int curStatus=Convert.ToInt32( status[missionId.ToString()]);
//				_missionModels[missionId].setStatus(curStatus);
//			}
		}

		return true;
	}	

	public override bool saveData ()
	{
//		JsonObject status = new JsonObject ();
//		foreach (int missionId in _missionModels.Keys) {
//			status[missionId.ToString()]=_missionModels[missionId].getStatus();
//		}
//		_missionData [MissionData.MISSION_STATUS] = status;
		return true;
	}
}
