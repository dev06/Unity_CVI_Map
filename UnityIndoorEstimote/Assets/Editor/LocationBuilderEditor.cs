using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocationBuilder))]
public class LocationBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        LocationBuilder myScript = (LocationBuilder)target;
        if(GUILayout.Button("Build Location"))
        {
            myScript.BuildLocation();
        }
    }
}
