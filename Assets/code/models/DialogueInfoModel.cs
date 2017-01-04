using UnityEngine;
using System;
using System.Collections;

public class DialogueInfoModel {
	private int _id;
	private string _content;
	private HeroModel _leftHeroModel;
	private HeroModel _rightHeroModel;
	private HeroModel _speakerHeroModel;

	public void setCfg(DialogueInfoConfig cfg,HeroMgr hMgr){
		_id = Convert.ToInt32 (cfg.id);
		_content = (string)cfg.content;

		int leftId = Convert.ToInt32 (cfg.leftSpeakerId);
		int rightId = Convert.ToInt32 (cfg.rightSpeakerId);
		int speakerId = Convert.ToInt32 (cfg.speakerId);

		if (leftId != 0) {
			HeroModel leftModel = hMgr.createHero (leftId, 1, 0);
			_leftHeroModel = leftModel;
		}

		if (rightId != 0) {
			HeroModel rightModel = hMgr.createHero (rightId, 1, 0);
			_rightHeroModel = rightModel;
		}

		if (speakerId != 0) {
			HeroModel speakerModel = hMgr.createHero (speakerId, 1, 0);
			_speakerHeroModel = speakerModel;
		}
	}

	public int getId(){
		return _id;
	}

	public string getContent(){
		return _content;
	}

	public HeroModel getLeftModel(){
		return _leftHeroModel;
	}

	public HeroModel getRightModel(){
		return _rightHeroModel;
	}

	public HeroModel getSperkerModel(){
		return _speakerHeroModel;
	}
}
