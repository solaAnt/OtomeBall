using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITools
{
	public static Sprite loadSprite(string img){
		Sprite sprite = Resources.Load<Sprite> (img);

		return sprite;
	}
	private const string DEFAULT_STATUS_HEAD = "head/status/BattleStatusHead";

	public static Image createStatusHead(HeroModel heroModel){
		int configId = heroModel.getConfigId ();

		Object obj = Resources.Load<Image> (DEFAULT_STATUS_HEAD + configId);
		if (obj == null)
			return null;
		
		Image head = MonoBehaviour.Instantiate (obj) as Image;
		
		return head;
	}

	private const string DEFAULT_ACTION_HEAD_BG = "head/action/Head";

	public static Image createActionHead (BattleHeroModel model)
	{
		HeroModel hModel = model.getHeroModel ();
		int configId = hModel.getConfigId ();

		Object obj = Resources.Load<Image> (DEFAULT_ACTION_HEAD_BG + configId);
		if (obj == null)
			return null;

		Image head = MonoBehaviour.Instantiate (obj) as Image;

		return head;
	}

	private const string DEFAULT_BATTLE_BALL = "ball/BodyImg";

	public static Image createBallImg (BattleHeroModel model)
	{
		int configId = model.getHeroModel ().getConfigId ();
		Object ballObject = Resources.Load<Image> (DEFAULT_BATTLE_BALL + configId);

		if (ballObject == null)
			return null;

		Image ball = MonoBehaviour.Instantiate (ballObject) as Image;

		return ball;
	}
}
