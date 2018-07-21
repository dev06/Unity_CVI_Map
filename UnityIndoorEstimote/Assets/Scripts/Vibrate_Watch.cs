using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Vibrate_Watch : MonoBehaviour {
    static Vibrate_Watch instance;
    public static void VIBRATE_LEFT(int numTimes, int timeOn, int timeOff) { if (instance) instance.TryVibrateLeft(numTimes, timeOn, timeOff); }
    public static void VIBRATE_RIGHT(int numTimes, int timeOn, int timeOff) { if (instance) instance.TryVibrateRight(numTimes, timeOn, timeOff); }
    public Text text_status;    //show the status on screen in top rights
	// Use this for initialization
    void Start () {
        text_status.text = "";
        instance = this;
    }

    public void TryInitLeft()
    {
        text_status.color = Color.white;
        text_status.text += "Trying to init...\n";
        try
        {
            //instance of Koi plugin
            AndroidJavaObject androidObject;
            //Reference to Unity Player
            AndroidJavaObject unityObject;


            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass androidClass = new AndroidJavaClass("com.kennesaw.guitar.pebblelibrary.VibratePebble");
            androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

            text_status.text += "~~~ MadeObjectsAndClasses...";
            text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
            //androidObject.Call("VibrateWatch");
            androidObject.Call("InitWatch", unityObject);
        }
        catch(System.Exception e)
        {
            text_status.text += "" + e.Message + "\n";
        }
    }

    public void TestVibrateLeft() { TryVibrateLeft(1, 128, 256); }
    public void TestVibrateRight() { TryVibrateRight(1, 128, 256); }

    public void TestVibrateRight(int time)
    {
        TryVibrateRight(time, 128, 256);
    }

    public void TestVibrateLeft(int time)
    {
        TryVibrateLeft(time, 128, 256);
    }

    /// <summary>
    /// Call to send message to android studio to send vibration to pebble
    /// </summary>
    public void TryVibrateLeft(int numTimes, int timeOn, int timeOff)
    {
        text_status.color = Color.white;
        text_status.text += "Trying to vibrate...";

        //instance of Koi plugin
        AndroidJavaObject androidObject;
        //Reference to Unity Player
        AndroidJavaObject unityObject;


        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass androidClass = new AndroidJavaClass ("com.kennesaw.guitar.pebblelibrary.VibratePebble");
        androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

        text_status.text += "MadeObjectsAndClasses...";
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
        //androidObject.Call("VibrateWatch");
        androidObject.Call("VibrateWatch", unityObject, numTimes, timeOn, timeOff);
    }

    public void TryInitRight()
    {
        text_status.color = Color.white;
        text_status.text += "Trying to init android...\n";
        try
        {
            //instance of Koi plugin
            AndroidJavaObject androidObject;
            //Reference to Unity Player
            AndroidJavaObject unityObject;


            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass androidClass = new AndroidJavaClass("com.kennesaw.guitar.pebblelibrary.VibrateWear");
            androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

            text_status.text += "~~~ MadeObjectsAndClasses...";
            text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
            //androidObject.Call("VibrateWatch");
            androidObject.Call("InitWatch", unityObject);
        }
        catch (System.Exception e)
        {
            text_status.text += "" + e.Message + "\n";
        }
    }

    /// <summary>
    /// Call to send message to android studio to send vibration to pebble
    /// </summary>
    public void TryVibrateRight(int numTimes, int timeOn, int timeOff)
    {
        text_status.color = Color.white;
        text_status.text += "Trying to vibrate android...";

        //instance of Koi plugin
        AndroidJavaObject androidObject;
        //Reference to Unity Player
        AndroidJavaObject unityObject;


        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass androidClass = new AndroidJavaClass("com.kennesaw.guitar.pebblelibrary.VibrateWear");
        androidObject = androidClass.GetStatic<AndroidJavaObject>("m_instance");

        text_status.text += "MadeObjectsAndClasses...";
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
        //androidObject.Call("VibrateWatch");
        androidObject.Call("VibrateWatch", unityObject, numTimes, timeOn, timeOff);
    }

    /// <summary>
    /// Called when pebble is vibrated
    /// </summary>
    public void VibrateSuccess(string message)
    {
        text_status.text += "++ Successfully vibrated!";
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
    }

    /// <summary>
    /// Called when pebble fails to vibrate
    /// </summary>
    public void VibrateFail(string error)
    {
        text_status.text += "\n -- Error: " + error;
        text_status.text += "\n- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -\n";
    }
}
