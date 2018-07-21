using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class PauseHandler : MonoBehaviour {

	public static bool PAUSE = false; 
	private Canvas canvas; 
	void Start () 
	{
		canvas = GetComponent<Canvas>(); 
		PAUSE = false; 
		canvas.enabled = PAUSE;

	}	
	
	
	void Update () 
	{
		canvas.enabled = PAUSE; 

#if !UNITY_EDITOR
		if(Input.GetButtonDown("Fire4"))
		{
			PAUSE = !PAUSE; 
		}	
#endif

		if(Input.GetKeyDown(KeyCode.I))
		{
			PAUSE = !PAUSE; 
		}
	}
}
