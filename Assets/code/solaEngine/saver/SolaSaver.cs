using UnityEngine;
using System.Collections;
using SimpleJson;

public class SolaSaver{
	private const string DATA_NAME = "otome.sav";

	private static SolaSaver _instance;
	private JsonObject _data;

	SolaSaver(){
		_data = new JsonObject ();
	}
	
	public static SolaSaver getInstance(){
		if (_instance == null) {
			_instance=new SolaSaver();
		}
		
		return _instance;
	}

	public JsonObject load(){
		ArrayList stringList=SolaFile.LoadAppFile (DATA_NAME);

		if (stringList == null) {
			Debug.Log("No data.");
			_data = new JsonObject ();
		} else {
			string data = (string)stringList [0];
//			Debug.Log ("Data load "+data);
			_data = (JsonObject)SimpleJson.SimpleJson.DeserializeObject (data);
			Debug.Log ("Loaded.");
		}

		return _data;

	}

	public JsonObject save(){
		if (_data == null)
			return null;

		SolaFile.DeleteAppFile (DATA_NAME);

		string content = _data.ToString ();
		SolaFile.CreateAppFile (DATA_NAME, content);
//		Debug.Log ("Data saved."+content);

		return _data;
	}
}
