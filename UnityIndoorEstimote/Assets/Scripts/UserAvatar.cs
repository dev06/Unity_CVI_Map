using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserAvatar : User
{
    public Text testing;
    [Header("Auto Move")]
    public bool auto_move;              //if the user should dmeo the environment and walk automatically
    public Slider auto_speed;           //speed to walk when demoing
    public Slider auto_rotation;

    [Header("Lock Movement")]
    public bool lock_move;


    // Use this for initialization
    void Start()
    {
        user_position = new Vector3();
        //if(SystemInfo.deviceType == DeviceType.Handheld)
        //{
        //    auto_move = false;
        //}

        StartCoroutine("UpdatePositionAndRotation");
    }

    Vector3 nextposition;
    IEnumerator UpdatePositionAndRotation()
    {
        while (true)
        {
            //auto move the player through the environment (used for testing)
            if (auto_move)
            {
                nextposition = path[curwaypointindex].transform.position;
                //move to the next target
                while ((transform.position - nextposition).sqrMagnitude > .1f)
                {
                    //rotate towards the next target

                    //float angle = Vector3.SignedAngle((nextposition - transform.position).normalized, transform.forward, Vector3.up);
                    //if ( angle > 1f)
                    //{
                    //    FindObjectOfType<Notification_Voice>().Speak(VoiceDirection.Left);
                    //}
                    //else if (angle < -1f)
                    //{
                    //    FindObjectOfType<Notification_Voice>().Speak(VoiceDirection.Right);
                    //}


                    while (Vector3.SignedAngle((nextposition - transform.position).normalized, transform.forward, Vector3.up) > 1)
                    {
                        transform.Rotate(0, -auto_rotation.value * Time.deltaTime * 20, 0);
                        SetMyValues();
                        yield return null;
                    }
                    while (Vector3.SignedAngle((nextposition - transform.position).normalized, transform.forward, Vector3.up) < -1)
                    {
                        transform.Rotate(0, auto_rotation.value * Time.deltaTime * 20, 0);
                        SetMyValues();
                        yield return null;
                    }
                    transform.LookAt(nextposition);

                    transform.Translate((nextposition - transform.position).normalized * auto_speed.value * Time.deltaTime, Space.World);
                    SetMyValues();
                    yield return null;
                }
                transform.position = nextposition;
            }
            else if (lock_move)
            {
                //get position from IndoorManager
                user_position.x = (float)IndoorManager.x;
                user_position.z = (float)IndoorManager.y;

                if (curwaypointindex >= 1)
                    transform.position = Vector3.Lerp(transform.position, NearestPointOnLine(path[curwaypointindex - 1].transform.position, path[curwaypointindex].transform.position, user_position), .02f);
                else
                    transform.position = Vector3.Lerp(transform.position, user_position, .02f);

                //get the local rotation (in terms of the room) and set the rotation of character
                user_localRotation = UserRotation.GetRotation();
                testing.text = string.Format("Rot: {0:0.00}", user_localRotation);
                transform.rotation = Quaternion.Euler(0, user_localRotation, 0);

                //store where the user is facing
                user_forward = transform.forward;
            }
            //use estimote and phone rotation to move (used for deployment)
            else
            {
                //get position from IndoorManager
                user_position.x = (float) IndoorManager.x;
                user_position.z = (float) IndoorManager.y;
                transform.position = Vector3.Lerp(transform.position, user_position, .02f);

                //get the local rotation (in terms of the room) and set the rotation of character
                user_localRotation = UserRotation.GetRotation();
                testing.text = string.Format("Rot: {0:0.00}", user_localRotation);
                transform.rotation = Quaternion.Euler(0, user_localRotation, 0);

                //store where the user is facing
                user_forward = transform.forward;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Find the point on the line from start to end that is closest to given pnt
    /// </summary>
    /// <param name="start">The start waypoint</param>
    /// <param name="end">The end waypoint</param>
    /// <param name="pnt">The point we are comparing</param>
    /// <returns></returns>
    Vector3 NearestPointOnLine(Vector3 start, Vector3 end, Vector3 pnt)
    {
        Vector3 lineDir = end - start;
        lineDir.Normalize(); //this needs to be a unit vector
        Vector3 v = pnt - start;
        float d = Vector3.Dot(v, lineDir);
        return start + lineDir * d;
    }

    /// <summary>
    /// -Get the next waypoint from the user
    /// -Increase the curwapointindex
    /// </summary>
    /// <returns>The next waypoint in path</returns>
    public Waypoint GetNextWaypoint()
    {

        //increase index to move to next
        if(curwaypointindex < path.Length - 1)
            curwaypointindex++;

        return path[curwaypointindex];
    }

    public void SetEndWaypoint(Waypoint w)
    {
        Waypoint[] newpath = WaypointManager.FindPathToWaypoint(path[curwaypointindex], w, FindObjectOfType<WaypointManager>().visited.ToArray());
        if(newpath == null)
        {
            Debug.Log("Path not valid, continuing on current path");
            return;
        }
        Debug.Log("Changing path");
        path = newpath;
        curwaypointindex = 0;
    }

    void SetMyValues()
    {
        user_position.x = transform.position.x;
        user_position.z = transform.position.z;
        user_localRotation = transform.rotation.eulerAngles.y;
        user_forward = transform.forward;
    }

    public void SetAutoWalk(bool walkauto)
    {
        auto_move = walkauto;
    }
}
