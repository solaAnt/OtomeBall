using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger{
	public delegate void VoidDelegate (GameObject go,PointerEventData pointEventData,BaseEventData baseEventData);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;
	public VoidDelegate onDrag;

	static public EventTriggerListener Get (GameObject go){
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}

	public override void OnPointerClick(PointerEventData eventData){
		if(onClick != null)
			onClick(gameObject,eventData,null);
	}

	public override void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject,eventData,null);
	}

	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject,eventData,null);
	}

	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject,eventData,null);
	}

	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject,eventData,null);
	}

	public override void OnDrag(PointerEventData eventData){
		if(onDrag != null) onDrag(gameObject,eventData,null);
	}

	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject,null,eventData);
	}

	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject,null,eventData);
	}
}