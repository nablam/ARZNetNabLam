using UnityEngine;
using System.Collections;

public class effectPlayVolume : MonoBehaviour 
{
	public ParticleSystem effectToPlay;
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
			if(effectToPlay.isPlaying == true)
			{
				effectToPlay.Clear();
				effectToPlay.Play();
			}
			else
			{
				effectToPlay.Play();
			}
		}
	}
}
