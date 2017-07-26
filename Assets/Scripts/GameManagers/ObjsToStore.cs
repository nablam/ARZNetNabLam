using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;


public class ObjsToStore : MonoBehaviour
{


    public void SaveAllTheseBAdBoysToStore(List<GameObject> argObjsToStore) {

        Debug.Log("in obj to store and about to save lets read lets read");
       // DictoPlacedObjects.Instance.DICT_ReadAll();

        foreach (GameObject go in argObjsToStore)
        {
            go.AddComponent<PersistoNab>();
            go.GetComponent<PersistoNab>().SetAnchorStoreName(go.name);
        }
    
    }

}
