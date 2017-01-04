using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapModel {
	private MapConfig _cfg;
	private int _id;

	private int _needOvercomeTimes;
	private string _mapName;

	private int[] _dialogueIds;
	private List<MapModel> _canAtkModels;
	private List<HeroModel> _randomMonsterModels;

	public void setCfg(MapConfig cfg){
		_cfg = cfg;

		_id = Convert.ToInt32 (cfg.id);
		_mapName = (string)cfg.mapName;
		_needOvercomeTimes = Convert.ToInt32 (cfg.overComeTimes);

		int dialogueSize = cfg.dialogueIds.Length;
		_dialogueIds = new int[dialogueSize];

		for (int i=0; i<dialogueSize; i++) {
			int dialogueId=Convert.ToInt32 (cfg.dialogueIds[i]);
			_dialogueIds[i]=dialogueId;
		}
	}

	public void initModel(HeroMgr hMgr,MapMgr mMgr){
		MapConfig cfg = _cfg;

		_randomMonsterModels = new List<HeroModel> ();
		int monsterSize = cfg.monsterData.Length;

		for (int i=0; i<monsterSize; i+=2) {
			int id=Convert.ToInt32 (cfg.monsterData[i]);
			int level=Convert.ToInt32 (cfg.monsterData[i+1]);
			
			HeroModel model=hMgr.createHero(id,level,1);
			_randomMonsterModels.Add(model);
		}


		_canAtkModels = new List<MapModel> ();
		int canAtkSize = cfg.canAtkIds.Length;

		for (int i=0; i<canAtkSize; i++) {
			int canAtkId=Convert.ToInt32 (cfg.canAtkIds[i]);
			MapModel mapModel=mMgr.getMapModel(canAtkId);

			_canAtkModels.Add(mapModel);
		}
	}

	public int getId(){
		return _id;
	}

	public string getName(){
		return _mapName;
	}

	public List<MapModel> getAtkMap(){
		return _canAtkModels;
	}

	public int getNeedOvercomeTimes(){
		return _needOvercomeTimes;
	}

	public List<HeroModel> getRandomMonster(){
		return _randomMonsterModels;
	}
}
