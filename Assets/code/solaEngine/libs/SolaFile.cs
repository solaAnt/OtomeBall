using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class SolaFile
{
	public static void createFile (string path, string info)
	{
		StreamWriter sw;
		FileInfo t = new FileInfo (path);
		if (!t.Exists) 
			sw = t.CreateText ();
		else 
			sw = t.AppendText ();
		
		sw.WriteLine (info);
		sw.Close ();
		sw.Dispose ();
	}

	public static void DeleteFile (string path)
	{
		File.Delete (path);
	}

	public static ArrayList LoadFile (string path)
	{
		StreamReader sr = null;
		try {
			sr = File.OpenText (path);
			Debug.Log ("Read data " + path);
		} catch (Exception e) {
			Debug.Log (e);
			return null;
		}
		
		string line;
		ArrayList arrlist = new ArrayList ();
		while ((line = sr.ReadLine()) != null) 
			arrlist.Add (line);
		
		sr.Close ();
		sr.Dispose ();
		
		return arrlist;
	}  

	/**
	* path：文件创建目录
	* name：文件的名称
	* info：写入的内容
	*/
	public static void CreateAppFile (string name, string info)
	{
		createFile (Application.persistentDataPath + Path.DirectorySeparatorChar + name, info);
	}

	/**
	* path：删除文件的路径
	* name：删除文件的名称
	*/ 
	public static void DeleteAppFile (string name)
	{
		File.Delete (Application.persistentDataPath + Path.DirectorySeparatorChar + name);
	}

	/**
	* path：读取文件的路径
	* name：读取文件的名称
	*/
	public static ArrayList LoadAppFile (string name)
	{
		return LoadFile (Application.persistentDataPath + Path.DirectorySeparatorChar + name);
	}  

}
