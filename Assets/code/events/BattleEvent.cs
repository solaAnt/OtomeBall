using UnityEngine;
using System.Collections;

public class BattleEvent {
	public delegate void VoidDelegate ();
	public static VoidDelegate NEXT_ROUND;
	public static VoidDelegate ITEM_DROPED;

	public static void init(){
		NEXT_ROUND += initVoid;
		ITEM_DROPED += initVoid;
	}

	private static void initVoid(){}
}
