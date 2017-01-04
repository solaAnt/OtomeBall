using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionModel
{
	public class status
	{
		public const int UNFINISH = 0;
		public const int FINISHED = 1;
	}
	
	public void formatMapBattle(BattleInfoModel[] battleInfos,DialogueModel[] dialogueModels){
		_battleInfoModels = battleInfos;
		_dialogueModels = dialogueModels;
	}

	public BattleInfoModel[] getBattleInfos ()
	{
		return _battleInfoModels;
	}

	public DialogueModel getDialogud (int round)
	{
		if (round >= _dialogueModels.Length)
			return null;
		return _dialogueModels [round];
	}

	public int getId ()
	{
		return _id;
	}
	
	public string getTitle ()
	{
		return _title;
	}
	
	public string getDesc ()
	{
		return _desc;
	}
	
	public int getStatus ()
	{
		return _status;
	}
	
	public void setStatus (int status)
	{
		_status = status;
		
		SolaEngine engine = SolaEngine.getInstance ();
		MissionMgr mMgr = (MissionMgr)engine.getMgr (typeof(MissionMgr));
		
		mMgr.saveData ();
		SolaSaver.getInstance ().save ();
	}
	
	public void initBattleInfo (BattleMgr bMgr)
	{
		int infoLength = _battleInfoIds.Length;
		_battleInfoModels = new BattleInfoModel[infoLength];
		
		for (int i=0; i<infoLength; i++) {
			int battleInfoId = _battleInfoIds [i];
			BattleInfoModel bInfoModel = bMgr.getBattleInfo (battleInfoId);
			
			_battleInfoModels [i] = bInfoModel;
		}
	}
	
	public void initDialogue (DialogueMgr dMgr)
	{
		int dialogueLength = _dialogueModelIds.Length;
		_dialogueModels = new DialogueModel[dialogueLength];
		
		for (int i=0; i<dialogueLength; i++) {
			int dialogueId = _dialogueModelIds [i];
			DialogueModel dModel = dMgr.getModel (dialogueId);
			
			_dialogueModels [i] = dModel;
		}
	}
	
	public void setCfg (MissionConfig cfg)
	{
		_id = Convert.ToInt32 (cfg.id);
		_title = (string)cfg.title;
		_desc = (string)cfg.desc;
		_status = status.UNFINISH;
		
		int[] infoIds = cfg.battleInfoId;
		int size = infoIds.Length;
		_battleInfoIds = new int[size];
		
		for (int i=0; i<size; i++) {
			int id = Convert.ToInt32 (infoIds [i]);
			_battleInfoIds [i] = id;
		}
		
		int[] dialogueIds = cfg.dialogueId;
		size = dialogueIds.Length;
		_dialogueModelIds = new int[size];
		
		for (int i=0; i<size; i++) {
			int id = Convert.ToInt32 (dialogueIds [i]);
			_dialogueModelIds [i] = id;
		}
	}

	private int _id;
	private string _title;
	private string _desc;
	private int _status;
	private int[] _battleInfoIds;
	private BattleInfoModel[] _battleInfoModels;
	private int[] _dialogueModelIds;
	private DialogueModel[] _dialogueModels;
}
