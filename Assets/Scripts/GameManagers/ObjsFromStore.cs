using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class ObjsFromStore : MonoBehaviour
{
    WorldAnchorStore anchorStore;

    // Use this for initialization
    private void Start()
    {
        Debug.Log("starting  OBJFRO and why not read lets read");
       // DictoPlacedObjects.Instance.DICT_ReadAll();
        //  InitWorldAnchorStore();
    }

    public void InitWorldAnchorStore()
    {
        Debug.Log("INNIT OBJFROM WA STORE");
        DictoPlacedObjects.Instance.DICT_ReadAll();
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
            if (ids[index].Contains(GameSettings.Instance.GetAnchorName_TestBox()))
            {
                Debug.Log("we contain " + GameSettings.Instance.GetAnchorName_TestBox());
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameSettings.Instance.GetAnchorName_TestBox().Length));
                Debug.Log(" ID INT " + thisId);

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
            Transform TToGet =DictoPlacedObjects.Instance.DICT_FindTrans(id);
            Debug.Log("getting t from gameeditor list   " + TToGet.name);
            testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(ObjsManager.Instance.PlaceHolder_TestBox, id, TToGet.position, TToGet.rotation));
        }

    }


    GameObject InstantiateObject_toBePlacedInTheWorld(GameObject obj, string id, Vector3 position, Quaternion rotation, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj, position, rotation) as GameObject;
        o.name = id;
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
