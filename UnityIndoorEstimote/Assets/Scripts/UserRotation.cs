using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserRotation : MonoBehaviour {
    static UserRotation instance;
    static float offset = 0;
    /// <summary>
    /// Return the room-local rotation of the phone from 0-360: North = 1, South = -1
    /// </summary>
    /// <param name="localRotation">
    /// Represents where North is in the local room
    /// </param>
    /// <returns></returns>
    public static float GetRotation()
    {
        float value = currentRotation;
        value -= RoomManager.RoomNorth;
        if (value < 0)
        value += 360;
        else if (value >= 360)
        value -= 360;
        return value + offset;
    }

    public void SetOffset(float f) { offset = f; }
    public void SetNorth()
    {
        float value = currentRotation;
        value -= RoomManager.RoomNorth;
        if (value < 0)
        value += 360;
        else if (value >= 360)
        value -= 360;
        offset = -value;
    }
    public void SetEast()
    {
        float value = currentRotation;
        value -= RoomManager.RoomNorth;
        if (value < 0)
        value += 90;
        else if (value >= 90)
        value -= 90;
        offset = -value;
    }

    public Transform measurePoint;
    Gyroscope gyro;
    public static float currentRotation;
    List<MeshRenderer> meshRenderers;
    // Use this for initialization
    void Start () {
        instance = this;
        //enable the gyro
        gyro = Input.gyro;
        gyro.enabled = true;
        //get meshrenderers on object (used for debugging)
        meshRenderers = new List<MeshRenderer>();
        meshRenderers.Add(GetComponent<MeshRenderer>());
        meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        Hide();
    }

    Vector3 tempvec;
    float tempangle;
	// Update is called once per frame
    void Update () {
        //rotate object to be phone rotation
        transform.rotation = gyro.attitude;

        //get the measurement position where x,y represents the phone rotation
        tempvec = -measurePoint.position;
        tempvec.z = 0;  //need to set z to 0 for angle to work correctly
        //find the angle from the xy position compared to (0,1)
        tempangle = Vector3.SignedAngle(Vector3.up, tempvec, Vector3.forward);

        //make angle from 0-360
        if (tempangle < 0)
        tempangle += 360;

        //current rotation, 0/360 is north, 90 is east, ...
        currentRotation = 360-tempangle;
        //base the rotation in terms of the room rotation
        currentRotation -= RoomManager.RoomNorth;
        //restrict from [0,360)
        if (currentRotation < 0)
        currentRotation += 360;
        else if (currentRotation >= 360)
        currentRotation -= 360;
    }

    //used for debugging, show/hide the xyz rotation
    public void Show()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = true;
        }
    }
    public void Hide()
    {
        foreach(MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }
    }

    
}
