using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TrainingButtonSelect : MonoBehaviour {

	private bool active;
	public Canvas trainingCanvas;
	public Feedback_Voice voice;
	public Feedback_Ping ping;
	public Feedback_Vibrate vibrate;
	public Toggle isTrainingToggle;
	public Toggle standing, walking;
	private Instruction instruction;

	private UserMovement userMovement;

	void Start()
	{
		instruction = FindObjectOfType<Instruction>();
		isTrainingToggle.isOn = AtriumController.Instance.isTraining;
		userMovement = FindObjectOfType<UserMovement>();
	}

	void Update()
	{
#if !UNITY_EDITOR

		if (active)
		{
			if (Input.GetButtonDown("Reload")) // B
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
			return;
		}

		if (Input.GetButtonDown("Fire2")) // B
		{
			if (trainingCanvas.enabled)
			{
				OnVoiceSelect();
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		if (Input.GetButtonDown("Fire1")) // A
		{
			if (trainingCanvas.enabled)
			{
				OnPingSelect();
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		if (Input.GetButtonDown("Fire3")) // A
		{
			if (trainingCanvas.enabled)
			{
				isTrainingToggle.isOn = !isTrainingToggle.isOn;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		if (Input.GetButtonDown("SwitchMode"))
		{
			if (standing.isOn)
			{
				walking.isOn = true;
			}
			else if (walking.isOn)
			{
				standing.isOn = true;
			}
		}

		if (Input.GetButtonDown("ToggleTraining"))
		{
			isTrainingToggle.isOn = !isTrainingToggle.isOn;
		}

#endif
		userMovement.mode = standing.isOn ? SessionMode.Standing : SessionMode.Walking;

		AtriumController.Instance.isTraining = isTrainingToggle.isOn;
	}

	public void OnTrainingValueChanged()
	{

	}


	public void OnVoiceSelect()
	{
		voice.toggle.isOn = true;
		trainingCanvas.enabled = false;
		active = true;
		if (AtriumController.Instance.isTraining)
		{
			instruction.PlayInstruction(InstructionType.Voice);
		}
	}

	public void OnPingSelect()
	{
		ping.toggle.isOn = true;
		trainingCanvas.enabled = false;
		active = true;
		if (AtriumController.Instance.isTraining)
		{
			instruction.PlayInstruction(InstructionType.Ping);
		}
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
