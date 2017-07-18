// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {
    public PathNode previousNode { get; private set; }
    public GameObject gridPointObj { get; private set; }
    GridPoint GPofThisNode;
    public GridPoint GetGPofThisNoe() { return GPofThisNode; }

    public float cost { get; private set; }
    public float segmentDistance { get; private set; }

    public void incrementCost()
    {
        this.cost = 0f;
    }

    public void setGP_toptext(float argval, bool argIstCheck)
    {
        if (argIstCheck) GPofThisNode.SetTXTtop("$" + argval.ToString() + "$");
        else
            GPofThisNode.SetTXTtop(argval.ToString());

    }
    public void setGP_bottext(string str) { GPofThisNode.SetTXTbot(str); }
    public void setGP_color(Color argc) { GPofThisNode.SetCOLOR(argc); }


    /// <summary>
    /// checks if ths node was part of the path to player1
    /// </summary>
    /// <param name="thisPoint"></param>
    /// <param name="segmentDistance"></param>
    /// <param name="previousNode"></param>
    /// <param name="cost"></param>
    public PathNode(GameObject thisPoint, float segmentDistance, PathNode previousNode, float argcost)
    {

        gridPointObj = thisPoint;
        GPofThisNode = gridPointObj.GetComponent<GridPoint>();
        this.segmentDistance = segmentDistance;
        this.previousNode = previousNode;


        if (GameSettings.Instance.applyCostTweek)
        {
            this.cost = argcost + (float)(GPofThisNode.TimesUsedInAfinalPath * 20);
        }
        else
            this.cost = argcost;

        setGP_toptext(cost, false);
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
        if (gridPointObj == targetPoint)
            return this;


        // if target has been found then start pruning
        if (targetFound)
        {
            // if bestCost is less than this node cost + distace to target then prune
            if (bestCost <= cost + Vector3.Distance(gridPointObj.transform.position, targetPoint.transform.position))
                return null;
        }

        // check all connected gridpoints and create PathNodes or Update existing ones as neccessary
        GridPoint point = gridPointObj.GetComponent<GridPoint>();
        if (point != null)
        {
            if (point.forwardGameObject != null)
                CheckNode(created, toVisit, point.forwardGameObject);

            if (point.leftGameObject != null)
                CheckNode(created, toVisit, point.leftGameObject);

            if (point.rightGameObject != null)
                CheckNode(created, toVisit, point.rightGameObject);

            if (point.backGameObject != null)
                CheckNode(created, toVisit, point.backGameObject);
        }


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
                setGP_toptext(newCost, true);
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
            if (p.gridPointObj == pointToCheck)
                return p;
        }

        return null;
    }

}
