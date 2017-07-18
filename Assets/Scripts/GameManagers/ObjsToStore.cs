using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;


public class ObjsToStore : MonoBehaviour
{


    public void SaveAllTheseBAdBoysToStore(List<GameObject> argObjsToStore) {

        Debug.Log("in obj to store and about to save lets read lets read");
        DictoPlacedObjects.Instance.DICT_ReadAll();

        foreach (GameObject go in argObjsToStore)
        {
           // DictoPlacedObjects.Instance.DICT_add(go.name, go.transform);
            go.AddComponent<PersistoNab>();
            go.GetComponent<PersistoNab>().SetAnchorStoreName(go.name);
        }
    
    }

















        void LoadObjects()
    {
        //string[] ids = anchorStore.GetAllIds();

        //for (int index = 0; index < ids.Length; index++)
        //{
        //    if (ids[index].Contains(Settings.GetAnchorName_TestBox()))
        //    {
        //        // get id number
        //        int thisId = int.Parse(ids[index].Substring(Settings.GetAnchorName_TestBox().Length));

        //        // set spawn id to highest anchorIdNum
        //        if (thisId > _NumberOfPlacedTestBoxes)
        //        {
        //            _NumberOfPlacedTestBoxes = thisId;
        //        }

        //        // add id to string list to instantiate later
        //        testBoxIds.Add(ids[index]);
        //    }
        //    //els if id is sosoos
        //    //  Debug.Log("storename is "+ anchorStore.N)

        //}
        //////ENDForloop
        //// load and instantiate all infinite ammo boxes
        //foreach (string id in testBoxIds)
        //{
        //    testBoxes.Add(InstantiateObject_OnHEAP(TestBoxObj, id));
        //}

    }

    //public void PlaceTestBox()
    //{
    //    Debug.Log("placing test box");

    //    if (GazeManager.Instance.isActiveAndEnabled)
    //    {
    //        _NumberOfPlacedTestBoxes++;
    //        string id = Settings.GetAnchorName_TestBox() + _NumberOfPlacedTestBoxes;
    //        // testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(TestBoxObj, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
    //    }
    //}


    /*
    GameObject InstantiateObject_toBePlacedInTheWorld(GameObject obj, string id, Vector3 position, Quaternion rotation, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj, position, rotation) as GameObject;
        PersistoNab pscript = o.GetComponent<PersistoNab>();
        pscript.SetAnchorStoreName(id);
        //pscript.SetRotateOnNormals(rotateOnNormals);
        //pscript.KeepUpright(keepUpright);
        return o;
    }

    GameObject InstantiateObject_OnHEAP(GameObject obj, string id)
    {
        GameObject o = Instantiate(obj) as GameObject;
        PersistoNab pscript = o.GetComponent<PersistoNab>();
        pscript.SetAnchorStoreName(id);
        return o;
    }

 
    */

}
