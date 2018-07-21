using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Notification : MonoBehaviour {

    public Toggle will_notify;

    //called when made
    public abstract void Start();
    //return how long to wait before another notification
    public abstract float Notify(float distance, float rotation);
}
