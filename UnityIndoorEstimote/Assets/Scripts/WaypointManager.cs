using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour {
    static WaypointManager instance;
    [Header("Testing")]
    public Waypoint testStart;
    public Waypoint testEnd;
    public Waypoint[] searchSpace;

    [Header("Checkpoints")]
    public List<Waypoint> visited = new List<Waypoint>();
    public Checkpoint checkpoint;           //Where the next checkpoint is
	// Use this for initialization
	void Start () {
        instance = this;
        
        //Waypoint[] pathMaybe;
        //if (searchSpace.Length > 0)
        //    pathMaybe = FindPathToWaypoint(testStart, testEnd, searchSpace);
        //else
        //    pathMaybe = FindPathToWaypoint(testStart, testEnd);
        //if (pathMaybe != null)
        //{
        //    string s = "";
        //    for (int i = 0; i < pathMaybe.Length; i++)
        //    {
        //        s += "" + pathMaybe[i].name + ", ";
        //    }
        //    Debug.Log(s);
        //}
	}

    public void SetCheckpointDistance(float newDist)
    {
        checkpoint.comfortable_distance = newDist;
    }

    public static Waypoint[] FindPathToWaypoint(Waypoint start, Waypoint end, Waypoint[] visited = null)
    {
        List<Waypoint> searchSpace = new List<Waypoint>();      //hold what we can see
        List<Waypoint> searching = new List<Waypoint>();        //hold what we currently want to search
        List<Waypoint> searched = new List<Waypoint>();         //hold what we already searched

        if (visited == null)
        {
            //add all the waypoints we have in scene (find shortest global path)
            searchSpace.AddRange(FindObjectsOfType<Waypoint>());
        }
        else
        {
            //only add the waypoints we visited (find shortest local path)
            searchSpace.AddRange(visited);
        }

        //if start or end arent there, we have error
        if(!searchSpace.Contains(start))
        {
            Debug.LogError("Start not in searchSpace");
            return null;
        }
        if(!searchSpace.Contains(end))
        {
            Debug.LogError("End not in searhSpace");
            return null;
        }

        //setup temp distance to prepare to find shortest path
        for(int i = 0; i < searchSpace.Count; i++)
        {
            searchSpace[i].tempDistance = 99999;
        }

        //start at end and set distances of each waypoint as we visit them (Uniform Cost Search through tree)
        end.tempDistance = 0;
        searching.Add(end);

        float minDist;
        Waypoint minWaypoint;
        while (searching.Count > 0)
        {
            //find minimum distance waypoint
            minDist = searching[0].tempDistance;
            minWaypoint = searching[0];
            for (int i = 1; i < searching.Count; i++)
            {
                if(searching[i].tempDistance < minDist)
                {
                    minDist = searching[i].tempDistance;
                    minWaypoint = searching[i];
                }
            }

            //remove from searching and add neighbors to searching (if in unvisited)
            searching.Remove(minWaypoint);
            searched.Add(minWaypoint);
            for (int i = 0; i < minWaypoint.neighbors.Length; i++)
            {
                //if we already searched, dont add
                if (!searched.Contains(minWaypoint.neighbors[i].waypoint))
                {
                    //if its not in searchSpace, dont add
                    if (searchSpace.Contains(minWaypoint.neighbors[i].waypoint))
                    {
                        searching.Add(minWaypoint.neighbors[i].waypoint);
                        minWaypoint.neighbors[i].waypoint.SetTempDistanceIfSmaller(minWaypoint.tempDistance + Vector3.Distance(minWaypoint.transform.position, minWaypoint.neighbors[i].waypoint.transform.position));
                    }
                }
            }
        }

        //find shortest path
        List<Waypoint> finalPath = new List<Waypoint>();
        Waypoint curWaypoint = start;
        minWaypoint = start;
        finalPath.Add(start);
        //while we havent found end
        int num = 0;    //used to see if we iterate through much, something went wrong
        while(finalPath[finalPath.Count-1].tempDistance != 0 && num < 2000)
        {
            //find minimum neighbor that is in search space
            minDist = 99999;
            for(int i = 0; i < curWaypoint.neighbors.Length; i++)
            {
                if(searchSpace.Contains(curWaypoint.neighbors[i].waypoint))
                {
                    if(curWaypoint.neighbors[i].waypoint.tempDistance < minDist)
                    {
                        minWaypoint = curWaypoint.neighbors[i].waypoint;
                        minDist = curWaypoint.neighbors[i].waypoint.tempDistance;
                    }
                }
            }

            //set curwaypoint to the minimum and add to final path
            curWaypoint = minWaypoint;
            finalPath.Add(curWaypoint);
            num++;
        }

        if (num < 2000)
        {
            //return shortest path
            return finalPath.ToArray();
        }

        //can't find a path
        return null;
    }
}