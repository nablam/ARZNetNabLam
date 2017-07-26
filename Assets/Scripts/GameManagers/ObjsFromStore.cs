using HoloToolkit.Examples.SharingWithUNET;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class ObjsFromStore : MonoBehaviour
{
    WorldAnchorStore anchorStore;
    ObjsManager _objsMngr;
    // Use this for initialization
    bool _lobbyMode;
    private void Start()
    {
        _lobbyMode = false;
        //Debug.Log("starting  OBJFRO and why not read lets read");
       // DictoPlacedObjects.Instance.DICT_ReadAll();
        //  InitWorldAnchorStore();
    }

    public void SetbjcksManager(ObjsManager argObjsMngr) {

        _objsMngr = argObjsMngr;
    }
    public void InitWorldAnchorStore(bool argLobbymode)
    {
        _lobbyMode = argLobbymode;
        //Debug.Log("INNIT OBJFROM WA STORE");
       // DictoPlacedObjects.Instance.DICT_ReadAll();
        //only if room is loaded .. handle this with a state machine
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        AllIds = new List<string>();
        testBoxes = new List<GameObject>();
        anchorStore = store;
        LoadObjects(_objsMngr);
    }

    List<string> AllIds;
    List<GameObject> testBoxes;

    void LoadObjects(ObjsManager argObjsMngr)
    {
        string[] ids = anchorStore.GetAllIds();

        for (int index = 0; index < ids.Length; index++)
        {
            if (ids[index].Contains(GameSettings.Instance.GetAnchorName_TestBox()))
            {
                //Debug.Log("we contain " + GameSettings.Instance.GetAnchorName_TestBox());
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameSettings.Instance.GetAnchorName_TestBox().Length));
                //Debug.Log(" ID INT " + thisId);

                // add id to string list to instantiate later
                 AllIds.Add(ids[index]);
            }
            else
            if (ids[index].Contains(GameSettings.Instance.GetAnchorName_GridMap()))
            {
                //Debug.Log("we contain " + GameSettings.Instance.GetAnchorName_GridMap());
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameSettings.Instance.GetAnchorName_GridMap().Length));
                //Debug.Log(" ID INT " + thisId);

                // add id to string list to instantiate later
                AllIds.Add(ids[index]);
            }
            else
            if (ids[index].Contains(GameSettings.Instance.GetAnchorName_ZoneOne()))
            {
                //Debug.Log("we contain " + GameSettings.Instance.GetAnchorName_ZoneOne());
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameSettings.Instance.GetAnchorName_ZoneOne().Length));
                //Debug.Log(" ID INT " + thisId);

                // add id to string list to instantiate later
                AllIds.Add(ids[index]);
            }

            else
            if (ids[index].Contains(GameSettings.Instance.GetAnchorName_ZoneTwo()))
            {
               // //Debug.Log("we contain " + GameSettings.Instance.GetAnchorName_ZoneTwo());
                // get id number
                int thisId = int.Parse(ids[index].Substring(GameSettings.Instance.GetAnchorName_ZoneTwo().Length));
               // //Debug.Log(" ID INT " + thisId);

                // add id to string list to instantiate later
                AllIds.Add(ids[index]);
            }
            //els if id is sosoos
            //  //Debug.Log("storename is "+ anchorStore.N)

        }
        ////ENDForloop
        // load and instantiate all infinite ammo boxes
        if (!_lobbyMode)
        {
            foreach (string id in AllIds)
            {
                TransData TToGet = DictoPlacedObjects.Instance.DICT_FindTrans(id);
                //Debug.Log("getting t from gameeditor list   " + TToGet.GetID());
                testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(argObjsMngr.GettheRightObjectFromAfullid(id), id, TToGet.Getpos(), TToGet.GetRot()));
            }
        }
        else {
            //TODO here get the non placeholders from Objectsmanager
            foreach (string id in AllIds)
            {
                TransData TToGet = DictoPlacedObjects.Instance.DICT_FindTrans(id);
                //Debug.Log("getting t from gameeditor list   " + TToGet.GetID());
                testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(argObjsMngr.GettheRightREALObject_For_LOBBY(id), id, TToGet.Getpos(), TToGet.GetRot()));
            }
        }

    }
    /// <summary>
    /// The transform of the shared world anchor.
    /// </summary>
    private Transform sharedWorldAnchorTransform;

    GameObject InstantiateObject_toBePlacedInTheWorld(GameObject obj, string id, Vector3 position, Quaternion rotation)
    {
        GameObject o = Instantiate(obj, position, rotation) as GameObject;
        o.name = id;
        PersistoNab pscript = o.GetComponent<PersistoNab>();
        pscript.SetAnchorStoreName(id);

        if (!id.Contains("GridMap"))
        {
            sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
            o.transform.SetParent(sharedWorldAnchorTransform);
        }

        return o;
    }

 
 
}
