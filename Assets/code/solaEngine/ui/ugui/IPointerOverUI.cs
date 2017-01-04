using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IPointerOverUI
{
	
	private static IPointerOverUI instance = new IPointerOverUI ();

	public static IPointerOverUI Instance {
		get {
			return instance;
		}
	}
	
	public bool IsPointerOverUIObject (Canvas canvas, Vector2 screenPosition)
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = screenPosition;
		
		GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster> ();
		List<RaycastResult> results = new List<RaycastResult> ();
		uiRaycaster.Raycast (eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public bool IsPointerOverUIObject ()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public bool IsPointerOverUIObject (GameObject gameObject)
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);

		foreach (RaycastResult rr in results) {
			if(rr.gameObject==gameObject)
				return true;
		}

		return false;
	}

	public bool IsTouchObject (Touch finger,GameObject gameObject)
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		
		eventDataCurrentPosition.position = finger.position;
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		foreach (RaycastResult rr in results) {
			if(rr.gameObject==gameObject)
				return true;
		}
		
		return false;
	}
}