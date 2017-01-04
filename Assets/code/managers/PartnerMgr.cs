using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class PartnerMgr : BaseMgr {
	private const int PARTNER_SIZE = 4;

	private JsonObject _parnerData;
	
	private int _leaderPos;
	private Dictionary<int,HeroModel> _partners;

	public bool isToBattle(HeroModel heroModel){
		foreach (HeroModel partner in _partners.Values)
			if(partner==heroModel)
				return true;

		return false;
	}

	public override bool init ()
	{
		_partners=new Dictionary<int, HeroModel>();
		return true;
	}
	
	public override bool inited ()
	{
		return true;
	}

	public override bool loadData (JsonObject data)
	{
		return true;
	}

	public bool loadDataAfterHeroMgr(JsonObject data,HeroMgr hMgr){
		string MGR_NAME = this.GetType ().Name;
		if (!data.ContainsKey (MGR_NAME)) {
			_parnerData = new JsonObject ();
			data [MGR_NAME] = _parnerData;
			Debug.Log ("No " + MGR_NAME + " data");
		} else {
			_parnerData = (JsonObject)data [MGR_NAME];
			_leaderPos =Convert.ToInt32( _parnerData [PartnerData.LEADER_POS]);

			JsonObject partnerInfos=(JsonObject)_parnerData [PartnerData.PARTNER_INFOS];
			foreach(string pos in partnerInfos.Keys){
				int heroId=Convert.ToInt32( partnerInfos[pos]);
				HeroModel hModel=hMgr.getHero(heroId);
				setPartner(Convert.ToInt32(pos),hModel);
			}
		}

		return true;
	}

	public override bool saveData ()
	{
		_parnerData [PartnerData.LEADER_POS] = _leaderPos;

		JsonObject partnerInfos = new JsonObject ();
		foreach (int pos in _partners.Keys) 
			partnerInfos[pos.ToString()]=_partners[pos].getId();

		_parnerData [PartnerData.PARTNER_INFOS] = partnerInfos;

		SolaSaver.getInstance ().save ();
		return true;
	}

	public void setPartner(int pos,HeroModel heroModel){
		if (_partners.ContainsKey (pos))
			_partners.Remove (pos);

		_partners.Add (pos, heroModel);
		heroModel.setToBattle (true);

		if (_partners.Count == 1) {
			_leaderPos=pos;
		}

		saveData ();
		PartnerEvent.PARTNERS_CHAGED ();
	}

	public void removePartner(int pos){
		if (pos == _leaderPos)
			return;

		_partners[pos].setToBattle (false);
		_partners.Remove (pos);

		saveData ();
		PartnerEvent.PARTNERS_CHAGED ();
	}

	public void removePartner(HeroModel heroModel){
		heroModel.setToBattle (false);

		int removePos=0;
		foreach (int pos in _partners.Keys) {
			if(_partners[pos]==heroModel){
				removePos=pos;
				break;
			}
		}

		_partners.Remove (removePos);

		saveData ();
		PartnerEvent.PARTNERS_CHAGED ();
	}

	public void setLeader(int pos){
		_leaderPos = pos;

		saveData ();
		PartnerEvent.PARTNERS_CHAGED ();
	}

	public Dictionary<int,HeroModel> getPartners(){
		return _partners;
	}
}
