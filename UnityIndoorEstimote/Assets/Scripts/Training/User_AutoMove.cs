using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class User_AutoMove : MonoBehaviour 
{

    public float speed = .2f;       //how fast the player should move towards the target
    public float targetDistance = .5f;  //how far the target should move when it is reached
    public GameObject targetObj;    //the red target visual to move when target moves
    public Waypoint[] path;
    private int curwaypointindex = 0;
    public float user_localRotation;
    public Slider slider; 
    private Feedback_Voice voice; 
    private Feedback_Vibrate vibrate; 
    private bool tutorialOver; 
    private Instruction instruction; 
    Waypoint curWaypoint;           //store the current waypoint moving towards
    Vector3 targetPos;              //store the current target position the player is walking to
    bool moveWaypoint = true;       //store if the waypoint should be moved next frame
    // Use this for initialization  
    public bool TutorialOver
    {
      get{return tutorialOver; }
    }
    void Start () 
    {
      curwaypointindex = 1;
      curWaypoint = path[curwaypointindex]; 
      curWaypoint.Show(); 
      instruction = FindObjectOfType<Instruction>(); 
      transform.position = path[0].transform.position;
      targetObj.transform.position = path[1].transform.position; 
      targetPos = targetObj.transform.position; 
      voice = FindObjectOfType<Feedback_Voice>(); 
      vibrate = FindObjectOfType<Feedback_Vibrate>(); 


    // FindObjectOfType<UserRotation>().SetNorth(); 
    }

    bool chooseNext; 
    float keyRot; 
    bool lockRotation; 
    bool lookingAtNextCheckpoint; 
    Quaternion storedRotation; 

    void Update () 
    {
      //  user_position.x = transform.position.x;
       // user_position.y = transform.position.z;

      if(PauseHandler.PAUSE)
      {
        return; 
      }

      if(Input.GetButtonDown("North"))
      {
        FindObjectOfType<UserRotation>().SetNorth(); 
      }


      if(instruction.isPlaying)
      {

        transform.position = path[0].transform.position;
        return; 
      }


      if(!lookingAtNextCheckpoint)
      {

        if(Input.GetKey(KeyCode.D))
        {
          keyRot+=10f;    
        }
        else if(Input.GetKey(KeyCode.A))
        {
         keyRot-=10f; 
       }
       user_localRotation = UserRotation.GetRotation();
     }

     if(!instruction.isPlaying)
     {
    #if UNITY_EDITOR
      transform.rotation = Quaternion.Euler(0, keyRot, 0); 
#else
      transform.rotation = Quaternion.Euler(0, user_localRotation, 0);        
#endif         
    }

    Vector3 distance = transform.position - curWaypoint.transform.position; 

    if(lookingAtNextCheckpoint && lockRotation)
    {
      transform.position = Vector3.MoveTowards(transform.position, curWaypoint.transform.position, Time.deltaTime * slider.value * speed);            
    }

    if(distance.sqrMagnitude == 0f)
    {

      lockRotation = false; 
      curWaypoint.Hide(); 
      curwaypointindex++; 
      if(curwaypointindex > path.Length - 1)
      {
       Debug.Log("Out of waypoint"); 
       instruction.PlayInstruction(InstructionType.Complete);
       tutorialOver = true; 
       voice.Stop(); 
       curwaypointindex = 0; 

       StopCoroutine("IReload"); 
       StartCoroutine("IReload"); 
     }
     curWaypoint = path[curwaypointindex]; 
     targetObj.transform.position = path[curwaypointindex].transform.position;  
     if(lookingAtNextCheckpoint)
     {    
       distance =  transform.position - curWaypoint.transform.position; 
     }
   }

   lookingAtNextCheckpoint = Vector3.Angle(transform.position - curWaypoint.transform.position, -transform.forward) < 10; 

   if(lockRotation == false)
   {   
    if(lookingAtNextCheckpoint)
    {
     lockRotation = true; 
   }
 }

 curWaypoint.Show(); 

 if(lockRotation)
 {
 // transform.LookAt(curWaypoint.transform.position); 
 }

 PlayVoice(); 

 VibrateWatches(); 
}

public float GetDistanceToTarget()
{
 float distance = Vector3.Distance(transform.position, targetObj.transform.position); 

 return distance; 
}

private void PlayVoice()
{
 if(tutorialOver) 
 {
  voice.Stop(); 
  return; 
}  
if(instruction.isPlaying) return; 

if(voice.selected == false) return;        
if(!lockRotation)
{
            // - right
            // + left
  float angle = GetAngle(); 

  if(angle >= 0)
  {
   voice.Play(0); 
 }
 else 
 {
   voice.Play(1); 
 }
}
else
{
  voice.Play(2); 
}
}

private void VibrateWatches()
{
 if(tutorialOver)
 {
  return; 
}

if(instruction.isPlaying) return; 

if(vibrate.selected == false) return; 

float angle = GetAngle(); 

//      Debug.Log(angle); 

if(!lockRotation)
{
  if(angle >=0)
  {
   vibrate.VibrateLeft(); 
 }
 else
 {
   vibrate.VibrateRight(); 
 }            
}
}

public float GetAngle()
{
 return Vector3.SignedAngle(transform.position -curWaypoint.transform.position, -transform.forward, Vector3.up); 
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
                //transform.position = Vector3.Lerp(start, targetPos, cur);
   cur += speed * Time.deltaTime;

   yield return null;
 }
}
       // transform.position = targetPos;
}

IEnumerator IReload()
{

 while(instruction.isPlaying)
 {
  yield return null; 
}

yield return new WaitForSeconds(.5f); 

UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
}
public void SetNorth()
{
  FindObjectOfType<UserRotation>().SetNorth(); 
}
}


