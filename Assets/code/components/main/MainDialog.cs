using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainDialog : MonoBehaviour
{
	private SolaEngine _engine;
	private RoleMgr _roleMgr;
	public Text roleNameText;
	public Slider roleHpSlider;
	public Slider roleExpSlider;
	public Text goldText;
	public Text paperText;
	public Image[] heroImgs;
	public Image[] heroStars;
	public Text heroName;
	public Slider heroAtkSlider;
	public Slider heroHpSlider;
	public Slider heroSpdSlider;
	public Text heroAtkText;
	public Text heroHpText;
	public Text heroSpdkText;
	public SolaScroll itemScroll;
	public MissionItem missionItem;
	public SolaButtonUgui storyBtn;
	
	void Start ()
	{
		storyBtn.onClicked += _onStoryClicked;

		_updateRoleView ();
		_updateHeroView ();
		_updateMissionView ();
	}

	private void _onStoryClicked (GameObject gameObject)
	{
		SolaEngine engine = SolaEngine.getInstance ();
		string sceneName = ScenesName.MAP;
		
		engine.enterScene (sceneName);
		Application.LoadLevel (sceneName);
	}

	private void _updateRoleView ()
	{
		_engine = SolaEngine.getInstance ();
		_roleMgr = (RoleMgr)_engine.getMgr (typeof(RoleMgr));
		
		roleNameText.text = _roleMgr.getRoleId ();
		int roleLevel = _roleMgr.getRoleLevel ();
		
		roleHpSlider.minValue = 0;
		int maxHp = roleLevel * 10 + 50;
		roleHpSlider.maxValue = maxHp;
		roleHpSlider.value = 30;
		
		roleExpSlider.minValue = 0;
		int maxExp = _roleMgr.getRoleMaxExp ();
		roleExpSlider.maxValue = maxExp;
		roleExpSlider.value = maxExp / 3 * 2;

		heroAtkSlider.maxValue = 999;
		heroHpSlider.maxValue = 999;
		heroSpdSlider.maxValue = 99;
	}

	private void _updateHeroView ()
	{
		foreach (Image image in heroImgs)
			image.gameObject.SetActive (false);

		PartnerMgr pMgr = (PartnerMgr)_engine.getMgr (typeof(PartnerMgr));
		int index = 0;

		Dictionary<int,HeroModel> partners = pMgr.getPartners ();
		int heroIndex = 0;
		foreach (HeroModel model in partners.Values) {
			if (heroIndex > 2)
				break;

			heroIndex++;

			string img = model.getBodyImg ();
			Sprite bodySprite = Resources.Load<Sprite> (img);

			Image image = heroImgs [index];
			image.gameObject.SetActive (true);
			image.sprite = bodySprite;

			if (index == 0) {
				int start = model.getStart ();
				int size = heroStars.Length;

				for (int i=0; i<size; i++) {
					GameObject gameObject = heroStars [i].gameObject;
					gameObject.SetActive (i < start);
				}

				heroName.text = model.getName ();

				int atk = model.getAtk ();
				int hp = model.getHp ();
				int spd = model.getSpd ();

				heroAtkSlider.value = atk;
				heroHpSlider.value = hp;
				heroSpdSlider.value = spd;

				heroAtkText.text = atk.ToString ();
				heroHpText.text = hp.ToString ();
				heroSpdkText.text = spd.ToString ();
			}

			index++;
		}
	}

	private void _updateMissionView ()
	{
		MissionMgr mMgr = (MissionMgr)_engine.getMgr (typeof(MissionMgr));
		Dictionary<int,MissionModel> missions = mMgr.getMissions ();

		itemScroll.reset ();
		foreach (MissionModel model in missions.Values) {
			MissionItem item = (MissionItem)MonoBehaviour.Instantiate (missionItem);
			item.setMission (model);
			itemScroll.addItem (item);
		}

		missionItem.gameObject.SetActive (false);
	}
}
