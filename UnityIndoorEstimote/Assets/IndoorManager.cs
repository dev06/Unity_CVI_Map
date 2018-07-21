using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class IndoorManager : MonoBehaviour
{
    static IndoorManager instance;
   /* public static float UserX { get { return (float)instance.x; } }
    public static float UserY { get { return (float)instance.y; } }
    public static bool InRoom {
        get {
            if (!instance)
                return false;
            return instance.x != 0 || instance.y != 0;
            } }*/

            public Text text;

            public IndoorLocationManager indoorManager = IndoorLocationManagerFactory.Create ();

            void Start ()
            {
                instance = this;
		//indoorManager.Start ("<APP ID>", "<APP TOKEN>", "<LOCATION IDENTIFIER>");
                indoorManager.Start("unity-estimote-integration-g47", "22c52e95562a9bfbe6ad2ecd2f72d266", "3rd-floor-stairs");
            }

            public static double x, y;
            void Update ()
            {
              if (indoorManager.IsInsideLocation ()) {
               x = indoorManager.GetX ();
               y = indoorManager.GetY ();

             //    text.text = string.Format ("Pos: x = {0:0.00}, y = {1:0.00}", x, y);
           } else {
            x = y = 0;
//			text.text = "Outside location";
        }
    }

}