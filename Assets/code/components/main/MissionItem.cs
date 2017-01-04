using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionItem : SolaScrollItem {
	private MissionModel _model;
	public RectTransform rectTranform;
	public SolaButtonUgui _btn;

	public Text titleTxt;
	public Text descTxt;
	public Text statusTxt;
	public const float GAP=30;

	void Start(){
		_btn.onClicked+=_onBtnClicked;
	}

	public void setMission(MissionModel model){
		_model = model;
		_updateView ();
	}

	private void _updateView(){
		MissionModel model = _model;

		titleTxt.text = model.getTitle ();
		descTxt.text = model.getDesc ();

		if(model.getStatus () == MissionModel.status.FINISHED)
			statusTxt.text="已完成";
		else
			statusTxt.text="未完成";
	}

	public override float getHeight(){
		return 100f;
	}

	private void _onBtnClicked(GameObject gameObject){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));
		bMgr.setMissionModel (_model);

		string sceneName = ScenesName.BATTLE;
		engine.enterScene (sceneName);
		Application.LoadLevel (sceneName);
	}
}
