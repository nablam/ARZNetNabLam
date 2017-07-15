// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridPoint : MonoBehaviour {

    public GameObject gridPoint;
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public float segmentDistance = 0.5f;
    //public Material scannedMaterial;

    [HideInInspector]
    public GameObject forwardGameObject { get; private set; }
    public GameObject leftGameObject { get; private set; }
    public GameObject rightGameObject { get; private set; }
    public GameObject backGameObject { get; private set; }
    public bool Connected { get; private set; }

    public bool wasvisited { get;  set; }
    
    GridPoint forwardGridPoint;
    GridPoint leftGridPoint;
    GridPoint rightGridPoint;
    GridPoint backGridPoint;

    void Start()
    {
        Connected = false;
        wasvisited = false;
    }

    public void turnCubeMeshOff() {
        transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(2).gameObject.SetActive(false);
    }


    public void Connect()
    {
        Connected = true;
    }

    public bool isValid()
    {
        // if there is floor beneath then this is a valid point
        RaycastHit hitInfo;
        return Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3.0f, layerMask);
    }

    public void SetForwardPoint(GameObject forwardPoint)
    {
        if (forwardPoint == null)
        {
            forwardGameObject = null;
            forwardGridPoint = null;
            return;
        }

        forwardGameObject = forwardPoint;
        forwardGridPoint = forwardGameObject.GetComponent<GridPoint>();
    }

    public void SetLeftPoint(GameObject leftPoint)
    {
        if (leftPoint == null)
        {
            leftGameObject = null;
            leftGridPoint = null;
            return;
        }

        leftGameObject = leftPoint;
        leftGridPoint = leftGameObject.GetComponent<GridPoint>();
    }

    public void SetRightPoint(GameObject rightPoint)
    {
        if (rightPoint == null)
        {
            rightGameObject = null;
            rightGridPoint = null;
            return;
        }

        rightGameObject = rightPoint;
        rightGridPoint = rightGameObject.GetComponent<GridPoint>();
    }

    public void SetBackPoint(GameObject backPoint)
    {
        if (backPoint == null)
        {
            backGameObject = null;
            backGridPoint = null;
            return;
        }

        backGameObject = backPoint;
        backGridPoint = backGameObject.GetComponent<GridPoint>();
    }

    public void Scan(List<GameObject> pointsList, Queue<GameObject> toVisit)
    {
        /// <summary>
        /// when instantiated, perform raycasts to forward, left, right, and back
        /// if raycast hits spatial map point is set to null
        /// if raycast hits another gridpoint set respective property to that object and tell that object to do the same
        /// else instantiate a new gridpoint and add it to pointList
        /// </summary>

        RaycastHit hitInfo;

        // raycast forward
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo, segmentDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.gameObject.CompareTag("GridPoint"))
            {
                SetForwardPoint(hitInfo.collider.gameObject);
                forwardGridPoint.SetBackPoint(gameObject);
            }
            else
            {
                SetForwardPoint(null);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (Vector3.forward * segmentDistance);
            GameObject newGridPoint = Instantiate(gridPoint, newPosition, Quaternion.identity) as GameObject;
            GridPoint gp = newGridPoint.GetComponent<GridPoint>();
            if (gp.isValid() && gp.CheckBounds())
            {
                pointsList.Add(newGridPoint);
                toVisit.Enqueue(newGridPoint);
                SetForwardPoint(newGridPoint);
            }
            else
            {
                SetForwardPoint(null);
                Destroy(newGridPoint);
            }
        }

        // raycast right
        if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, segmentDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.gameObject.CompareTag("GridPoint"))
            {
                SetRightPoint(hitInfo.collider.gameObject);
                rightGridPoint.SetLeftPoint(gameObject);
            }
            else
            {
                SetRightPoint(null);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (Vector3.right * segmentDistance);
            GameObject newGridPoint = Instantiate(gridPoint, newPosition, Quaternion.identity) as GameObject;
            GridPoint gp = newGridPoint.GetComponent<GridPoint>();
            if (gp.isValid() && gp.CheckBounds())
            {
                pointsList.Add(newGridPoint);
                toVisit.Enqueue(newGridPoint);
                SetRightPoint(newGridPoint);
            }
            else
            {
                SetRightPoint(null);
                Destroy(newGridPoint);
            }
        }

        // raycast left
        if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, segmentDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.gameObject.CompareTag("GridPoint"))
            {
                SetLeftPoint(hitInfo.collider.gameObject);
                leftGridPoint.SetRightPoint(gameObject);
            }
            else
            {
                SetLeftPoint(null);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (Vector3.left * segmentDistance);
            GameObject newGridPoint = Instantiate(gridPoint, newPosition, Quaternion.identity) as GameObject;
            GridPoint gp = newGridPoint.GetComponent<GridPoint>();
            if (gp.isValid() && gp.CheckBounds())
            {
                pointsList.Add(newGridPoint);
                toVisit.Enqueue(newGridPoint);
                SetLeftPoint(newGridPoint);
            }
            else
            {
                SetLeftPoint(null);
                Destroy(newGridPoint);
            }
        }

        // raycast back
        if (Physics.Raycast(transform.position, Vector3.back, out hitInfo, segmentDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider.gameObject.CompareTag("GridPoint"))
            {
                SetBackPoint(hitInfo.collider.gameObject);
                backGridPoint.SetForwardPoint(gameObject);
            }
            else
            {
                SetBackPoint(null);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (Vector3.back * segmentDistance);
            GameObject newGridPoint = Instantiate(gridPoint, newPosition, Quaternion.identity) as GameObject;
            GridPoint gp = newGridPoint.GetComponent<GridPoint>();
            if (gp.isValid() && gp.CheckBounds())
            {
                pointsList.Add(newGridPoint);
                toVisit.Enqueue(newGridPoint);
                SetBackPoint(newGridPoint);
            }
            else
            {
                SetBackPoint(null);
                Destroy(newGridPoint);
            }
        }

        //gameObject.GetComponentInChildren<MeshRenderer>().material = scannedMaterial;
    }

    /// <summary>
    /// Checks the bounds of this grid point. Returns false if we are removing this point.
    /// </summary>
    /// <returns>
    public bool CheckBounds()
    {
        // if there is not enough room for an agent, then delete point
        Vector3 center = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 halfExtents = new Vector3(segmentDistance * 0.5f, 0.5f, segmentDistance * 0.5f);

        if (Physics.CheckBox(center, halfExtents, Quaternion.identity, layerMask, QueryTriggerInteraction.Ignore))
        {
            // RemovePoint();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks the connectivity of this grid point to the root node. Returns false if we are removing this point.
    /// </summary>
    /// <returns>
    public bool CheckConnectivity()
    {
        if (!Connected)
        {
            RemovePoint();
            return false;
        }

        return true;
    }

    void RemovePoint()
    {
        // Debug.Log("Removing Point");
        if (forwardGameObject != null)
            forwardGridPoint.SetBackPoint(null);

        if (leftGameObject != null)
            leftGridPoint.SetRightPoint(null);

        if (rightGameObject != null)
            rightGridPoint.SetLeftPoint(null);

        if (backGameObject != null)
            backGridPoint.SetForwardPoint(null);

        Destroy(gameObject);
    }
}
