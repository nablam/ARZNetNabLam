using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class ObjsFromStore : MonoBehaviour {

    WorldAnchorStore anchorStore;

    // Use this for initialization
    private void Start()
    {
      //  InitWorldAnchorStore();
    }

    public void InitWorldAnchorStore()
    {
        //only if room is loaded .. handle this with a state machine
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        testBoxIds = new List<string>();
        testBoxes = new List<GameObject>();
        anchorStore = store;
        LoadObjects();
    }

    List<string> testBoxIds;
    List<GameObject> testBoxes;

    void LoadObjects()
    {
        string[] ids = anchorStore.GetAllIds();

        for (int index = 0; index < ids.Length; index++)
        {
            if (ids[index].Contains(GameEditMNGR.Instance.Settings.GetAnchorName_TestBox()))
            {
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameEditMNGR.Instance.Settings.GetAnchorName_TestBox().Length));

                // add id to string list to instantiate later
                testBoxIds.Add(ids[index]);
            }
            //els if id is sosoos
            //  Debug.Log("storename is "+ anchorStore.N)

        }
        ////ENDForloop
        // load and instantiate all infinite ammo boxes
        foreach (string id in testBoxIds)
        {
            testBoxes.Add(InstantiateObject_OnHEAP(ObjsManager.Instance.PlaceHolder_TestBox, id));
        }

    }

    GameObject InstantiateObject_OnHEAP(GameObject obj, string id)
    {
        GameObject o = Instantiate(obj) as GameObject;
        PersistoNab pscript = o.GetComponent<PersistoNab>();
        pscript.SetAnchorStoreName(id);
        return o;
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

    
 
    */

}
