using UnityEngine;
using System.Collections;

public class startPlaying_stopPlaying : MonoBehaviour 
{
	public Animator anim;
	public bool armed = false;
	
	void OnTriggerEnter(Collider other)
	{
		armed = true;
	}	
	
	void OnTriggerExit(Collider other)
	{
		armed = false;
	}

	void Update () 
	{
		if(armed && Input.GetKeyDown(KeyCode.Space))
		{
			StartPlaying ();
		}
	}

	void StartPlaying ()
	{
		anim.speed = 1f;
	}

	void StopPlaying ()
	{
		anim.speed = 0f;
	}
}
