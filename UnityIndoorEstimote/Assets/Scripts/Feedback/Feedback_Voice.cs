using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback_Voice : FeedbackType {


	public AudioClip[] clips;

	public AudioSource source;

	private bool startTimer;

	private float timer;

	private int clipID;

	private float play_delay = 2f;

	private float defVolume;

	void Start ()
	{
		source.loop = false;

		timer = play_delay;

		defVolume = source.volume;
	}

	void Update () {

		selected = toggle.isOn;

		source.volume = PauseHandler.PAUSE ? 0F : defVolume;


		if (selected == false)
		{
			Stop();
		}

		if (startTimer)
		{
			timer += Time.deltaTime;
		}

		if (timer > play_delay)
		{
			PlaySource();
			timer = 0;
		}
	}

	public void Play(int id)
	{
		startTimer = true;

		this.clipID = id;
	}

	private void PlaySource()
	{
		if (!selected)
		{
			Stop();
		}
		source.clip = clips[clipID];

		source.Play();

	}



	public void Stop()
	{
		source.Stop();
		startTimer = false;
		timer = 0;
	}
}
