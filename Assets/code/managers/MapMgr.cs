using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class MapMgr : BaseMgr {
	private Dictionary<int,MapModel> _mapModels;
	private int _curRound=0;

	public int getCurRound(){
		return _curRound;
	}

	public MapModel getMapModel(int id){
		return _mapModels [id];
	}

	public override bool init(){
		_mapModels = new Dictionary<int, MapModel> ();

		SolaEngine engine = SolaEngine.getInstance ();
		List<MapConfig> cfgs =(List<MapConfig>) engine.getCfg (MapConfigData.NAME);

		foreach (MapConfig cfg in cfgs) {
			MapModel mapModel=new MapModel();
			mapModel.setCfg(cfg);

			int mapId=mapModel.getId();
			_mapModels.Add(mapId,mapModel);
		}

		return true;
	}
	
	public override bool inited ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		HeroMgr hMgr =(HeroMgr) engine.getMgr (typeof(HeroMgr));

		foreach (MapModel mapModel in _mapModels.Values) {
			mapModel.initModel(hMgr,this);
		}

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
