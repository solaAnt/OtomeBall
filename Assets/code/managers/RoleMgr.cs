using UnityEngine;
using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoleMgr : BaseMgr
{
	public const string MGR_NAME = "RoleMgr";
	private JsonObject _roleData;
	private SolaEngine _engine;
	private RoleConfig _roleCfg;
	private CreateRoleConfig _createRoleCfg;
	private string _roleId = "";
	private int _roleLevel = 0;
	private int _roleExp = 0;
	private int _roleMaxExp = 0;
	private int _gold = 0;
	private int _paper = 0;

	public override bool init ()
	{
		_engine = SolaEngine.getInstance ();
		_roleCfg = ((List<RoleConfig>)_engine.getCfg (RoleConfigData.NAME)) [0];
		_createRoleCfg = ((List<CreateRoleConfig>)_engine.getCfg (CreateRoleConfigData.NAME)) [0];
		
		return true;
	}
	
	public override bool inited ()
	{
		return true;
	}

	public override bool loadData (JsonObject data)
	{
		if (!data.ContainsKey (MGR_NAME)) {
			_roleData = new JsonObject ();
			data [MGR_NAME] = _roleData;
			Debug.Log ("No " + MGR_NAME + " data");
		} else {
			_roleData = (JsonObject)data [MGR_NAME];
			
			_roleId = (string)_roleData [RoleData.ROLE_ID];
			_roleLevel = Convert.ToInt32 (_roleData [RoleData.ROLE_LEVEL]);
			_roleExp = Convert.ToInt32 (_roleData [RoleData.ROLE_EXP]);
			
			_gold = Convert.ToInt32 (_roleData [RoleData.ROLE_GOLD]);
			_paper = Convert.ToInt32 (_roleData [RoleData.ROLE_PAPER]);
		}
		
		int expBase = Convert.ToInt32 (_roleCfg.expBase);
		_roleMaxExp = _roleLevel * expBase;
		return true;
	}
	
	public override bool saveData(){
		_roleData [RoleData.ROLE_ID] = _roleId;
		_roleData [RoleData.ROLE_LEVEL] = _roleLevel;
		_roleData [RoleData.ROLE_EXP] = _roleExp;
		
		_roleData [RoleData.ROLE_GOLD] = _gold;
		_roleData [RoleData.ROLE_PAPER] = _paper;

		SolaSaver.getInstance ().save ();
		return true;
	}

	public string getRoleId ()
	{
		return _roleId;
	}

	public int getRoleLevel ()
	{
		return _roleLevel;
	}

	public int getRoleExp ()
	{
		return _roleExp;
	}

	public int getRoleMaxExp ()
	{
		return _roleMaxExp;
	}

	public int getGold ()
	{
		return _gold;
	}

	public int getPaper ()
	{
		return _paper;
	}

	public bool isSigned ()
	{
		return _roleId != "";
	}

	public void sign (string roleId)
	{
		if (_roleId != "")
			return;

		CreateRoleConfig createRoleCfg = _createRoleCfg;

		_roleId = roleId;
		_roleLevel = 1;
		_roleExp = 0;

		_gold = Convert.ToInt32 (createRoleCfg.gold);
		_paper = Convert.ToInt32 (createRoleCfg.paper);


		int configId = Convert.ToInt32 (createRoleCfg.heroId);
		int level = Convert.ToInt32 (createRoleCfg.heroLevel);

		HeroMgr hMgr =(HeroMgr) _engine.getMgr (typeof(HeroMgr));
		HeroModel heroModel=hMgr.createHero (configId,level,0);
		hMgr.addHero (heroModel);
		//Fix me:Test Data.
		for (int i=1; i<=5; i++) {
			HeroModel hero = hMgr.createHero (i, 5, 5);
			hMgr.addHero (hero);
		}
		hMgr.saveData ();

		PartnerMgr pMgr = (PartnerMgr)_engine.getMgr (typeof(PartnerMgr));
		pMgr.setPartner (0, heroModel);
		saveData ();
	}


}
