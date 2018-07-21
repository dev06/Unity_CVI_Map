using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_ClickToMove : User {
    public float speed = .2f;       //how fast the player should move towards the target
    public float targetDistance = .5f;  //how far the target should move when it is reached
    public GameObject targetObj;    //the red target visual to move when target moves
    public Transform waypointObj;

    Waypoint curWaypoint;           //store the current waypoint moving towards
    Vector3 targetPos;              //store the current target position the player is walking to
    bool moveWaypoint = true;       //store if the waypoint should be moved next frame
	// Use this for initialization
	void Start () {
        targetObj.transform.position = path[0].transform.position;
        targetPos = targetObj.transform.position;
        curwaypointindex = -1;
        NextTarget();
        transform.position = path[0].transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        user_position.x = transform.position.x;
        user_position.y = transform.position.z;

        //get the local rotation (in terms of the room) and set the rotation of the character
        user_localRotation = UserRotation.GetRotation();
        transform.rotation = Quaternion.Euler(0, user_localRotation, 0);
        
        //store where the user is facing
        user_forward = transform.forward;

        //if user presses button, the user reached the current target
        /*if (Input.GetKeyDown(KeyCode.Space))
            NextTarget();*/

        if (Input.GetButtonDown("Fire2"))
            NextTarget();
	}

    public void ChangeTargetDistance(float newDist) { targetDistance = newDist; }

    public void ChangeMoveSpeed(float newSpeed) { speed = newSpeed; }

    public void HitButton() { NextTarget(); }

    /// <summary>
    /// Move the target towards the next waypoint
    /// </summary>
    public void NextTarget()
    {
        //changing to the next waypoint
        if (moveWaypoint)
        {
            curwaypointindex++;
            if (curwaypointindex >= path.Length)
            {
                Debug.Log("Out of waypoints...");
                return;
            }
            //hide the current waypoint
            if(curWaypoint)
                curWaypoint.Hide();
            //increment to next waypoint
            curWaypoint = path[curwaypointindex];
            //show the new waypoint
            curWaypoint.Show();
            waypointObj.position = curWaypoint.transform.position;
            //dont need to move next time we move target
            moveWaypoint = false;
        }
        //move the target towards the current waypoint
        StopAllCoroutines();
        Vector3 offset = curWaypoint.transform.position - targetPos;
        //if we are close enough to waypoint, target is waypoint and we increment waypoint next time
        if (offset.sqrMagnitude < targetDistance * targetDistance)
        {
            moveWaypoint = true;
            targetPos = curWaypoint.transform.position;
        }
        //otherwise, move target towards the waypoint a specified amount
        else
        {
            offset.Normalize();
            targetPos = targetPos + (offset * targetDistance);
        }
        targetObj.transform.position = targetPos;
        StartCoroutine("MoveToTarget");
    }

    float cur;
    Vector3 start, end;
    /// <summary>
    /// gradually move toward the target, if we want that
    /// </summary>
    IEnumerator MoveToTarget()
    {
        //if speed 0, jump to end
        if (speed != 0)
        {
            cur = 0;
            start = transform.position;
            while (cur < 1)
            {
                transform.position = Vector3.Lerp(start, targetPos, cur);
                cur += speed * Time.deltaTime;

                yield return null;
            }
        }
        transform.position = targetPos;
    }
}
