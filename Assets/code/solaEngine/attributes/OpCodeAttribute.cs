using UnityEngine;
using System.Collections;

public class OpCodeAttribute : System.Attribute {
	private int[] _opCode;
	
	public int[] opCodes
	{
		get { return _opCode; }
		set { _opCode = value; }
	}
}
