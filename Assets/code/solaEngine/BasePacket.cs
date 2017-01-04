using UnityEngine;
using System.Collections;
using System.Reflection;
using SimpleJson;
using System;

public class BasePacket
{
	public JsonObject toData ()
	{
		JsonObject json = new JsonObject ();
		System.Type t = this.GetType ();
		FieldInfo[] fis = t.GetFields ();
		
		foreach (FieldInfo fi in fis) {	
			string fieldName = fi.Name;

			object value = fi.GetValue (this);
			if (value == null)
				continue;

			System.Type ft = value.GetType ();
			if (ft.IsPrimitive || ft.FullName == "System.String")
				json [fieldName] = value;
			else if (ft.IsArray) {
				Type itemClass = ft.GetElementType ();
				if (itemClass.IsPrimitive || itemClass.FullName == "System.String") {
					json [fieldName] = value;
				} else {
					BasePacket[] bp = (BasePacket[])value;
					JsonArray ja = new JsonArray ();
					for (int i = 0; i < bp.Length; i++) {
						ja.Add (bp [i].toData ());
					}
					json [fieldName] = ja;
				}
			} else {
				json [fieldName] = ((BasePacket)value).toData ();
		
			}
		}

		return json;
	}

	public void antiSerialization (JsonObject json)
	{
		System.Type t = this.GetType ();
		FieldInfo[] fis = t.GetFields ();

		foreach (FieldInfo field in fis) {
			string fieldName = field.Name;
			if (!json.ContainsKey (fieldName))
				continue;

			System.Type ftype = field.FieldType;
			object value = json [fieldName];

			if (ftype.IsPrimitive || ftype.FullName == "System.String")
				field.SetValue (this, value);
			else if (ftype.IsArray) {
				Type itemClass = ftype.GetElementType ();
				JsonArray ja = (JsonArray)json [fieldName];
				Debug.Log (itemClass.FullName);
				if (itemClass.FullName == "System.String") {
					string[] il = new string[ja.Count];
					for (int i = 0; i < il.Length; i++)
						il [i] = (string)ja [i];
					field.SetValue (this, il);
				} else if (itemClass.FullName == "System.Int32") {
					int[] il = new int[ja.Count];
					for (int i = 0; i < il.Length; i++)
						il [i] = (int)ja [i];
					field.SetValue (this, il);

				} else {
					Array il = Array.CreateInstance (itemClass, ja.Count);
					for (int i = 0; i < il.Length; i++) {
						JsonObject jo = (JsonObject)ja [i];
						var jObj = Activator.CreateInstance (itemClass);
						((BasePacket)jObj).antiSerialization (jo);
						il.SetValue (jObj, i);
					}
					field.SetValue (this, il);
				}
			}
		}
	}
}
