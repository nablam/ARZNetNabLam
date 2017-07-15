// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

    [Tooltip("Layer that GridPoints exist on.")]
    public LayerMask gridPointLayer = Physics.DefaultRaycastLayers;

    [Tooltip("How long in seconds does it take for player to be ready to take another hit.")]
    public float recoverTime = 3f;

    [HideInInspector]
    public GameObject gridPosition { get; private set; }

    //TimerBehaviour t;
    //float checkTime = 4.0f;

    // Use this for initialization
    void Start()
    {
        //t = gameObject.AddComponent<TimerBehaviour>();
        //t.SetDestroyOnComplete(false);
    }
	
    public void SetGridPosition()
    {
        GridMap gm = GameObject.FindObjectOfType<GridMap>();
        gridPosition = gm.GetClosestPoint(gameObject);
    }

    public void SetGridPosition(GameObject point)
    {
        gridPosition = point;
    }

    // Update is called once per frame
    void Update ()
    {
        //RaycastHit hitInfo;
        //if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3.0f, gridPointLayer, QueryTriggerInteraction.Collide))
        //{
        //    gridPosition = hitInfo.collider.gameObject;
        //    //t.StartTimer(checkTime, SetGridPosition);  <-- causing a slight frame rate drop every time it is called
        //}
	}
}
