// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMap : MonoBehaviour {

    [Tooltip("Gridpoint prefab that is used and placed to create the gridmap.")]
    public GameObject gridPoint;
    
    [Tooltip("Layers that raycasts will hit (spatial mapping & other grid points)")]
    public LayerMask layerMask = Physics.DefaultRaycastLayers;

    [Tooltip("Height of gridmap in room")]
    public float gridHeight = 0.125f;

    [HideInInspector]
    public GameObject firstPoint { get; private set; }
    public float segmentDistance;

    List<GameObject> points;    // list of points on the grid map

    public bool isTestMode = false; 
	void Start () {
       
	}
	
	public List<GameObject> GetGridMap()
    {
        return points;
    }

   
    public GameObject GetClosestPoint(GameObject obj)
    {
        Vector3 transPos = new Vector3(obj.transform.position.x, gridHeight, obj.transform.position.z);
        GameObject closestPoint = null;
        float distance = -1.0f;

        foreach (GameObject p in points)
        {
            float thisDistance = Vector3.Distance(p.transform.position, transPos);
            if (distance < 0.0f)
            {
                distance = thisDistance;
                closestPoint = p;
            } else
            {
                if (thisDistance < distance)
                {
                    distance = thisDistance;
                    closestPoint = p;
                }
            }
        }

        return closestPoint;
    }

    public void CreateGrid(bool argCubeMeshOff)
    {
        Scan(argCubeMeshOff);
    }

    void Scan(bool argCubeMeshOff)
    {
        points = new List<GameObject>();
        Queue<GameObject> toVisit = new Queue<GameObject>();

        // create the first point and add it to the gridpoint list and the toVisit list
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + gridHeight, transform.position.z);

        GameObject point = Instantiate(gridPoint, startPosition, Quaternion.identity) as GameObject;

        GridPoint gp = point.GetComponent<GridPoint>();
        segmentDistance = gp.segmentDistance;
        if (argCubeMeshOff) gp.turnCubeMeshOff();

        firstPoint = point;
        points.Add(point);

        toVisit.Enqueue(point);

        // while there are points in the queue, tell each one to scan for new points
        while (toVisit.Count > 0)
        {
            GameObject p = toVisit.Dequeue();
            p.GetComponent<GridPoint>().Scan(points, toVisit);
        }

        // remove points that are too close to spatial mesh
        //CheckBounds();

        //remove any points that are not connected to root node (firstPoint)
        //CheckConnectivity();

        GameObject.Find("GameManager").GetComponent<GameManager>().GridBuilt(this);
    }

    void CheckBounds()
    {
        // remove any points that are too close to the spatial map
        // Debug.Log("Number of points: " + points.Count);
        List<GameObject> removedPoints = new List<GameObject>();
        foreach (GameObject p in points)
        {
            // if the point has been removed
            if (!p.GetComponent<GridPoint>().CheckBounds())
            {
                removedPoints.Add(p);
            }
        }

        foreach (GameObject p in removedPoints)
        {
            points.Remove(p);
        }
    }

    void CheckConnectivity()
    {
        // remove any points that are not connected to root node
        Queue<GameObject> toVisit = new Queue<GameObject>();
        List<GameObject> removedPoints = new List<GameObject>();

        // establish connectivity
        toVisit.Enqueue(firstPoint);
        while (toVisit.Count > 0)
        {
            GridPoint p = toVisit.Dequeue().GetComponent<GridPoint>();
            p.Connect();
            if (p.forwardGameObject != null && !p.forwardGameObject.GetComponent<GridPoint>().Connected)
                toVisit.Enqueue(p.forwardGameObject);
            if (p.backGameObject != null && !p.backGameObject.GetComponent<GridPoint>().Connected)
                toVisit.Enqueue(p.backGameObject);
            if (p.leftGameObject != null && !p.leftGameObject.GetComponent<GridPoint>().Connected)
                toVisit.Enqueue(p.leftGameObject);
            if (p.rightGameObject != null && !p.rightGameObject.GetComponent<GridPoint>().Connected)
                toVisit.Enqueue(p.rightGameObject);
        }

        // check connectivity, adding points that are not connected to the list to be removed
        foreach(GameObject p in points)
        {
            if (!p.GetComponent<GridPoint>().CheckConnectivity())
            {
                removedPoints.Add(p);
            }
        }

        // remove points from main points list
        foreach (GameObject p in removedPoints)
        {
            points.Remove(p);
        }
    }
}
