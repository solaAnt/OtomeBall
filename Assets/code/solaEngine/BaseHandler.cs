using UnityEngine;
using System.Collections;

public abstract class BaseHandler
{
	public abstract void handle (int opcode, SimpleJson.JsonObject json);
}
