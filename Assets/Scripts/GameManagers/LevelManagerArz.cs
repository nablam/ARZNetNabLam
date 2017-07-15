// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public class LevelManagerArz : MonoBehaviour {

    
    void Awake()
    {
        
    }

    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
	}


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
