using UnityEngine;
using System.Collections;

public class SolaScroll : MonoBehaviour {
	public Transform itemContainer;
	public float itemGap;
	
	public void reset(){
		_curIndex = 0;
		_height = 0;
	}

	public void addItem(SolaScrollItem item){
		item.transform.SetParent(itemContainer);
		item.transform.localScale = new Vector3 (1, 1, 1);
		
		_curIndex++;
		_height += (item.getHeight () + itemGap);

		RectTransform rt = itemContainer.GetComponent<RectTransform> ();
		Vector2 oldSize = rt.sizeDelta;
		oldSize.y = _height;
		rt.sizeDelta = oldSize;
	}
	
	private int _curIndex;
	private float _height;
}