using UnityEngine;
using System.Collections;

public class DemoMusicPlayer : MonoBehaviour {

    public AudioSource musicAudioSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayMusic()
    {
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}
