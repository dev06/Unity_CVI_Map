using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    //use these to vibrate watch to tell user how close they are
    float distanceOffset;       //change delay between Vibes
    float rotationOffset;       //change Vibe

    public Transform target;
    public Transform user;

    [Header("Notification Options")]
    public Notification notification_vibrate;
    public Notification notification_ping;
    public Notification notification_voice;
    // Use this for initialization
    void Start () {
        //start coroutines to send notifications at intervals
        StartCoroutine("Check_Notification", notification_vibrate);
        StartCoroutine("Check_Notification", notification_ping);
        StartCoroutine("Check_Notification", notification_voice);
        Debug.Log("started");
    }

    Vector3 temp;
    // Called once a frame
    private void Update()
    {
        //get distance from player and their rotation compared to where it should be
        temp = target.position - user.position;
        distanceOffset = temp.magnitude;
        rotationOffset = Vector3.SignedAngle(temp, user.forward, Vector3.up);
    }

    // Called when object destroyed
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //check the player and which notificaiton options are active and send a message
    IEnumerator Check_Notification(Notification notif)
    {
        while (true)
        {
            //if the player is in the room && notification is toggled on
            while (notif.will_notify.isOn)
            {
                yield return new WaitForSecondsRealtime(notif.Notify(distanceOffset, rotationOffset));
            }

            //wait until next frame
            yield return null;
        }
    }
}
