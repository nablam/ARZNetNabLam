// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class GridCollider : MonoBehaviour {

    /*
     * THIS CLASS HAS BEEN DEPRECATED FOR SINGLE PLAYER
     */ 

    SphereCollider col;
    GridMap gm;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindObjectOfType<GridMap>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gm == null)
            gm = GameObject.FindObjectOfType<GridMap>();
        else
            rb.MovePosition(new Vector3(Camera.main.transform.position.x, gm.gridHeight, Camera.main.transform.position.z));
	}


    void Activate()
    {
        col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GridPoint"))
        {
            Camera.main.gameObject.SendMessage("SetGridPosition", other.gameObject);
        }
    }
}
