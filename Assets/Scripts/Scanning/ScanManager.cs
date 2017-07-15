// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public class ScanManager : MonoBehaviour {

    RoomSaver roomSaver;

	// Use this for initialization
	void Start () {
        roomSaver = gameObject.GetComponent<RoomSaver>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SaveRoom()
    {
        if (roomSaver != null)
        {
            Debug.Log("Saved in scan manager");
            roomSaver.SaveRoom();
        }
          
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
