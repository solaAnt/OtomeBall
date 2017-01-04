using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleDialog : MonoBehaviour {	
	public Dialogue dialogue;

	public BattleActionHead actionHeadItem;
	public GameObject[] actionHeadContainers;

	public BattleStatus[] status;
	public BattleResult resultDialog;

	public Transform ballContainer;
	public BattleBall ballItem;

	public Text goldText;
	public Text paperText;

	private List<Vector3> _partnersPos;

	void Start () {
		BattleEvent.NEXT_ROUND += _onNextRound;
		BattleEvent.ITEM_DROPED += _onNextRound;

		dialogue.onEnded += _onDialogueEnded;

		_partnersPos = new List<Vector3> ();

		_partnersPos.Add (new Vector3 (-250, -200,0));
		_partnersPos.Add (new Vector3 (-140, -95,0));
		_partnersPos.Add (new Vector3 (140, -95,0));
		_partnersPos.Add (new Vector3 (250, -200,0));

		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));

		bMgr.readyForBattle ();

		Dictionary<int,BattleHeroModel> partners = bMgr.getPartners ();
		int size = status.Length;

		for (int i=0; i<size; i++) {
			BattleStatus statu=status[i];

			if(partners.ContainsKey(i)){
				statu.gameObject.SetActive(true);

				BattleHeroModel model=partners[i];
				model.setPos(_partnersPos[i]);
				statu.setHeroModel(model);

				_createHeroBall(model);
				continue;
			}

			statu.gameObject.SetActive(false);
		}

		bMgr.nextRound ();
		_onItemDroped ();
	}

	void OnDestroy(){
		BattleEvent.NEXT_ROUND -= _onNextRound;
		BattleEvent.ITEM_DROPED -= _onNextRound;
	}

	private void _nextAction(BattleHeroModel moveEndModel){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));

		if (bMgr.getMonsters ().Count == 0) {
			bool isWin=bMgr.nextRound ();

			if(isWin==true){
				resultDialog.gameObject.SetActive(true);
				resultDialog.updateView();
			}
			return;
		}

		if (bMgr.isWin() == false&&bMgr.getPartners().Count==0) {
			resultDialog.gameObject.SetActive(true);
			resultDialog.updateView();

			return;
		}

		if (moveEndModel != null)
			bMgr.applyAction (moveEndModel);
		else
			bMgr.resetAction ();

		List<BattleHeroModel> actionSeq=bMgr.getActionSeq ();
		BattleHeroModel actionModel = actionSeq [0];
		actionModel.setAction (true);

		for (int i=0; i<actionSeq.Count; i++) {
			BattleHeroModel model=actionSeq[i];

			GameObject container=actionHeadContainers[i];
			Transform containerTranform=container.transform;

			foreach(Transform child in containerTranform){
				child.gameObject.SetActive(false);
				Destroy(child.gameObject);
			}

			BattleActionHead head=Instantiate(actionHeadItem) as BattleActionHead;
			head.setModel(model);

			head.transform.SetParent(containerTranform);
			head.transform.localScale=new Vector3(1,1,1);
			head.transform.localPosition=new Vector3(0,0,0);
			head.transform.localEulerAngles=new Vector3(0,0,0);
		}

		if (actionModel.isMonster ()) {
//			Debug.Log("actionModel.autoAtk();");
			actionModel.autoAtk();
		}
			
	}

	private void _onItemDroped(){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));

		int gold = bMgr.getGold ();
		int paper = bMgr.getPaper ();

		goldText.text = gold.ToString ();
		paperText.text = paper.ToString ();
	}

	private void _onNextRound(){
		SolaEngine engine = SolaEngine.getInstance ();
		BattleMgr bMgr = (BattleMgr)engine.getMgr (typeof(BattleMgr));

		List<BattleHeroModel> monsters = bMgr.getMonsters ();

		foreach (BattleHeroModel model in monsters) 
			_createHeroBall (model);

		DialogueModel dialogueModel = bMgr.getDialogueModel();
		if (dialogueModel == null) {
			_nextAction (null);
			return;
		}

		dialogue.gameObject.SetActive (true);
		dialogue.setDialogueModel (dialogueModel);
	}

	private void _createHeroBall(BattleHeroModel model){
		BattleBall monsterBall = Instantiate (ballItem) as BattleBall;
		monsterBall.setBattleModel (model);

		monsterBall.transform.SetParent(ballContainer);
		monsterBall.transform.localScale=new Vector3(100,100,1);

		Vector3 point = model.getPos ();
		monsterBall.transform.localPosition=point;

		monsterBall.gameObject.SetActive (true);
		model.setAction (false);
		model.MOVE_END += _nextAction;
	}

	private void _onDialogueEnded(){
		_nextAction (null);
		dialogue.gameObject.SetActive (false);
	}
}
