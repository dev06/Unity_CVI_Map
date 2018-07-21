using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback_Ping : FeedbackType {


	private UserMove user; 

	private UserController userController; 

	public AudioSource source; 
	
	public AudioReverbZone reverb; 

	private bool startTimer; 

	private float timer; 

	private float play_delay = 1f; 

	private Instruction instruction; 

	private float defVolume; 

	private string activeSceneName; 



	void Start () 
	{
		timer = play_delay; 

		user = FindObjectOfType<UserMove>(); 

		userController = FindObjectOfType<UserController>(); 

		instruction = FindObjectOfType<Instruction>(); 

		defVolume = source.volume; 

		activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame

	float storedDistance; 
	float prevDistance; 
	
	void Update () {
		
		selected = toggle.isOn;

		source.volume = PauseHandler.PAUSE ? 0F : defVolume; 


		if(prevDistance != storedDistance || (prevDistance == 0 && storedDistance == 0))
		{
			storedDistance = activeSceneName == "TrainingScene" ? user.GetDistanceToTarget() : userController.GetDistanceToTarget(); 
			
			prevDistance = storedDistance; 
		}


		if(selected == false)
		{
			Stop(); 
		} 

		if(startTimer)
		{
			timer+=Time.deltaTime; 
		}

		float delay = 0; 

		if(activeSceneName == "TrainingScene")
		{
			delay = user.GetDistanceToTarget(); 

			delay = user.GetDistanceToTarget() / storedDistance;  

			delay = Mathf.Clamp(delay,.4f, 1f); 
		}
		else if( activeSceneName == "Experiment")
		{
			delay = userController.GetDistanceToTarget(); 

			delay = userController.GetDistanceToTarget() / storedDistance;  

			delay = Mathf.Clamp(delay, .4f, 1f); 
		}

		if(timer > delay)
		{
			PlaySource(); 
			timer = 0; 
		}

	}

	public void Play()
	{
		timer = play_delay; 
		startTimer = true; 
	}

	private void PlaySource()
	{	
		if(user != null)
		{
			if(user.TutorialOver)
			{
				Stop(); 
				return; 
			}
		}
		// else if(userController != null)
		// {
		// 	Stop()
		// 	return; 
		// }




		if(instruction != null)
		{
			if(instruction.isPlaying) return; 			
		}
		
		if(!selected)
		{
			Stop();  
		}
		source.Play(); 
		//reverb.enabled = true; 
	}



	public void Stop()
	{
		//reverb.enabled = false; 
		source.Stop(); 
		startTimer = false; 
		timer = 0;
	}
}
