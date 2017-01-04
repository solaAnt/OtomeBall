using UnityEngine;
using System.Collections;

public abstract class BaseConditionNode {
	public string name{ get; set;}

	public virtual bool isSatisfied(){
		return true;
	}
}
