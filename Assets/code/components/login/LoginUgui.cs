using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoginUgui : MonoBehaviour
{
	public SolaButtonUgui loginBtn;
	public InputField roleIdEdit;
	private int _clickCount = 0;
	private RoleMgr _roleMgr;

	void Start ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		_roleMgr = (RoleMgr)engine.getMgr (typeof(RoleMgr));

		loginBtn.onClicked += _onBtnClicked;

		if (_roleMgr.isSigned ()) {
//			Application.LoadLevel (ScenesName.MAIN);
//			Invoke("_autoLogin",2);
//			engine.send(1);
			return;
		}
		

	}

	void OnApplicationQuit ()
	{
		SocketHelper.GetInstance ().Closed ();
	}

	private void _onBtnClicked (GameObject gameObject)
	{
		_clickCount++;
		string roleId = roleIdEdit.text.Trim ();
		SolaEngine engine = SolaEngine.getInstance ();

		if (false) {
			TestPacket tp2 = new TestPacket ();
			tp2.aa = _clickCount;
			tp2.bb = roleId;

			TestInfoPacket tp3 = new TestInfoPacket ();
			tp3.cc = _clickCount + 1;
			tp3.dd = roleId + "dd";

			tp2.cc = tp3;

			string[] ss = new string[2];
			ss [0] = "asdasd";
			ss [1] = "1111111asdasd";
			tp3.ddd = ss;

			TestInfoPacket[] tp4 = new TestInfoPacket[2];
			TestInfoPacket ssss = new TestInfoPacket ();
			ssss.dd = "ssss";
			tp4 [0] = ssss;

			tp2.ee = tp4;

			TestInfoPacket sssss = new TestInfoPacket ();
			sssss.ddd = ss;
			tp4 [1] = sssss;

			engine.send (OpCode.TEST_OPCODE, tp2.toData ());
			return;
		}

		if (roleId == "") {
			Debug.Log ("请输入账号");
			return;
		}
	
		_roleMgr.sign (roleId);
		string sceneName = ScenesName.MAIN;

		engine.enterScene (sceneName);
		Application.LoadLevel (sceneName);
	}

	private void _autoLogin ()
	{
		SolaEngine engine = SolaEngine.getInstance ();
		string sceneName = ScenesName.MAIN;
		
		engine.enterScene (sceneName);
		Application.LoadLevel (sceneName);
	}
}
