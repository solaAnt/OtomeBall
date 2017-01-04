using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class HeroModel
{
	public delegate void VoidDelegate ();
	public VoidDelegate BATTLE_STATUS_CHANGED;

	public void setToBattle(bool isToBattle){
		_isToBattle = isToBattle;

		if (BATTLE_STATUS_CHANGED != null)
			BATTLE_STATUS_CHANGED ();
	}

	public bool isToBattle(){
		return _isToBattle;
	}

	private bool _isToBattle;

//	private HeroConfig _cfg;
	private int _configId;
	private string _name;
	private string _bodyImg;

	private int _start;
	private int _id;
	private int _level;
	private int _exp;
	private int _atk;
	private int _hp;
	private int _spd;
	private int _atkBase;
	private int _atkParama;
	private int _hpBase;
	private int _hpParama;
	private int _spdBase;
	private int _spdParama;
	private List<HeroSkillModel> _heroSkillModels;

	public void setData (HeroConfig cfg, int id, 
	                     int level, int exp,SkillMgr skillMgr)
	{
//		_cfg = cfg;

		_configId = Convert.ToInt32 (cfg.id);
		_id = id;
		_exp = exp;

		_start = Convert.ToInt32 (cfg.star);
		_bodyImg = "body/body" + _configId;
		_name = (string)cfg.name;

		_atkBase = Convert.ToInt32 (cfg.atkBase);
		_atkParama = Convert.ToInt32 (cfg.atkParam);

		_hpBase = Convert.ToInt32 (cfg.hpBase);
		_hpParama = Convert.ToInt32 (cfg.hpParam);

		_spdBase = Convert.ToInt32 (cfg.spdBase);
		_spdParama = Convert.ToInt32 (cfg.spdParam);

		_heroSkillModels = new List<HeroSkillModel> ();
		int[] skillInfo = cfg.skillInfo;//[level,skillId]
		for (int i=0; i<skillInfo.Length; i=i+2) {
			int needLevel=Convert.ToInt32(skillInfo[i]);
			int skillId=Convert.ToInt32(skillInfo[i+1]);

			SkillModel skillModel=skillMgr.getSkillModel(skillId);
			HeroSkillModel hsModel=new HeroSkillModel();
			hsModel.setSkillModel(skillModel,needLevel);

			_heroSkillModels.Add(hsModel);
		}

		setLevel (level);
	}

	public JsonObject getSavaData ()
	{
		JsonObject savaData = new JsonObject ();
		savaData [HeroData.Info.ID] = _id;
		savaData [HeroData.Info.CONFIG_ID] = _configId;
		savaData [HeroData.Info.LEVEL] = _level;
		savaData [HeroData.Info.EXP] = _exp;

		return savaData;
	}

	public int getId ()
	{
		return _id;
	}

	public string getBodyImg ()
	{
		return _bodyImg;
	}

	public string getName(){
		return _name;
	}

	public int getConfigId ()
	{
		return _configId;
	}

	public int getStart(){
		return _start;
	}

	public int getAtk(){
		return _atk;
	}

	public int getHp(){
		return _hp;
	}

	public int getSpd(){
		return _spd;
	}

	public int getLevel(){
		return _level;
	}

	public void setLevel (int level)
	{
		_level = level;

		_atk = _atkBase + level * _atkParama/10;
		_hp = _hpBase + level * _hpParama;
		_spd = (int)((float)(_spdBase + level * _spdParama)*0.1f);
	}
}
