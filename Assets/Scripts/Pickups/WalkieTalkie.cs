// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public class WalkieTalkie : MonoBehaviour {

    public UAudioManager audioManager { get; private set; }
    public AudioWatcher audioWatcher { get; private set; }

	void Awake()
    {
        audioManager = GetComponent<UAudioManager>();
        audioWatcher = GetComponent<AudioWatcher>();
    }

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
