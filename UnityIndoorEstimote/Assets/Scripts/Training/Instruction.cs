using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum InstructionType
{
	None,
	Ping,
	Voice,
	Complete,
	Haptic
}
public class Instruction : MonoBehaviour {


	public Text isInstructionPlayingText;

	public AudioClip[] clips;

	private AudioSource source;

	public bool isPlaying;

	void Start ()
	{
		source = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (PauseHandler.PAUSE)
		{
			source.Pause();
		}

		isInstructionPlayingText.text = "Instruction Playing: " + source.isPlaying;
	}

	public void PlayInstruction(InstructionType type)
	{
		source.Stop();

		switch (type)
		{
			case InstructionType.Ping:
			{
				source.clip = clips[0];
				source.Play();
				break;
			}

			case InstructionType.Voice:
			{
				source.clip = clips[1];
				source.Play();
				break;
			}
			case InstructionType.Complete:
			{
				source.clip = clips[2];
				source.Play();
				break;
			}
			case InstructionType.Haptic:
			{
				source.clip = clips[3];
				source.Play();
				break;
			}
		}

		StopCoroutine("IIsPlaying");
		StartCoroutine("IIsPlaying");
	}

	IEnumerator IIsPlaying()
	{
		while (source.isPlaying)
		{
			isPlaying = true;
			yield return null;
		}

		isPlaying = false;

	}
}