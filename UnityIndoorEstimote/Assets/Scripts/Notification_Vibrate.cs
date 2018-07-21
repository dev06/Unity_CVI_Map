using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Vibrate : Notification {

    //vibes to represent the different rotations
    public Vibe[] vibes;

    // Use this for initialization
    public override void Start()
    {

    }

    public override float Notify(float distance, float rotation)
    {
        //stop any previous vibrations
        StopAllCoroutines();

        //player facing straight
        if (Mathf.Abs(rotation) < 10) {
            Vibrate_Watch.VIBRATE_LEFT(3, 128, 256);
            Vibrate_Watch.VIBRATE_RIGHT(3, 128, 256);
            StartCoroutine("Vibrate", vibes[0]);
        }
        else
        {
            if(rotation < 0)
            {
                rotation = -rotation;
                if (rotation < 45)
                {
                    Vibrate_Watch.VIBRATE_RIGHT(3, 128, 256);
                }
                else if (rotation < 90)
                {
                    Vibrate_Watch.VIBRATE_RIGHT(2, 128, 256);
                }
                else
                {
                    Vibrate_Watch.VIBRATE_RIGHT(1, 128, 256);
                }
            }
            else
            {
                if (rotation < 45)
                {
                    Vibrate_Watch.VIBRATE_LEFT(3, 128, 256);
                }
                else if(rotation < 90)
                {
                    Vibrate_Watch.VIBRATE_LEFT(2, 128, 256);
                }
                else
                {
                    Vibrate_Watch.VIBRATE_LEFT(1, 128, 256);
                }
            }
        }


        //return how long to wait until next notification
        return 3;
    }

    /// <summary>
    /// Vibrate the phone according to the vibe
    /// </summary>
    /// <param name="vibe">
    /// Struct holding the vibration pattern
    /// </param>
    /// <returns></returns>
    IEnumerator Vibrate(Vibe vibe)
    {
        yield return null;
        for (int i = 0; i < vibe.numTimes; i++)
        {
            Vibration.Vibrate(10);
            yield return new WaitForSecondsRealtime(vibe.delay);
        }
    }
}

/// <summary>
/// used to hold the vibration pattern ("Vibe")
/// </summary>
[System.Serializable]
public struct Vibe
{
    //how long to delay between vibrations
    public float delay;
    public int numTimes;
    public Vibe(float _delay, int _times)
    {
        delay = _delay;
        numTimes = _times;
    }
}