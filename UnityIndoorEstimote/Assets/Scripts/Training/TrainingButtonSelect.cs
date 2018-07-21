using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingButtonSelect : MonoBehaviour {

	private bool active; 
	public Canvas trainingCanvas; 
	public Feedback_Voice voice; 
	public Feedback_Ping ping; 
	public Feedback_Vibrate vibrate; 

	private Instruction instruction; 

	void Start()
	{
		instruction = FindObjectOfType<Instruction>(); 
	}

	void Update()
	{
		#if !UNITY_EDITOR

		if(Input.GetButtonDown("NextScene"))
		{
			
			UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
			
		}

		if(active) 
		{
				if(Input.GetButtonDown("Reload")) // B
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
				}
				return;
			} 

		if(Input.GetButtonDown("Fire2")) // B
		{
			if(trainingCanvas.enabled)
			{
				OnVoiceSelect(); 
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
			}
		}

		if(Input.GetButtonDown("Fire1")) // A
		{
			if(trainingCanvas.enabled)
			{
				OnPingSelect(); 
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
			}
		}

		if(Input.GetButtonDown("Fire3")) // A
		{
			if(trainingCanvas.enabled)
			{
				OnVibrationSelect(); 
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
			}
		}		

		#endif
	}


	public void OnVoiceSelect()
	{
		voice.toggle.isOn = true; 
		trainingCanvas.enabled = false; 
		active = true; 
		instruction.PlayInstruction(InstructionType.Voice); 
	}

	public void OnPingSelect()
	{
		ping.toggle.isOn = true; 
		trainingCanvas.enabled = false; 
		active = true; 
		instruction.PlayInstruction(InstructionType.Ping); 
	}

	public void OnVibrationSelect()
	{
		vibrate.toggle.isOn = true; 
		trainingCanvas.enabled = false; 
		active = true; 
		// play vibration tutorial here!
		instruction.PlayInstruction(InstructionType.Haptic); 
	}

	public void OnExperimentSelect()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
	}
}
