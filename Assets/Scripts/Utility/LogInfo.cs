// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogInfo : MonoBehaviour {

    public Text log;

	// Use this for initialization
	void Start () {
        log = gameObject.GetComponent<Text>();
	}


    public void UpdateLog(string value)
    {
        log.text += value + "\n";
    }

	// Update is called once per frame
	void Update () {
	
	}
}
