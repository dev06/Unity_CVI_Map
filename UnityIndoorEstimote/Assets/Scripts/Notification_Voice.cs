using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Voice : Notification {

    public AudioSource voice;

    public AudioClip left, right;
    // Use this for initialization
    public override void Start()
    {

    }

    public override float Notify(float distance, float rotation)
    {
        Debug.Log("VOICING");

        if (rotation > 1f)
        {
            Speak(VoiceDirection.Left);
        }
        else if (rotation < -1f)
        {
            Speak(VoiceDirection.Right);
        }
        return 3;
        //DO ANYTHING USING AMOUNT TO NOTIFY

        //return how long to wait until next notification
        return 1;
    }

    public void Speak(VoiceDirection direction)
    {
        if (!will_notify.isOn)
        {
            voice.Stop();
            return;
        }

        if (voice.isPlaying == false)
        {
            AudioClip clipToPlay = direction == VoiceDirection.Left ? left : right;

            voice.clip = clipToPlay;

            voice.Play();
        }
    }
}

public enum VoiceDirection
{
    Left,
    Right,
}


