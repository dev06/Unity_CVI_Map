using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {
    public static Vector3 user_position;
    public static float user_localRotation;
    public static Vector3 user_forward;
    
    public Waypoint[] path;
    protected int curwaypointindex = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
