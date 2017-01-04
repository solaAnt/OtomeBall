using UnityEngine;
using System.Collections;
using SimpleJson;

public abstract class BaseMgr  {
	public abstract bool init();

	public abstract bool inited();

	public abstract bool loadData(JsonObject data);

	public abstract bool saveData();
}
