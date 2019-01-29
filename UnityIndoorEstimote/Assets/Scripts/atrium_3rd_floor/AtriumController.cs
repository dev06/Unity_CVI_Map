using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtriumController : MonoBehaviour {

	public static AtriumController Instance;

	public bool ActivateTutorial;

	public bool isTraining;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start ()
	{

	}

	void Update () {

	}
}
