using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemOrientation : MonoBehaviour {

    public GameObject[] lefthand;
    public GameObject[] rightHand;

	// Use this for initialization
	private void Awake () {
	    if (PlayerPrefs.HasKey("hand"))
        {
            if (PlayerPrefs.GetInt("hand") == 1)
            {
                foreach(GameObject o in rightHand)
                {
                    o.SetActive(false);
                }
                foreach(GameObject o in lefthand)
                {
                    o.SetActive(true);
                }
                return;
            }
        }

        // else
        foreach (GameObject o in rightHand)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in lefthand)
        {
            o.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
