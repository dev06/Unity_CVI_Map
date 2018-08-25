using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class UserMove : MonoBehaviour 
{


 	public float speed = .2f;       //how fast the player should move towards the target

    public GameObject targetObj;    //the red target visual to move when target moves

    public Slider rotationOffsetSlider; 

    public Text isLookingAtTarget; 
    
    public Waypoint[] path;
    
    private int curwaypointindex = 0;
    
    public float user_localRotation;
    
    public Slider slider; 
    
    private Feedback_Voice voice; 
    
    private Feedback_Vibrate vibrate; 
    
    private bool tutorialOver; 
    
    private Instruction instruction; 
    
    private Waypoint curWaypoint;           
    
    private Vector3 targetPos;              
    
    private bool moveWaypoint = true;       
    
    private float keyRot; 
    
    private bool lockRotation; 
    
    private bool lookingAtNextCheckpoint;  

    private TrainingButtonSelect trainingButtonSelect;   

    private float lookingTimer; 

    private bool startMoving; 

    private float movementDelay = 1.5f; 

    private float rotationOffsetValue; 

    private bool instructionStopped; 

    public bool TutorialOver
    {
    	get{return tutorialOver; }
    }

    void Start () 
    {
    	
    	curwaypointindex = 1;
    	
    	curWaypoint = path[curwaypointindex]; 

        previousCheckpoint = path[curwaypointindex - 1]; 

        curWaypoint.Show(); 

        instruction = FindObjectOfType<Instruction>(); 

        transform.position = path[0].transform.position;

        targetObj.transform.position = path[1].transform.position; 

        targetPos = targetObj.transform.position; 

        voice = FindObjectOfType<Feedback_Voice>(); 

        vibrate = FindObjectOfType<Feedback_Vibrate>(); 

        trainingButtonSelect = FindObjectOfType<TrainingButtonSelect>(); 
    }



    void Update () 
    {
      //  user_position.x = transform.position.x;
       // user_position.y = transform.position.z;
    	if(trainingButtonSelect.trainingCanvas.enabled) return; 

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

    	if(!instructionStopped)
    	{
    		StopCoroutine("IUpdateNorthOnDelay"); 
    		StartCoroutine("IUpdateNorthOnDelay"); 
    		instructionStopped = true; 
    	}

    	UpdatePCControls();

    	UpdateTargetCheckpoints(); 

    	lookingAtNextCheckpoint = Vector3.Angle(transform.position - curWaypoint.transform.position, -transform.forward) < 15; 

        //not used
        if(lookingAtNextCheckpoint)
        {
            if(!lookingEvent)
            {
                SnapRotation(); 
                lookingEvent = true; 
            }
        }
        else
        {
            lookingEvent = false; 
        }
        //not used

        isLookingAtTarget.text = "Looking At Target: " + lookingAtNextCheckpoint;

        UpdateLookTimer(); 

        curWaypoint.Show(); 

        PlayVoice(); 

        VibrateWatches();

        UpdateKeys(); 
    }

    private bool lookingEvent;
    private Waypoint previousCheckpoint;  

    public void SnapRotation()
    {

    }

    IEnumerator IUpdateNorthOnDelay()
    {
    	yield return new WaitForSeconds(.15f); 
    	FindObjectOfType<UserRotation>().SetNorth(); 
    }

    private void UpdatePCControls()
    {
    	if(Input.GetKey(KeyCode.D))
    	{
    		keyRot+=1f; 
    	}
    	else if(Input.GetKey(KeyCode.A))
    	{
    		keyRot-=1f; 
    	}

    	user_localRotation = UserRotation.GetRotation(); 

    	if(!instruction.isPlaying)
    	{
            #if UNITY_EDITOR
    		transform.rotation = Quaternion.Euler(0, keyRot, 0); 
            #else
    		transform.rotation = Quaternion.Euler(0, user_localRotation, 0); 
            #endif
    	}
    }

    private void UpdateTargetCheckpoints()
    {
    	Vector3 distance = transform.position - curWaypoint.transform.position; 

    	if(startMoving)
    	{
    		transform.position = Vector3.MoveTowards(transform.position, curWaypoint.transform.position, Time.deltaTime * slider.value * speed);            
    	}

    	if(distance.sqrMagnitude == 0f)
    	{

    		lockRotation = false; 

            previousCheckpoint = curWaypoint; 
            
            curWaypoint.Hide(); 

            curwaypointindex++; 

            StopCoroutine("IStop"); 

            voice.Play(3); 

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

            StopCoroutine("SelectNextCheckpoint"); 

            StartCoroutine("SelectNextCheckpoint");         
        } 
    }

    private IEnumerator SelectNextCheckpoint()
    {
        yield return new WaitForSeconds(1); 

        targetObj.transform.position = path[curwaypointindex].transform.position;  

    }

    private void UpdateLookTimer()
    {
    	if(lookingAtNextCheckpoint)
    	{
    		lookingTimer+=Time.deltaTime; 

    		if(lookingTimer > movementDelay)
    		{
    			startMoving = true; 
    		}
    	}
    	else
    	{
    		startMoving = false; 

    		lookingTimer = 0; 
    	}
    }

    private void UpdateKeys()
    {
    	if(Input.GetAxis("Horizontal") == 0) return; 

    	rotationOffsetValue+=Input.GetAxis("Horizontal"); 

    	rotationOffsetSlider.value = rotationOffsetValue; 
    }



    public float GetDistanceToTarget()
    {
    	float distance = Vector3.Distance(transform.position, targetObj.transform.position); 

    	return distance; 
    }

    public float GetDistance(Waypoint w)
    {
        return Vector3.Distance(transform.position, w.transform.position); 
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

        if(!lookingAtNextCheckpoint)
        {

            float slightAngle = 65; 
            StopCoroutine("IStop");
            startIStop = false; 
            float angle = GetAngle(); 


            if(angle > 0 && angle < slightAngle)
            {
                voice.Play(4); 
            }

            if(angle >= slightAngle)
            {
                voice.Play(0); 
            }

            if(angle < 0 && angle > -slightAngle)
            {
                voice.Play(5); 
            }
            else if(angle <=-slightAngle)
            {
                voice.Play(1); 
            }
        }
        else
        {
            if(!startIStop)
            {
                StopCoroutine("IStop"); 

                StartCoroutine("IStop");         
            }                
        }
    }

    bool startIStop; 

    IEnumerator IStop()
    {

        startIStop = true; 

        if(previousCheckpoint != null)
        {
            if(GetDistance(previousCheckpoint) < .1f)
            {
                voice.Play(3);                  
                yield return new WaitForSeconds(1); 
            }
        }

        while(true)
        {
            voice.Play(2); 
            yield return null; 
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

    	if(!lookingAtNextCheckpoint)
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

    public void SetNorth()
    {
    	FindObjectOfType<UserRotation>().SetNorth(); 
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

    public void ExperimentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
    }
}
