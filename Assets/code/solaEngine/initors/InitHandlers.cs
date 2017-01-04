using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitHandlers {
	private Dictionary<int,List<BaseHandler>> _handlers = new Dictionary<int, List<BaseHandler>> ();

	public bool init(){
		TestHandler testHandler = new TestHandler ();
		loadHandler (testHandler.GetType (), testHandler);

		return true;
	}

	public Dictionary<int,List<BaseHandler>> getHandlers(){
		return _handlers;
	}
	
	private void loadHandler(System.Type type,BaseHandler handler){

		object[] objs = type.GetCustomAttributes(typeof(OpCodeAttribute), true);
		foreach (object obj in objs)
		{
			OpCodeAttribute attr = obj as OpCodeAttribute;
			if (attr != null)
			{
				int[] opcodes=attr.opCodes;
				for(int i=0;i<opcodes.Length;i++){
					int curOpcode=opcodes[i];
					List<BaseHandler> handlerList=null;

					if(!_handlers.ContainsKey(curOpcode)){
						handlerList=new List<BaseHandler>();
						_handlers[curOpcode]=handlerList;
					}else
						handlerList=_handlers[curOpcode];

					handlerList.Add(handler);
				}
			}
		}
	}
}
