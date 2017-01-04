using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitPackets {
	private Dictionary<int,BasePacket> _packets = new Dictionary<int, BasePacket> ();

	public bool init(){
		TestPacket packet = new TestPacket ();
		loadHandler (packet.GetType (), packet);

		return true;
	}

	public Dictionary<int,BasePacket> getPackets(){
		return _packets;
	}

	private void loadHandler(System.Type type,BasePacket packet){
//		object[] objs = type.GetCustomAttributes(typeof(OpCodeAttribute), true);
//		foreach (object obj in objs)
//		{
//			OpCodeAttribute attr = obj as OpCodeAttribute;
//			if (attr != null)
//			{
//				int[] opcode=attr.opCodes;
//				_packets.Add(opcode,packet);
//				break;
//			}
//		}
	}
}
