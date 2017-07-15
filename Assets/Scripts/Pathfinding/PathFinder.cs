// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour{

    public float segmentDistance;                               // distance between nodes


    [HideInInspector]
    public Stack<PathNode> finalPath { get; private set; }      // the stack of path nodes representing path to target
    public bool isFinding { get; private set; }                 // flag that pathfinder is searching

    List<PathNode> created;                                     // list of path nodes that have already been created
    Queue<PathNode> toVisit;                                    // the stack of path nodes that have not been visited
    PathNode targetNode;                                        // the node representing the final destination - algorithim creates path from this point

    float bestCost;                                             // lowest distance to target - officially set the first time target is found
    bool targetFound;                                           // flag that destination has been reached at least once
    void Awake()
    {
        finalPath = null;
    }

    public void FindPath(GameObject startPoint, GameObject targetPoint)
    {
        //if (startPoint == null) Debug.Log("Start point null");
        //if (targetPoint == null) Debug.Log("Target point null");

        StartCoroutine(GetNewPath(startPoint, targetPoint));
    }

    IEnumerator GetNewPath(GameObject startPoint, GameObject targetPoint)
    {
        //Debug.Log("Finding path...");
        isFinding = true;

        if (startPoint == null)
        {
            GridMap grid = GameObject.FindObjectOfType<GridMap>();
            List<GameObject> points = grid.GetGridMap();
            Vector3 transPos = new Vector3(transform.position.x, grid.gridHeight, transform.position.y);
            foreach (GameObject p in points)
            {
                if (Vector3.Distance(p.transform.position, transPos) < segmentDistance)
                {
                    startPoint = p;
                    break;
                }
            }
        }
        //else Debug.Log("Startpoint not null");

        created = new List<PathNode>();
        toVisit = new Queue<PathNode>();
        Stack<PathNode> path = new Stack<PathNode>();
        finalPath = null;

        // target has not been found
        bestCost = -1.0f;
        targetFound = false;

        PathNode startNode = new PathNode(startPoint, segmentDistance, null, 0.0f);
        created.Add(startNode);
        toVisit.Enqueue(startNode);

        //Debug.Log("Entering loop...");
        int count = 0;
        while (toVisit.Count > 0)
        {
            // Debug.Log("Number of Nodes toVisit: " + toVisit.Count);
            PathNode n = toVisit.Dequeue();
            PathNode target = n.Visit(created, toVisit, startPoint, targetPoint, bestCost, targetFound);
            if (target != null)
            {
              
                targetFound = true;
                targetNode = target;
            }
            if (targetFound) {
               
                bestCost = targetNode.cost;
            }
               
            count++;

            // yield to frame every 500 nodes checked
            if (count >= 500)
            {
                count = 0;
                yield return null;
            }
        }
        //Debug.Log("End of loop...");

        //Debug.Log("Entering path construction loop...");
        // construct path
        PathNode currentNode = targetNode;
        while (currentNode.previousNode != null)
        {
            path.Push(currentNode);
            currentNode = currentNode.previousNode;
        }
        //Debug.Log("End of loop...");

        finalPath = path;

        SendMessage("SetPath");

        isFinding = false;
        // Debug.Log("Path found...");
    }
}
