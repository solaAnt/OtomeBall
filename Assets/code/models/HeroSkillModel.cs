using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HeroSkillModel
{
	public int getNeedLevel(){
		return _needLevel;
	}

	public void setActive(bool isActive){
		_isActive = isActive;
	}

	public bool isActive(){
		return _isActive;
	}

	public SkillModel getSkillModel(){
		return _skillModel;
	}

	public void setSkillModel(SkillModel skillModle,int needLevel){
		_skillModel = skillModle;
		_needLevel = needLevel;
	}

	private int _needLevel;
	private bool _isActive=false;
	private SkillModel _skillModel;
}
