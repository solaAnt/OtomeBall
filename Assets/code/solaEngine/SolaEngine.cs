using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class SolaEngine
{
	public static SolaEngine getInstance ()
	{
		if (_instance == null) {
			_instance = new SolaEngine ();
			_instance._startUp ();
		}
		
		return _instance;
	}

	public void enterScene (string sceneName)
	{
		_scenesSequence.Add (_curSceneName);

		_curSceneName = sceneName;
	}

	public string BackToScene ()
	{
		int lastIndex = _scenesSequence.Count - 1;
		string lastSceneName = _scenesSequence [lastIndex];

		_scenesSequence.RemoveAt (lastIndex);
		_curSceneName = lastSceneName;
		return lastSceneName;
	}

	public int randomInt (int min, int max)
	{
		return _random.Next (min, max);
	}

	public object getMgr (System.Type mgrType)
	{
		return _mgrs [mgrType.Name];
	}

	public object getCfg (string cfgName)
	{
		return _cfgs [cfgName];
	}

	public void send (int opCode, JsonObject packetData)
	{
		SimpleJson.JsonObject json = (SimpleJson.JsonObject)SimpleJson.SimpleJson.DeserializeObject (packetData.ToString ());
		json ["opcode"] = opCode;

		SocketHelper.GetInstance ().SendMessage (json.ToString ());
	}

	public void send (int opCode, BasePacket packet)
	{
		JsonObject json = packet.toData ();
		json ["opcode"] = opCode;
		
		SocketHelper.GetInstance ().SendMessage (json.ToString ());
	}

	public void closeSocket ()
	{
		SocketHelper.GetInstance ().Closed ();
	}

	private static SolaEngine _instance;
	private static Dictionary<string,object> _mgrs;
	private static Dictionary<string,object> _cfgs;
	private System.Random _random = new System.Random ();
	private List<string> _scenesSequence = new List<string> ();
	private string _curSceneName;

	private bool _startUp ()
	{
		_curSceneName = ScenesName.LOGIN;

		InitHandlers initHandlers = new InitHandlers ();
		initHandlers.init ();
//		SocketHelper.GetInstance ().setHandlers (initHandlers.getHandlers ());

		InitEvents initEvents = new InitEvents ();
		initEvents.init ();

		InitConfigs initCfgs = new InitConfigs ();
		initCfgs.init ();
		_cfgs = initCfgs.getCfgs ();

		InitMgrs initMgr = new InitMgrs ();
		initMgr.init ();
		_mgrs = initMgr.getMgrs ();
		initMgr.inited ();

		SolaSaver saver = SolaSaver.getInstance ();
		JsonObject data = saver.load ();

		foreach (BaseMgr mgr in _mgrs.Values)
			mgr.loadData (data);

		return true;
	}
}

