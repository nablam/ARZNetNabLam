// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class SpecialAmmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Take()
    {
        gameObject.SetActive(false);
    }
}
