using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBuilder : MonoBehaviour {

    public GameObject ZombieABv2;

    void Start () {
        FindLimb(ZombieABv2.transform, "Hip_L");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Transform FindLimb(Transform argTrans, string s)
    {
        if (string.Compare(argTrans.name, "Hip_L") == 0) {
         //   Debug.Log("found" + s);
        }
        for (int childId = 0; childId < argTrans.childCount; childId++) {
            Transform result = FindLimb(argTrans.GetChild(childId), s);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

}
