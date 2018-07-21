using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBuilder : MonoBehaviour {
    public GameObject wall_prefab;
    public Transform wall_parent;
    public Vector2[] positions;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Transform tempWall;
    Vector3 tempScale = new Vector3(.2f, 3, 1);
    Vector3 tempPos, nextPos;
    public void BuildLocation()
    {
        if (positions.Length == 0)
            return;

        tempPos = nextPos = Vector3.zero;
        tempPos.x = positions[0].x;
        tempPos.z = positions[0].y;

        int childcount = wall_parent.childCount;
        //delete all previous walls
        while(childcount > 0)
        {
            DestroyImmediate(wall_parent.GetChild(0).gameObject);
            childcount--;
        }

        //iterate through positions adding new walls (face next position)
        for(int i = 0; i < positions.Length - 1; i++)
        {
            //spawn wall
            tempWall = Instantiate(wall_prefab, tempPos, Quaternion.identity, wall_parent).transform;
            //get next pos
            nextPos.x = positions[i + 1].x;
            nextPos.z = positions[i + 1].y;
            //look at next pos
            tempWall.LookAt(nextPos, Vector3.up);
            //scale to next pos
            tempScale.z = Vector3.Distance(tempPos, nextPos);
            tempWall.localScale = tempScale;
            //set name
            tempWall.name = "Wall (" + i + ")";
            //set temp pos to next pos
            tempPos = nextPos;
        }
        //add the last vertices that loops to start
        tempWall = Instantiate(wall_prefab, tempPos, Quaternion.identity, wall_parent).transform;
        nextPos.x = positions[0].x;
        nextPos.z = positions[0].y;
        tempWall.LookAt(nextPos, Vector3.up);
        tempScale.z = Vector3.Distance(tempPos, nextPos);
        tempWall.localScale = tempScale;
        tempWall.name = "Wall (" + (positions.Length - 1) + ")";
    }
}
