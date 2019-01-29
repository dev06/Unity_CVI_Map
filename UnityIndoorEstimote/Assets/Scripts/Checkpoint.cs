using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Used to tell the user where the need to go
/// The transform position is where the current checkpoint is
/// When the user gets close to the checkpoint, it is moved in WaypointManager
/// </summary>
public class Checkpoint : MonoBehaviour {

    //use these to vibrate watch to tell user how close they are
    float distanceOffset;       //change delay between Vibes
    float rotationOffset;       //change Vibe

    public float radius;        //how close the player needs to be to go to the next checkpoint
    float radiusSquared;

    public float comfortable_distance;  //how far the checkpoint will move towards the next
    UserAvatar userAvatar;
    Waypoint nextWaypoint;

    [Header("Notification Options")]
    public Notification notification_vibrate;
    public Notification notification_ping;
    public Notification notification_voice;

    // Use this for initialization
    void Start () {
        radiusSquared = radius * radius;
        userAvatar = FindObjectOfType<UserAvatar>();
        nextWaypoint = userAvatar.path[0];
        transform.position = nextWaypoint.transform.position;
        //start coroutines to send notifications at intervals
        StartCoroutine("Check_Notification", notification_vibrate);
        StartCoroutine("Check_Notification", notification_ping);
        StartCoroutine("Check_Notification", notification_voice);
    }


    Vector3 temp;
    // Called once a frame
    private void Update()
    {
        //get distance from player and their rotation compared to where it should be
        temp = transform.position - User.user_position;
        distanceOffset = temp.magnitude;
        rotationOffset = Vector3.SignedAngle(temp, User.user_forward, Vector3.up);

        //if close to the checkpoint, move the checkpoint
        if (temp.sqrMagnitude < radiusSquared)
        {
            MoveToNextPoint();
        }
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

    Vector3 checkpointToWaypointDisplacement;
    //move the checkpoint towards the next point taking into account the comfortable distance
    public void MoveToNextPoint()
    {
        //displacement between waypoint and current position
        checkpointToWaypointDisplacement = nextWaypoint.transform.position - transform.position;

        //if the user is right on top of the current waypoint
        if (checkpointToWaypointDisplacement.sqrMagnitude < .1f)
        {
            nextWaypoint.Visit();
            nextWaypoint.Hide();    //hide old waypoint
            nextWaypoint = userAvatar.GetNextWaypoint();    //get next waypoint from user
            nextWaypoint.Show();    //show new waypoint

            //update the distance of checkpoint -> waypoint
            checkpointToWaypointDisplacement = nextWaypoint.transform.position - transform.position;
        }

        //if the user is close to next waypoint (less than next comfortable_distance)
        if (checkpointToWaypointDisplacement.magnitude < comfortable_distance)
        {
            //move the checkpoint to the next waypoint
            transform.position = nextWaypoint.transform.position;
        }
        //if the user is far from the next waypoint (more than comfortable_distance)
        else
        {
            //move the checkpoint in the direction of the next waypoint

            //get the normalized vector towards next waypoint
            Vector3 temp = nextWaypoint.transform.position - transform.position;
            temp.y = 0;
            temp.Normalize();

            //scale by checkPointDistance
            temp *= comfortable_distance;
            transform.position = transform.position + temp;
        }
    }
}