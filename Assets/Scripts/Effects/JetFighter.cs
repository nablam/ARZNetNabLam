// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public class JetFighter : MonoBehaviour {

    Rigidbody rb;
    UAudioManager audioManager;
    bool startMoving = false;
    float moveSpeed;
    float height = 3.0f;

	// Use this for initialization
	void Start () {   
	}
	
	// Update is called once per frame
	void Update () {
        if (startMoving)
        {
            rb.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime * 4.0f));
        }

    }

    public void StartMoving(Vector3 targetPosition, float distance, float timeFlying)
    {
        rb = GetComponent<Rigidbody>();
        audioManager = GetComponent<UAudioManager>();
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(timeFlying, DoneFlying);
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        transform.position += Vector3.up * height;
        transform.position += transform.forward * -10.0f;
        distance = distance + 20.0f;
        moveSpeed = distance / timeFlying;
        audioManager.PlayEvent("_FlyBy");
        startMoving = true;
    }

    public void DoneFlying()
    {
        Destroy(gameObject);
    }
}
