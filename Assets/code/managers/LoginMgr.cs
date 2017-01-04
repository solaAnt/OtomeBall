using UnityEngine;
using System.Collections;
using SimpleJson;

public class LoginMgr : BaseMgr {
	public override bool init(){
		return true;
	}
	
	public override bool inited ()
	{
		return true;
	}

	public override bool loadData(JsonObject data){
		return true;
	}

	public override bool saveData ()
	{
		return true;
	}
}
