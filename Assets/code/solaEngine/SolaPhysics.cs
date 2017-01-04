using UnityEngine;
using System.Collections;

public class SolaPhysics
{	 
	public static float sin(Vector3 basePoint,Vector3 reference){
		Vector3 baseP = new Vector3 (basePoint.x,basePoint.y,0);
		Vector3 referP = new Vector3 (reference.x,reference.y,0);
		float distance= Vector3.Distance (baseP, referP);

		return (reference.y-basePoint.y)/distance;
	}

	public static float cos(Vector3 basePoint,Vector3 reference){
		Vector3 baseP = new Vector3 (basePoint.x,basePoint.y,0);
		Vector3 referP = new Vector3 (reference.x,reference.y,0);
		float distance= Vector3.Distance (baseP, referP);
		
		return (reference.x-basePoint.x)/distance;
	}
}
