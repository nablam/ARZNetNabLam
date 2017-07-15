// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioWatcher : MonoBehaviour {

    public bool canPlay { get; private set; }
    UAudioManager audioManager;
    Queue<string> eventQueue = new Queue<string>();
        
	// Use this for initialization
	void Start () {
        canPlay = true;
        audioManager = GetComponent<UAudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canPlay && eventQueue.Count > 0)
        {
            string e = eventQueue.Dequeue();
            PlayEvent(e);
        }
	}

    public void Playing()
    {
        canPlay = false;
    }

    public void Done()
    {
        canPlay = true;
    }

    public void PlayEvent(string eventName)
    {
        if (audioManager == null)
            audioManager = GetComponent<UAudioManager>();

        if (canPlay)
        {
            audioManager.PlayEvent(eventName, gameObject, "Done");
        } else
        {
            eventQueue.Enqueue(eventName);
        }
    }
}
