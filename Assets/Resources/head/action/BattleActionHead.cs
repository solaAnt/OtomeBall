using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleActionHead : MonoBehaviour {
	private BattleHeroModel _model;
	public Image headContainer;
	public Image headImg;

	public void setModel(BattleHeroModel model){
		_model = model;

		_updateView ();
	}

	private void _updateView(){
		BattleHeroModel model = _model;

		Image head = UITools.createActionHead (model);
		if (head == null) {
			string img = model.getHeroModel ().getBodyImg ();

			Sprite bodySprite = Resources.Load<Sprite> (img);
			headImg.sprite = bodySprite;
		} else {
			headImg.gameObject.SetActive(false);

			headImg=head;
			headImg.transform.SetParent(headContainer.transform);
			headImg.transform.localEulerAngles=new Vector3(0,0,0);
		}
	}
}
