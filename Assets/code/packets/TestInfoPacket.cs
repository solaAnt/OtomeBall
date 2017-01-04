using UnityEngine;
using System.Collections;

[OpCodeAttribute(opCodes=new int[]{OpCode.TEST_OPCODE})]
public class TestInfoPacket : BasePacket {
//	public int testInt;
//	public int[] testIntList;
//
//	public string testString;
//	public string[] testStringList;
//
//	public bool testBool;
//
//	public TestPacket_end end;

	public int cc;
	public string dd;
	public string[] ddd;
}
