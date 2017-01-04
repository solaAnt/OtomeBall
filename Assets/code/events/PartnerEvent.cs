using UnityEngine;
using System.Collections;

public class PartnerEvent {
	public delegate void VoidDelegate ();
	public static VoidDelegate PARTNERS_CHAGED;

	public static void init(){
		PARTNERS_CHAGED += initVoid;
	}

	private static void initVoid(){}
}
