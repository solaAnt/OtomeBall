using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleInfoModel {
	
	public int getId(){
		return _id;
	}
	
	public int getMaxRound(){
		return _maxRound;
	}
	
	public List<BattleHeroModel> getMonsters(){
		return _monsters;
	}

	public void mapBattleInfo(int id,int maxRound,List<BattleHeroModel> monsters){
		_id = id;
		_maxRound = maxRound;
		_monsters = monsters;
	}

	public void setCfg(BattleInfoConfig cfg,HeroMgr hMgr){
		_id=Convert.ToInt32(cfg.id);
		_maxRound=Convert.ToInt32(cfg.round);

		int[] monsterInfos=cfg.monster;
		_monsters = new List<BattleHeroModel> ();

		for (int i=0; i<monsterInfos.Length; i+=4) {
			int id=Convert.ToInt32(monsterInfos[i]);
			int level=Convert.ToInt32(monsterInfos[i+1]);

			HeroModel heroModel=hMgr.createHero(id,level,0);
			BattleHeroModel bhModel=new BattleHeroModel();

			int x=Convert.ToInt32(monsterInfos[i+2]);
			int y=Convert.ToInt32(monsterInfos[i+3]);
			bhModel.setModel(heroModel,true,new Vector3(x,y,0));

			_monsters.Add(bhModel);
		}
	}
	
	private int _id;
	private int _maxRound;
	private List<BattleHeroModel> _monsters;
}
