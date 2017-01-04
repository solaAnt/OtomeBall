using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine.UI;
using SimpleJson;

class DownLoadInfo
{
	public string name = "";
	public int index = 1;
	public JsonObject json = null;
	public bool isJpg = true;
	public string comicName = "";
}

public class HttpTest : MonoBehaviour
{
	public InputField nameInput;
	public InputField minIndexInput;
	public InputField maxIndexInput;
	public InputField comicNameInput;
	public SolaButtonUgui addBtn;
	public SolaButtonUgui clearBtn;
	public SolaButtonUgui startBtn;
	public SolaButtonUgui stopBtn;
	public Text logText;
	//===========================
	private List<DownLoadInfo> _downloadInfos = new List<DownLoadInfo> ();
	private WWW _www;
	private string _filePath;
	private Texture2D _texture2D;
	private JsonObject _json = new JsonObject ();
	private JsonArray _jsonArr = null;
	private static string LIST_KEY = "LIST_KEY";
	private static string SAVE_FILE_NAME = "DW_SAVE";
	//===
	private static string NAME_KEY = "NAME_KEY";
	private static string MIN_INDEX_KEY = "MIN_INDEX_KEY";
	private static string MAX_INDEX_KEY = "MAX_INDEX_KEY";
	private static string INDEX_KEY = "INDEX_KEY";
	private static string IS_JPG_KEY = "IS_JPG_KEY";
	private static string COMIC_NAME_KEY = "COMIC_NAME_KEY";
	private static string SDCARD_PATH = "/mnt/sdcard1/.comic/";
	private static string SDCARD_COMIC_PATH = "/mnt/sdcard1/comic/";
	//===
	//private bool _isStarted = false;
	private List<string> _logInfo = new List<string> ();
	private AndroidJavaObject _javaObj;

	void Start ()
	{
		Application.targetFrameRate = 20;
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		_javaObj = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		_javaObj.Call ("testCall", "testCall");
		_javaObj.Call ("setPath", SDCARD_PATH);
		_javaObj.Call ("setZipPath", SDCARD_COMIC_PATH);
		#endif
		load ();
		
		addBtn.onClicked += _onAddClicked;
		clearBtn.onClicked += _onClearClicked;
		startBtn.onClicked += _onStartClicked;
		stopBtn.onClicked += _onStopClicked;

		#if UNITY_STANDALONE_WIN
		Application.runInBackground = true;
		#endif

		stopBtn.gameObject.SetActive (false);

		getUrlContent ("http://nhentai.net/g/159190/");
	}

	private void _log (string content)
	{
		if (_logInfo.Count > 30) 
			_logInfo.RemoveAt (0);

		_logInfo.Add (content);

		string result = "";
		for (int i=0; i<_logInfo.Count; i++) {
			result = result + (string)_logInfo [i] + "\r\n";
		}

		logText.text = result;
	}

	private void _error (string content)
	{
		_log (content);
	}

	private void load ()
	{
		ArrayList stringList = SolaFile.LoadAppFile (SAVE_FILE_NAME);
		
		if (stringList == null) {
			_log ("No data.");
			_jsonArr = new JsonArray ();
			_json [LIST_KEY] = _jsonArr;

		} else {
			string data = (string)stringList [0];
			_json = (JsonObject)SimpleJson.SimpleJson.DeserializeObject (data);
			_jsonArr = (JsonArray)_json [LIST_KEY];

			JsonArray ja = _jsonArr;
			for (int i=0; i<ja.Count; i++) {
				JsonObject json = (JsonObject)ja [i];
				DownLoadInfo info = new DownLoadInfo ();
				info.name = (string)json [NAME_KEY];
				info.index = (int)json [INDEX_KEY];
				info.isJpg = (bool)json [IS_JPG_KEY];
				info.comicName = (string)json [COMIC_NAME_KEY];

				info.json = json;
				_downloadInfos.Add (info);
				_javaObj.Call ("addDownLoad", json.ToString ());
			}
			_log ("Loaded.");
		}
	}

	private void _clearData ()
	{
		_jsonArr.Clear ();
		_downloadInfos.Clear ();
		save ();

		#if UNITY_ANDROID
		_javaObj.Call ("clearDownLoad", "asd");
		#endif
	}

	private void save ()
	{
		SolaFile.DeleteAppFile (SAVE_FILE_NAME);
		
		string content = _json.ToString ();
		SolaFile.CreateAppFile (SAVE_FILE_NAME, content);
	}

	private void _onAdded(String content){
		JsonObject json2 =(JsonObject) SimpleJson.SimpleJson.DeserializeObject (content);
		string name =(string) json2 [NAME_KEY];
		int minIndex =(int) json2 [MIN_INDEX_KEY];
		int maxIndex=(int)json2 [MAX_INDEX_KEY];
		string comicName = (string)json2 [COMIC_NAME_KEY];

		for (int i=minIndex; i<(maxIndex+1); i++) {
			JsonObject json = new JsonObject ();
			json [NAME_KEY] = name;
			json [INDEX_KEY] = i;
			json [COMIC_NAME_KEY] = comicName;
			_jsonArr.Add (json);

			DownLoadInfo info = new DownLoadInfo ();
			info.name = name;
			info.index = i;
			info.comicName = comicName;
			info.json = json;

			json [IS_JPG_KEY] = info.isJpg;

			_downloadInfos.Add (info);
			_log ("add " + name + " " + i);
		}

		save ();
	}

	private void _addDownLoad (string name,string minIndex, string maxIndex, string comicName)
	{
		JsonObject json = new JsonObject ();
		json [COMIC_NAME_KEY] = comicName;
		json [MIN_INDEX_KEY] = Int32.Parse(minIndex);
		json [MAX_INDEX_KEY] = Int32.Parse(maxIndex);
		#if UNITY_ANDROID
		_javaObj.Call ("addDl2", json.ToString ());
		#endif
	}

	private void _removeDownload (String content)
	{
//		_log ("_removeDownload " + content);
		string[] info = content.Split (new char[]{'|'});
		string name = info [0];
		int index = Int32.Parse (info [1]);

		DownLoadInfo dInfo = null;
		for (int i=0; i<_downloadInfos.Count; i++) {
			DownLoadInfo curInfo = _downloadInfos [i];
//			_log (curInfo.name);
//			_log (name);
//			_log (curInfo.index + "");
//			_log (index + "");
			if (curInfo.name == name && curInfo.index == index) {
				_log ("find " + content);
				dInfo = curInfo;
				break;
			}
		}

		if (dInfo == null) {
			_log ("not find " + content);
			return;
		}
			
		_log ("_removeDownload " + content);
		_jsonArr.Remove (dInfo.json);
		_downloadInfos.Remove (dInfo);
		save ();
	}

	private void _onStartClicked (GameObject gameObject)
	{
		_log ("_onStartClicked");
		startBtn.gameObject.SetActive (false);

		#if UNITY_ANDROID
		_javaObj.Call ("startDownload", "");
		#endif
		#if UNITY_STANDALONE_WIN
		StartCoroutine (LoadImg ());
		#endif
	}

	private void _onStopClicked (GameObject gameObject)
	{
		stopBtn.gameObject.SetActive (false);
		startBtn.gameObject.SetActive (true);

		_javaObj.Call ("stopDownload", "");
	}

	private void _onClearClicked (GameObject gameObject)
	{
		_clearData ();
	}

	private void _onAddClicked (GameObject gameObject)
	{
		_log ("_onAddClicked");
		string name = nameInput.text.Trim ();
		if (name == "") {
			_log ("name null");
			//return;
		}

		string[] ssd = name.Split (new char[]{'/'});
		if (ssd.Length > 1) {
			name = ssd [ssd.Length - 2];
			_log (ssd [ssd.Length - 2]);
		} 
					
		string minIndex = minIndexInput.text.Trim ();
		if (minIndex == "") {
			_log ("minIndex default 1");
			minIndex = "1";
//			return;
		}

		string maxIndex = maxIndexInput.text.Trim ();
		if (maxIndex == "") {
			_log ("maxIndex null");
			return;
		}

		string comicName = comicNameInput.text.Trim ();
		if (comicName == "")
			comicName = name;

		_addDownLoad (name, minIndex,maxIndex, comicName);
	}

	private void writeFile (DownLoadInfo info, int index)
	{
//		_log ("写文件");
		_texture2D = _www.texture;
		byte[] bytes;
		string fix;
		if (info.isJpg) {
			fix = ".jpg"; 
			bytes = _texture2D.EncodeToJPG ();
		} else {
			fix = ".png"; 
			bytes = _texture2D.EncodeToPNG ();
		}
		
		string dir = "c:\\comic\\" + info.comicName;
		if (Application.platform == RuntimePlatform.Android)
			dir = SDCARD_PATH + info.comicName;

		string indexStr = "";

		if (index < 10)
			indexStr = "00" + index;
		else if (index < 100)
			indexStr = "0" + index;
		else
			indexStr = "" + index;

		string path = dir + Path.DirectorySeparatorChar + indexStr + fix; 

//		_log ("路径" + path);
		if (Application.platform == RuntimePlatform.Android) {
			Directory.CreateDirectory (dir);
		} else {
			DirectoryInfo dirInfo = new DirectoryInfo (dir);
			dirInfo.Create ();
		}
		
		File.WriteAllBytes (path, bytes);
		_log (path + "完成下载");

		_jsonArr.Remove (info.json);
		_downloadInfos.Remove (info);
		save ();

		Destroy (_texture2D);
	}

	IEnumerator LoadImg ()
	{ 
		if (_downloadInfos.Count == 0) {
			yield return new WaitForSeconds (1f);
			StartCoroutine (LoadImg ());
			yield break;
		}

		DownLoadInfo info = _downloadInfos [_downloadInfos.Count - 1];
		int index = info.index;
		string suffix = ".jpg";
		if (!info.isJpg)
			suffix = ".png";

		string url = "http://i.nhentai.net/galleries/" + info.name + "/" + index + suffix;

		_log ("开始下载" + url);
		_www = new WWW (url);
		yield return _www;

		if (_www.error != null) {
			if (info.isJpg == false) {
				_log ("download failure " + url);
				_jsonArr.Remove (info.json);
				_downloadInfos.Remove (info);
				save ();
			} else {
				info.json [IS_JPG_KEY] = false;
				info.isJpg = false;
				save ();
			}
		} else {
			writeFile (info, index);
		}

		StartCoroutine (LoadImg ());
	}

	private void getUrlContent (string url)
	{
		string rXml = string.Empty;
		HttpWebRequest myHttpWebRequest = System.Net.WebRequest.Create (url) as HttpWebRequest;
		myHttpWebRequest.KeepAlive = false;
		myHttpWebRequest.AllowAutoRedirect = false;
		myHttpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
		myHttpWebRequest.Timeout = 50000;
		myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
		using (HttpWebResponse res = (HttpWebResponse)myHttpWebRequest.GetResponse()) {
//			if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.PartialContent) {//返回为200或206
			string dd = res.ContentEncoding;
			System.IO.Stream strem = res.GetResponseStream ();
			System.IO.StreamReader r = new System.IO.StreamReader (strem);
			rXml = r.ReadToEnd ();
//			}
		}

		Debug.Log ("rXml" + rXml);
	}
}