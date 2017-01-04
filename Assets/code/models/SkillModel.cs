using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillModel
{
	public int getId(){
		return _id;
	}

	public int getType(){
		return _skillType;
	}

	public int getSuccessChance(){
		return _successChance;
	}

	public int getTargetCount(){
		return _targetCount;
	}

	public string getName(){
		return _name;
	}

	public void setCfg(SkillConfig cfg){
//		_cfg = cfg;

		_id = Convert.ToInt32 (cfg.id);
		_name = cfg.name.ToString ();
		_targetCount= Convert.ToInt32 (cfg.targetCount);
		_successChance= Convert.ToInt32 (cfg.successChance);
		_skillType= Convert.ToInt32 (cfg.skillType);
	}

//	private SkillConfig _cfg;
	private int _id;
	private string _name;
	private int _targetCount;
	private int _successChance;
	private int _skillType;
}
