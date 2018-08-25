using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataSaver : MonoBehaviour {

	private static FileInfo f;

	private int csv_id = 0;

	private static string fileName = (System.DateTime.Now).ToString(); 

	public static void Save(List<string> lines)
	{
		string s = fileName; 

		s = s.Replace("/", "_"); 

		f = new FileInfo(Application.persistentDataPath + "\\" + "study-" + s + ".txt");

		StreamWriter w = null;

		w = f.CreateText();

		for(int i = 0;i < lines.Count; i++)
		{
			w.WriteLine(lines[i]); 
		}

		Debug.Log("Data Saved"); 

		w.Close();
	}
}
