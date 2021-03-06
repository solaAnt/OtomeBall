using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Net;  
using System.Net.Sockets;  
using System.Threading;  
using UnityEngine;

public class SocketHelper
{  
	private static SocketHelper _socketHelper;
	private static Dictionary<int,List<BaseHandler>> _handlers;
	private Socket _socket;
	private Thread _thread;

	public static SocketHelper GetInstance ()
	{  
		if (_socketHelper == null) {
			_socketHelper = new SocketHelper ();
		}
		return _socketHelper;  
	}

	public void setHandlers (Dictionary<int,List<BaseHandler>> handlers)
	{
		_handlers = handlers;
	}

	public void Closed ()
	{  
		if (_socket != null && _socket.Connected) {  
			_socket.Shutdown (SocketShutdown.Both);  
			_socket.Close ();  
		}  
		_socket = null;  
		_thread.Abort ();

	}
	
	public void SendMessage (string str)
	{  
		List<byte> dataList = new List<byte> ();
		byte[] bytes = BitConverter.GetBytes (12);
		for (int i=0; i<bytes.Length; i++)
			dataList.Add (bytes [i]);

		byte[] msg = Encoding.UTF8.GetBytes (str);  
		for (int i=0; i<msg.Length; i++)
			dataList.Add (msg [i]);

		dataList.ToArray ();
		if (!_socket.Connected) {  
			Debug.Log ("socket 关闭了  ");
			_socket.Close ();  
			return;  
		}  
		try {  
			IAsyncResult asyncSend = _socket.BeginSend (dataList.ToArray (), 0, dataList.Count, SocketFlags.None, new AsyncCallback (SendCallback), _socket);  
			bool success = asyncSend.AsyncWaitHandle.WaitOne (5000, true);  
			if (!success) {  
				_socket.Close ();  
				Debug.Log ("Failed to SendMessage server.");  
			}
		} catch {  
			Debug.Log ("send message error");  
		}  
	}

	private SocketHelper ()
	{  
		_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

		IPAddress address = IPAddress.Parse ("192.168.1.115");   
		IPEndPoint endpoint = new IPEndPoint (address, 10000);  
		IAsyncResult result = _socket.BeginConnect (endpoint, new AsyncCallback (ConnectCallback), _socket);  

		bool success = result.AsyncWaitHandle.WaitOne (5000, true);  
		if (!success) {  
			Closed ();  
			Debug.Log ("connect Time Out");  
		} else {  
			_thread = new Thread (new ThreadStart (ReceiveSorket));  
			_thread.IsBackground = true;  
			_thread.Start ();  
		}  
		
	}
	
	private void ConnectCallback (IAsyncResult asyncConnect)
	{  
		Debug.Log ("connect success");  
	}
	
	private void ReceiveSorket ()
	{  
		while (true) {  
			
			if (!_socket.Connected) {  
				Debug.Log ("Failed to clientSocket server.");  
				_socket.Close ();  
				break;  
			}  
			try {    
				byte[] bytes = new byte[4096];   
				int i = _socket.Receive (bytes);  
				if (i > 0) {
					string pack = Encoding.UTF8.GetString (bytes);
					Debug.Log (pack);
					SimpleJson.JsonObject json = (SimpleJson.JsonObject)SimpleJson.SimpleJson.DeserializeObject (pack);

					int opCode = Convert.ToInt32 (json ["opcode"]);
					List<BaseHandler> bHandler = _handlers [opCode];

					for (int j=0; j<bHandler.Count; j++) {
						BaseHandler bh = bHandler [j];
						bh.handle (opCode, json);
					}
				}
					
			} catch (Exception e) {  
				Debug.Log ("Failed to clientSocket error." + e);  
				Closed ();
				break;  
			}  
		}  
	}

	private void SendCallback (IAsyncResult asyncConnect)
	{  
		Debug.Log ("send success");  
	}  
	
	
}  