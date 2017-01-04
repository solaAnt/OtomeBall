using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapDialog : MonoBehaviour {

	public Text roundTxt;
	public SolaButtonUgui backBtn;

	public Image[] actionImg;
	public MapItem[] mapItems;

	public ToBattleDialog toBattleDialog;

	void Start () {
		backBtn.onClicked += _onBackClicked;

		SolaEngine engine = SolaEngine.getInstance ();
		MapMgr mMgr = (MapMgr)engine.getMgr (typeof(MapMgr));

		for (int i=0; i<mapItems.Length; i++) {
			MapItem mapItem=mapItems[i];
			MapModel mapModel=mMgr.getMapModel(i+1);

			mapItem.setMapModel(mapModel,toBattleDialog,this);
		}
	}

	private void _onBackClicked(GameObject src){
		SolaEngine engine = SolaEngine.getInstance ();
		string sceneName = engine.BackToScene ();
		Application.LoadLevel (sceneName);
	}

	private void _onActionEd(GameObject src){

	}

	private void _onRoundEnd(GameObject src){

	}
}
