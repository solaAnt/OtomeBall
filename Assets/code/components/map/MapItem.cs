using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapItem : MonoBehaviour {
	public SolaButtonUgui selfBtn;
	public Text nameTxt;

	public void setMapModel(MapModel model,ToBattleDialog toBattleDialog,MapDialog mapDialog){
		_mapModel = model;
		_toBattleDialog = toBattleDialog;
		_mapDialog = mapDialog;

		_updateView ();
	}

	private MapDialog _mapDialog;
	private ToBattleDialog _toBattleDialog;
	private MapModel _mapModel;

	void Start () {
		selfBtn.onClicked+=_onSelfClicked;
	}
	
	private void _onSelfClicked(GameObject gameObject){
		ToBattleDialog toBattleDialog=_toBattleDialog;
		toBattleDialog.gameObject.SetActive (true);
		toBattleDialog.setModel (_mapModel);

		_mapDialog.gameObject.SetActive (false);
	}

	private void _updateView(){
		MapModel model = _mapModel;

		nameTxt.text = model.getName ();
	}
}
