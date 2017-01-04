using UnityEngine;
using System.Collections;

[OpCodeAttribute(opCodes=new int[]{OpCode.TEST_OPCODE})]
public class TestHandler : BaseHandler
{

	public override void handle (int opcode, SimpleJson.JsonObject json)
	{
		Debug.Log ("TestHandler handle");
		TestPacket s = new TestPacket ();
		s.antiSerialization (json);
		Debug.Log ("success");
	}
}
