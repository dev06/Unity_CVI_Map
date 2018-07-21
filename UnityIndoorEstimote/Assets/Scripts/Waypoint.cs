using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to keep track of the waypoints, contains the neighbors it can move to
/// </summary>
public class Waypoint : MonoBehaviour {
    public Neighbor[] neighbors;    //which waypoints I can move to
    int myLayer;
    bool visited;       //stores if this waypoint has been visited

    public float tempDistance = -1;
    public void SetTempDistanceIfSmaller(float newDist)
    {
        if (newDist < tempDistance)
        tempDistance = newDist;
    }

    private void Awake()
    {
        //set my layer to same layer as everyone else (if not already set)
        myLayer = gameObject.layer;
        tempDistance = -1;
        visited = false;
        FindNeighbors();
        Hide();
    }

    public void Visit()
    {
        WaypointManager wm = FindObjectOfType<WaypointManager>();
        if(!wm.visited.Contains(this))
        wm.visited.Add(this);
        visited = true;
    }

    public void OnMouseDown()
    {
        try
        {
            FindObjectOfType<UserAvatar>().SetEndWaypoint(this);

        }
        catch(System.Exception e)
        {

        }
    }

    public void FindNeighbors()
    {
        List<Neighbor> temp = new List<Neighbor>();
        //go through current neighbors and keep the ones that are preset
        for(int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i].preset)
            temp.Add(neighbors[i]);
        }

        //raycast in each direction to find neighbors
        RaycastHit hit;
        Neighbor newNeigh;
        Waypoint newWaypoint;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100))
        {
            newWaypoint = hit.collider.gameObject.GetComponent<Waypoint>();
            if (newWaypoint)
            {
                newNeigh = new Neighbor(hit.collider.gameObject.GetComponent<Waypoint>(), false);
                if (!temp.Contains(newNeigh))
                temp.Add(newNeigh);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 100))
        {
            newWaypoint = hit.collider.gameObject.GetComponent<Waypoint>();
            if (newWaypoint)
            {
                newNeigh = new Neighbor(hit.collider.gameObject.GetComponent<Waypoint>(), false);
                if (!temp.Contains(newNeigh))
                temp.Add(newNeigh);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 100))
        {
            newWaypoint = hit.collider.gameObject.GetComponent<Waypoint>();
            if (newWaypoint)
            {
                newNeigh = new Neighbor(hit.collider.gameObject.GetComponent<Waypoint>(), false);
                if (!temp.Contains(newNeigh))
                temp.Add(newNeigh);
            }
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, 100))
        {
            newWaypoint = hit.collider.gameObject.GetComponent<Waypoint>();
            if (newWaypoint)
            {
                newNeigh = new Neighbor(hit.collider.gameObject.GetComponent<Waypoint>(), false);
                if (!temp.Contains(newNeigh))
                temp.Add(newNeigh);
            }
        }

        //set new neighbors
        neighbors = temp.ToArray();
    }


    public void Hide()
    {
        if(visited)
        GetComponent<SpriteRenderer>().color = Color.yellow;
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
            GetComponent<SpriteRenderer>().enabled = false; 
        }

        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void Show()
    {
        GetComponent<SpriteRenderer>().color =  new Color(.5f, 1f, .5f, 1f);
        GetComponent<SpriteRenderer>().enabled = true; 

        GetComponent<SpriteRenderer>().sortingOrder = 10;
    }
}

[System.Serializable]
public struct Neighbor
{
    public Waypoint waypoint;
    public bool preset;

    public Neighbor(Waypoint w, bool p)
    {
        waypoint = w;
        preset = p;
    }

    public override bool Equals(object obj)
    {
        return ((Neighbor)obj).waypoint == waypoint;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
