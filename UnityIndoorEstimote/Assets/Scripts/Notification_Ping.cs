using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_Ping : Notification {

    // Use this for initialization
    public AudioSource ping;
    public AudioClip pingsound;
    public override void Start () {

    }

    public override float Notify(float distance, float rotation)
    {
        Debug.Log("PINGING");
        Ping();
        return 5;
        //DO ANYTHING USING AMOUNT TO NOTIFY
        //return how long to wait until next notification
        return 1;
    }

    public void Ping()
    {
        Debug.Log("Pinged");
        if (!will_notify.isOn) {
            ping.Stop();
            return;
        }

        ping.PlayOneShot(pingsound);
    }
}
