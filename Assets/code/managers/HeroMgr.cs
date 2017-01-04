using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class HeroMgr : BaseMgr
{
	private JsonObject _heroData;
	private SolaEngine _engine;
	List<HeroConfig> _cfgs;
	private List<HeroModel> _modelList;
	private Dictionary<int,HeroModel> _modelDict;
	private int _heroIndex;

	public override bool init ()
	{
		_modelList = new List<HeroModel> ();
		_modelDict = new Dictionary<int, HeroModel> ();

		_engine = SolaEngine.getInstance ();
		_cfgs = (List<HeroConfig>)_engine.getCfg (HeroConfigData.NAME);

		return true;
	}
	
	public override bool inited ()
	{
		return true;
	}

	public override bool loadData (JsonObject data)
	{
		string MGR_NAME = this.GetType ().Name;
		JsonObject heroInfos;
		if (!data.ContainsKey (MGR_NAME)) {
			_heroData = new JsonObject ();
			data [MGR_NAME] = _heroData;

			_heroIndex = 1;
			heroInfos = new JsonObject ();
		} else {
			_heroData = (JsonObject)data [MGR_NAME];

			_heroIndex = Convert.ToInt32 (_heroData [HeroData.HERO_INDEX]);
			heroInfos = (JsonObject)_heroData [HeroData.HERO_INFOS];
		}

		foreach (JsonObject savaData in heroInfos.Values) {
			int id = Convert.ToInt32 (savaData [HeroData.Info.ID]);
			int configId = Convert.ToInt32 (savaData [HeroData.Info.CONFIG_ID]);
			int level = Convert.ToInt32 (savaData [HeroData.Info.LEVEL]);
			int exp = Convert.ToInt32 (savaData [HeroData.Info.EXP]);

			HeroModel model = createHero (id, configId, level, exp);
			addHero (model);
		}

		PartnerMgr pMgr = (PartnerMgr)_engine.getMgr (typeof(PartnerMgr));
		pMgr.loadDataAfterHeroMgr (data, this);

		return true;
	}

	public override bool saveData ()
	{
		if (_heroData == null)
			return false;

		JsonObject heroInfos = new JsonObject ();
		foreach (HeroModel model in _modelList) {
			int heroId = model.getId ();
			heroInfos [heroId.ToString ()] = model.getSavaData ();
		}
		
		_heroData [HeroData.HERO_INFOS] = heroInfos;
		_heroData [HeroData.HERO_INDEX] = _heroIndex;

		SolaSaver.getInstance ().save ();
		return true;
	}

	public List<HeroModel> getTotalHero ()
	{
		return _modelList;
	}

	public HeroModel getHero (int id)
	{
		if (_modelDict.ContainsKey (id))
			return _modelDict [id];
		
		return null;
	}

	public HeroModel createHero (int configId, int level, int exp)
	{
		HeroModel model = createHero (_heroIndex, configId, level, exp);
		_heroIndex++;
		//saveData ();
		return model;
	}

	private HeroModel createHero (int id, int configId, int level, int exp)
	{
		HeroConfig cfg = _cfgs [configId - 1];
		HeroModel model = new HeroModel ();

		SolaEngine engine = SolaEngine.getInstance ();
		SkillMgr skillMgr = (SkillMgr)engine.getMgr (typeof(SkillMgr));
		model.setData (cfg, id, level, exp, skillMgr);

		return model;
	}

	public void addHero (HeroModel heroModel)
	{
		int id = heroModel.getId ();
		_modelDict.Add (id, heroModel);
		_modelList.Add (heroModel);

		//saveData ();
	}
}
