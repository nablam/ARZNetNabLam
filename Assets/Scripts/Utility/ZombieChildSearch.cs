using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChildSearch : MonoBehaviour {

	// Use this for initialization
	void Start () {
         

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    Transform DisableBoxColliders(Transform argTrans)
    {
        if (argTrans.gameObject.GetComponent<BoxCollider>())
            argTrans.gameObject.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform c in argTrans)
        {
            var result = DisableBoxColliders(c);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    Transform deepSearch(Transform parent, string val)
    {
        Debug.Log("on "+ parent.gameObject.name);

        foreach (Transform c in parent)
        {
            if (c.name == val) { return c; }
            var result = deepSearch(c, val);
            if (result != null)
                return result;

        }
      
        return null;
    }
}
