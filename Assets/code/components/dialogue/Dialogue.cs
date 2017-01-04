using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogue : MonoBehaviour {
	public delegate void VoidDelegate();
	public VoidDelegate onEnded;

	public SolaButtonUgui skipBtn;
	public SolaButtonUgui continueBtn;

	public Image leftBodyImg;
	public Image rightBodyImg;

	public Text speakerName;
	public Text speakContent;

	private DialogueModel _model;

	void Start(){
		skipBtn.onClicked += _onSkipClicked;
		continueBtn.onClicked += _onContinueClicked;
	}

	public void setDialogueModel(DialogueModel model){
		_model = model;
		
		_updateView ();
	}
	
	private void _updateView(){
		DialogueModel model = _model;
		if (model.isEnded() == true) {
			if(onEnded!=null)
				onEnded();

			return;
		}

		DialogueInfoModel infoModel= model.next ();
		
		string content = infoModel.getContent ();
		speakContent.text = content;
		
		HeroModel speakerModel = infoModel.getSperkerModel ();
		speakerName.text = speakerModel.getName ();
		
		HeroModel leftModel = infoModel.getLeftModel ();
		_updateImg(leftModel,leftBodyImg);
		
		HeroModel rightModel = infoModel.getRightModel ();
		_updateImg(rightModel,rightBodyImg);
	}
	
	private void _updateImg(HeroModel model,Image image){
		if (model != null) {
			image.gameObject.SetActive(true);
			
			string img = model.getBodyImg ();
			Sprite sprite = UITools.loadSprite (img);
			
			image.sprite = sprite;
		} else {
			image.gameObject.SetActive(false);
		}
	}

	private void _onSkipClicked(GameObject gameObject){
		Destroy (gameObject);
	}

	private void _onContinueClicked(GameObject gameObject){
		_updateView ();
	}
}
