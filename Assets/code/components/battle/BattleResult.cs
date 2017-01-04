using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleResult : MonoBehaviour {
	public Image winTitleImg;
	public Image falseTitleImg;

	public Image[] starImgs;
	public Text roundTxt;

	public SolaButtonUgui nextBtn;

	void Start () {
		nextBtn.onClicked += _onBtnClicked;
	}

	public void updateView(){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr =(BattleMgr) engine.getMgr (typeof(BattleMgr));

		bool isWin = bMgr.isWin ();
		winTitleImg.gameObject.SetActive (isWin);
		falseTitleImg.gameObject.SetActive (!isWin);

		int star = bMgr.getStar ();

		for (int i=0; i<starImgs.Length; i++)
			starImgs [i].gameObject.SetActive (i < star);

		roundTxt.text=bMgr.getActionTime().ToString();
	}

	private void _onBtnClicked(GameObject gameObject){
		SolaEngine engine = SolaEngine.getInstance ();
		string sceneName = engine.BackToScene ();
		Application.LoadLevel (sceneName);
	}
}
