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

    //[HideInInspector]
    public GameObject firstPoint { get; private set; }
    float segmentDistanceGridmap;

    [SerializeField]
    List<GameObject> MapGridPoints_GO;    // list of points on the grid map


    void Start()
    {
        segmentDistanceGridmap = GameManager.Instance.Settings.SegmentSizeMaster;
    }

    //used in old "pathfinder"
    public List<GameObject> Get_ListOfMapGridPoints_GOS()
    {
        return MapGridPoints_GO;
    }

    public List<GameObject> Get_COPY_OF_ListOfMapGridPoints_GOS()
    {
        List<GameObject> copylist = new List<GameObject>();
        foreach (GameObject go in MapGridPoints_GO)
            copylist.Add(go);
        return copylist;
    }


    public GameObject GetClosestGridPoint(GameObject obj)
    {
        Vector3 transPos = new Vector3(obj.transform.position.x, gridHeight, obj.transform.position.z);
        GameObject closestPoint = null;
        float distance = -1.0f;

        foreach (GameObject p in MapGridPoints_GO)
        {
            float thisDistance = Vector3.Distance(p.transform.position, transPos);
            if (distance < 0.0f)
            {
                distance = thisDistance;
                closestPoint = p;
            }
            else
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

    public void CreateGrid()
    {
        Scan();
    }

    void Scan()
    {
        MapGridPoints_GO = new List<GameObject>();
        Queue<GameObject> toVisit = new Queue<GameObject>();

        // create the first point and add it to the gridpoint list and the toVisit list
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + gridHeight, transform.position.z);
        // Vector3 startPosition = new Vector3(transform.position.x, transform.position.y , transform.position.z);

        GameObject point = Instantiate(gridPoint, startPosition, Quaternion.identity) as GameObject;
        GridPoint gp = point.GetComponent<GridPoint>();
        point.name = gp.GridpointName;
        point.transform.parent = this.transform;

        //segmentDistanceGridmap = gp.GetSegmentDist();
        if (!GameManager.Instance.Settings.IsShowGridPointMesh) gp.turnCubeMeshOff();

        firstPoint = point;
        point.name += MapGridPoints_GO.Count.ToString();
        MapGridPoints_GO.Add(point);

        toVisit.Enqueue(point);


        // while there are points in the queue, tell each one to scan for new points
        while (toVisit.Count > 0)
        {
            GameObject p = toVisit.Dequeue();
            p.GetComponent<GridPoint>().Scan(MapGridPoints_GO, toVisit, this.transform);
        }

        // remove points that are too close to spatial mesh
        GM_CheckBounds();

        ////remove any points that are not connected to root node (firstPoint)
        //CheckConnectivity();
    }

    public void ResetGridPointValues()
    {
        foreach (GameObject p in MapGridPoints_GO)
        {
            p.GetComponent<GridPoint>().Init_Times_Used_in_a_path();
        }
    }


    void GM_CheckBounds()
    {
        // remove any points that are too close to the spatial map
        // Debug.Log("Number of points: " + points.Count);
        List<GameObject> removedPoints = new List<GameObject>();
        foreach (GameObject p in MapGridPoints_GO)
        {
            // if the point has been removed
            if (!p.GetComponent<GridPoint>().GP_CheckBounds())
            {
                removedPoints.Add(p);
            }
        }

        foreach (GameObject p in removedPoints)
        {
            MapGridPoints_GO.Remove(p);
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
            if (p.forwardGameObject != null && !p.forwardGameObject.GetComponent<GridPoint>().GP_Connected)
                toVisit.Enqueue(p.forwardGameObject);
            if (p.backGameObject != null && !p.backGameObject.GetComponent<GridPoint>().GP_Connected)
                toVisit.Enqueue(p.backGameObject);
            if (p.leftGameObject != null && !p.leftGameObject.GetComponent<GridPoint>().GP_Connected)
                toVisit.Enqueue(p.leftGameObject);
            if (p.rightGameObject != null && !p.rightGameObject.GetComponent<GridPoint>().GP_Connected)
                toVisit.Enqueue(p.rightGameObject);
        }

        // check connectivity, adding points that are not connected to the list to be removed
        foreach (GameObject p in MapGridPoints_GO)
        {
            if (!p.GetComponent<GridPoint>().GP_CheckConnectivity())
            {
                removedPoints.Add(p);
            }
        }

        // remove points from main points list
        foreach (GameObject p in removedPoints)
        {
            MapGridPoints_GO.Remove(p);
        }
    }
}
