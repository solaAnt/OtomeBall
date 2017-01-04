using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueModel {
	private int _id;
	private bool _isEnded;
	private int _infoModelIndex;
	private List<DialogueInfoModel> _models;

	public void setCfg(DialogueConfig cfg,DialogueMgr dMgr){
		_infoModelIndex = 0;
		_isEnded = false;
		_id = Convert.ToInt32 (cfg.id);

		_models = new List<DialogueInfoModel> ();
		foreach (object contentId in cfg.content) {
			int id=Convert.ToInt32(contentId);
			DialogueInfoModel infoModel=dMgr.getInfoModel(id);
			_models.Add (infoModel);
		}
	}

	public DialogueInfoModel next(){
		DialogueInfoModel infoModel = _models [_infoModelIndex];

		_infoModelIndex++;
		if (_infoModelIndex >= _models.Count) {
			_isEnded=true;
			_infoModelIndex = 0;
		}

		return infoModel;
	}

	public int getId(){
		return _id;
	}

	public bool isEnded(){
		return _isEnded;
	}
}
