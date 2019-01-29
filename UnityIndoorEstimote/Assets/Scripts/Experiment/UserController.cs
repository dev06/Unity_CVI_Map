using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserController : MonoBehaviour
{

	public Transform pingTransform;

	public GameObject miniCheckpointPrefab;

	public float speed = .2f;

	public float targetDistance = .5f;

	public GameObject targetObj;

	public Text isLookingAtTarget;

	public Slider rotationSlider;

	public Waypoint[] path;

	private int curwaypointindex = 0;

	private int nextwaypointindex = 0;

	public float user_localRotation;

	public Slider slider;

	public Toggle pingToggle, vibrationToggle, voiceToggle;

	private Feedback_Voice voice;

	private Feedback_Vibrate vibrate;

	private bool tutorialOver;

	private float rotationOffsetValue;

	private bool chooseNext;

	private float keyRot;

	private bool lookingAtNextCheckpoint;

	private Quaternion storedRotation;

	private bool startMoving;

	private float lookingTimer;

	private float movementDelay = .4f;

	private Waypoint curWaypoint, previousCheckpoint;

	private Vector3 targetPos;

	private bool moveWaypoint = true;

	private bool startIStop;

	private Vector3 targetPosition, lastPosition;

	private List<string> dataLines = new List<string>();

	private float recordDataTimer;

	private int csv_id;


	void Start ()
	{

		dataLines.Add("id  ,  timestamp  ,  x  ,  y  ,  direction  ,  feedbacktype  ,  checkpoint");

		curwaypointindex = 1;

		curWaypoint = path[curwaypointindex];

		previousCheckpoint = path[curwaypointindex - 1];

		curWaypoint.Show();

		transform.position = path[0].transform.position;

		lastPosition = transform.position;

		targetPosition = transform.position;

		targetObj.transform.position = targetPosition;

		targetPos = targetObj.transform.position;

		pingTransform.position = curWaypoint.transform.position;

		voice = FindObjectOfType<Feedback_Voice>();

		vibrate = FindObjectOfType<Feedback_Vibrate>();


		ShowPathToNextPoint();

		StopCoroutine("IUpdateNorthOnDelay");

		StartCoroutine("IUpdateNorthOnDelay");

	}


	void Update()
	{
		if (PauseHandler.PAUSE)
		{
			return;
		}

		UpdateKeys();

		UpdateRotationOffsetKeys();

		UpdatePCControls();

		lookingAtNextCheckpoint = Vector3.Angle(transform.position - curWaypoint.transform.position, -transform.forward) < 15;

		isLookingAtTarget.text = "Looking At Target: " + lookingAtNextCheckpoint;

		if (Input.GetButtonDown("Fire6"))
		{
			GetNextCheckpoint();
		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			GetNextCheckpoint();
		}

		//transform.position = Vector3.Lerp(transform.position, targetObj.transform.position, Time.deltaTime * slider.value * speed);

		transform.position = targetPosition;

		if (Input.GetButtonDown("North"))
		{
			FindObjectOfType<UserRotation>().SetNorth();
		}

		if (Input.GetButtonDown("NextScene"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

		PlayVoice();

		//VibrateWatches();

		recordDataTimer += Time.deltaTime;

		if (recordDataTimer > 1f)
		{
			string feedbacktype = "None"; //Ping

			if (voiceToggle.isOn)
			{
				feedbacktype = "voice";
			}
			if (vibrationToggle.isOn)
			{
				feedbacktype = "vibrate";
			}
			if (pingToggle.isOn)
			{
				feedbacktype = "ping";
			}

			dataLines.Add(csv_id + "  ,  " + System.DateTime.Now + "  ,  " + transform.position.x + "  ,  " + transform.position.z + "  ,  " + UserRotation.GetRotation() + "  ,  " + feedbacktype + "  ,  " + curwaypointindex);

			csv_id++;

			recordDataTimer = 0;
		}
	}

	private void UpdatePCControls()
	{
		if (Input.GetKey(KeyCode.D))
		{
			keyRot += 1f;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			keyRot -= 1f;
		}

		user_localRotation = UserRotation.GetRotation();

#if UNITY_EDITOR
		transform.rotation = Quaternion.Euler(0, keyRot, 0);
#else
		transform.rotation = Quaternion.Euler(0, user_localRotation, 0);
#endif
	}



	IEnumerator IUpdateNorthOnDelay()
	{
		yield return new WaitForSeconds(.3f);
		FindObjectOfType<UserRotation>().SetNorth();
	}

	public void GetNextCheckpoint()
	{
		Vector3 offset = curWaypoint.transform.position - targetPosition;

		// targetPosition = targetPosition + (offset * .33f);

		//Vector3 dist = curWaypoint.transform.position - targetPosition;

		if (offset.sqrMagnitude < targetDistance * 3f)
		{
			targetPosition = curWaypoint.transform.position;

			SetNextWaypoint();
		}
		else
		{
			offset.Normalize();

			targetPosition = targetPosition + (offset * targetDistance);
		}

		targetObj.transform.position = targetPosition;
	}

	private void SetNextWaypoint()
	{
		curWaypoint.Hide();

		previousCheckpoint = curWaypoint;

		curwaypointindex++;

		if (curwaypointindex > path.Length - 1)
		{

			curwaypointindex = path.Length - 1;

			Debug.Log("Out of points");

			DataSaver.Save(dataLines);

			voice.Stop();
		}

		curWaypoint = path[curwaypointindex];

		pingTransform.transform.position = curWaypoint.transform.position;

		curWaypoint.Show();

		ShowPathToNextPoint();
	}


	public void ShowPathToNextPoint()
	{
		GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Minicheckpoint");

		for (int i = 0; i < checkpoints.Length; i++)
		{
			Destroy(checkpoints[i]);
		}

		for (int i = 0; i < Mathf.Ceil(Vector3.Distance(curWaypoint.transform.position, previousCheckpoint.transform.position) / targetDistance); i++)
		{
			GameObject c = Instantiate(miniCheckpointPrefab);

			Vector3 pos = curWaypoint.transform.position - previousCheckpoint.transform.position;

			c.transform.position = previousCheckpoint.transform.position + pos * ((i + .5f) / 5f);

			pos.Normalize();

			c.transform.position = previousCheckpoint.transform.position + pos * i * targetDistance;
		}
	}


	public float GetDistanceToTarget()
	{
		return Vector3.Distance(transform.position, curWaypoint.transform.position);
	}

	float GetDistance(Waypoint w)
	{
		return Vector3.Distance(transform.position, w.transform.position);
	}

	private void PlayVoice()
	{
		if (tutorialOver)
		{
			voice.Stop();
			return;
		}

		if (!voice.selected) return;

		float angle = GetAngle();

		if (!lookingAtNextCheckpoint)
		{

			float slightAngle = 65;
			StopCoroutine("IStop");
			startIStop = false;

			if (angle > 0 && angle < slightAngle)
			{
				voice.Play(4);
			}
			else if (angle >= 30)
			{
				voice.Play(0);
			}

			if (angle < 0 && angle > -slightAngle)
			{
				voice.Play(5);
			}
			else if (angle <= -slightAngle)
			{
				voice.Play(1);
			}

		}
		else
		{
			if (!startIStop)
			{
				StopCoroutine("IStop");
				StartCoroutine("IStop");
			}
		}
	}


	IEnumerator IStop()
	{
		startIStop = true;

		if (previousCheckpoint != null)
		{
			if (GetDistance(previousCheckpoint) < .1f)
			{
				voice.Play(3);
				yield return new WaitForSeconds(1);
			}
		}

		while (true)
		{
			voice.Play(2);

			yield return null;
		}
	}


	// private void VibrateWatches()
	// {
	//   if (tutorialOver)
	//   {
	//     return;
	//   }

	//   if (!vibrate.selected) return;

	//   float angle = GetAngle();

	//   if (!lookingAtNextCheckpoint)
	//   {
	//     if (angle >= 0)
	//     {
	//       vibrate.VibrateLeft();
	//     }
	//     else
	//     {
	//       vibrate.VibrateRight();
	//     }
	//   }
	// }

	private void UpdateRotationOffsetKeys()
	{
		if (Input.GetAxis("Horizontal") == 0) return;

		rotationOffsetValue += Input.GetAxis("Horizontal");

		rotationSlider.value = rotationOffsetValue;
	}

	public float GetAngle()
	{
		return Vector3.SignedAngle(transform.position - curWaypoint.transform.position, -transform.forward, Vector3.up);
	}

	public void SetNorth()
	{
		FindObjectOfType<UserRotation>().SetNorth();
	}

	IEnumerator IReload()
	{
		yield return new WaitForSeconds(.5f);

		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}


	private void UpdateKeys()
	{

		if (Input.GetButtonDown("Fire2")) // B
		{

			voiceToggle.isOn = true;

			pingToggle.isOn = false;

			vibrationToggle.isOn = false;
		}

		if (Input.GetButtonDown("Fire1")) // A
		{
			voiceToggle.isOn = false;

			pingToggle.isOn = true;

			vibrationToggle.isOn = false;
		}

		if (Input.GetButtonDown("Fire3")) // A
		{
			voiceToggle.isOn = false;

			pingToggle.isOn = false;

			vibrationToggle.isOn = true;
		}
	}
}


