// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {

    public PathNode previousNode { get; private set; }
    public GameObject gridPoint { get; private set; }
    public float cost { get; private set; }
    public float segmentDistance { get; private set; }

  

	public PathNode(GameObject thisPoint, float segmentDistance, PathNode previousNode, float cost)
    {
         
        gridPoint = thisPoint;
        this.segmentDistance = segmentDistance;
        this.previousNode = previousNode;
        this.cost = cost;

         
    }

    public void UpdateNode(PathNode previousNode, float cost)
    {
        this.previousNode = previousNode;
        this.cost = cost;
 

    }

    // returns a PathNode if this is the targetPoint
    public PathNode Visit(List<PathNode> created, Queue<PathNode> toVisit, GameObject startPoint, GameObject targetPoint, float bestCost, bool targetFound)
    {
        // if this node is the target then return this node
        if (gridPoint == targetPoint)
            return this;
        

        // if target has been found then start pruning
        if (targetFound)
        {
            // if bestCost is less than this node cost + distace to target then prune
            if (bestCost <= cost + Vector3.Distance(gridPoint.transform.position, targetPoint.transform.position))
                return null;
        }

        // check all connected gridpoints and create PathNodes or Update existing ones as neccessary
        GridPoint point = gridPoint.GetComponent<GridPoint>();
        if (point.forwardGameObject != null)
            CheckNode(created, toVisit, point.forwardGameObject);

        if (point.leftGameObject != null)
            CheckNode(created, toVisit, point.leftGameObject);

        if (point.rightGameObject != null)
            CheckNode(created, toVisit, point.rightGameObject);

        if (point.backGameObject != null)
            CheckNode(created, toVisit, point.backGameObject);

        return null;
    }

    private void CheckNode(List<PathNode> created, Queue<PathNode> toVisit, GameObject pointToCheck)
    {
        // if pointToCheck has been created
        PathNode point = IsCreated(created, pointToCheck);
        if (point != null)
        {
            // if new cost is better then old cost
            // update node
            float newCost = cost + segmentDistance;
            if (newCost < point.cost)
            {
                point.UpdateNode(this, newCost);
            }
        }
        else
        {
            // create PathNode
            point = new PathNode(pointToCheck, segmentDistance, this, cost + segmentDistance);
            
            // add the new node to created
            created.Add(point);

            // add the new node to toVisit 
            toVisit.Enqueue(point);
        }
    }

    private PathNode IsCreated(List<PathNode> created, GameObject pointToCheck)
    {
        // if pointToCheck has been visited already then return it
        foreach (PathNode p in created)
        {
            if (p.gridPoint == pointToCheck)
                return p;
        }
        
        return null;
    }
}
