using UnityEngine;
using System.Collections;

public class effectSpawnVolume : MonoBehaviour 
{
	public Transform effectToSpawn;
	public Vector3 spawnPosition = Vector3.zero;
	public Vector3 spawnRotation = Vector3.zero;
	public bool armed = false;


	void OnTriggerEnter(Collider other)
	{
		armed = true;
	}	

	void OnTriggerExit(Collider other)
	{
		armed = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if(armed && Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate (effectToSpawn, transform.position + spawnPosition, Quaternion.Euler(spawnRotation));
		}
	}
}
