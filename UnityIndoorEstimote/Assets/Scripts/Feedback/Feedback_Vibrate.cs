using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback_Vibrate : FeedbackType {

	Vibrate_Watch watch; 

	private bool leftInit, rightInit; 

	private bool left, right; 

	private float timer; 

	void Start () 
	{
		watch = FindObjectOfType<Vibrate_Watch>(); 

		Init(); 
	}
	
	
	void Update () 
	{
		selected = toggle.isOn;

		if(left || right)
		{
			timer+=Time.deltaTime; 
		}

		if(!selected)
		{	
			if(leftInit && rightInit)
			{
				// watch.TestVibrateLeft(0); 
				// watch.TestVibrateRight(0); 
			}
		}

		if(timer > 1f)
		{
			if(left)
			{
				watch.TestVibrateLeft(); 
				timer = 0; 
			}

			if(right)
			{
				watch.TestVibrateRight(); 
				timer = 0; 
			}
		}
	}

	public void Init()
	{
		watch.TryInitLeft(); 
		watch.TryInitRight(); 
		leftInit = true; 
		rightInit = true; 
	}

	public void VibrateLeft()
	{
		if(leftInit == false)
		{
			watch.TryInitLeft(); 
			leftInit = true; 
		}

		if(PauseHandler.PAUSE) return; 

		left = true; 
		right = false; 
		timer = 0; 
		watch.TestVibrateLeft(1); 
	}

	public void VibrateRight()
	{
		if(rightInit == false)
		{
			watch.TryInitRight(); 
			rightInit = true; 
		}

		if(PauseHandler.PAUSE) return; 

		right = true; 
		left = false; 
		timer = 0; 
		watch.TestVibrateRight(1); 
	}
}
