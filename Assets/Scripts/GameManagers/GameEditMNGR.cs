using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class GameEditMNGR : MonoBehaviour {
    public static GameEditMNGR Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitVars();
        }
        else
            Destroy(gameObject);
    }
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    #region Dependencies   
    public GameSettings Settings;
    WorldAnchorStore anchorStore;
    #endregion

    //REMOVE FROM HERE 
    public GameObject TestBoxObj;
    //DONE list of things to gemove

    #region Things to init
    int _NumberOfPlacedTestBoxes;
    List<string> testBoxIds;
    List<GameObject> testBoxes;
    void InitVars()
    {
        _NumberOfPlacedTestBoxes = 0;
        testBoxIds = new List<string>();
        testBoxes = new List<GameObject>();
    }
    #endregion

    private void Start()
    {
        InitWorldAnchorStore();
    }

    public void PlaceTestBox() {
        Debug.Log("placing test box");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            _NumberOfPlacedTestBoxes++;
            string id = Settings.GetAnchorName_TestBox() + _NumberOfPlacedTestBoxes;
            testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(TestBoxObj, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

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

    public void InitWorldAnchorStore()
    {
        //only if room is loaded .. handle this with a state machine
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
        LoadObjects();
    }
    void AnchorStoreReadySave(WorldAnchorStore store)
    {
        // save instance
        anchorStore = store;

        // load saved anchor
        //if (!anchorStore.Load(anchorStoreName, trans.gameObject))
        //{
        //    // if no saved anchor then create one
        //    WorldAnchor attachingAnchor = trans.gameObject.AddComponent<WorldAnchor>();
        //    if (attachingAnchor.isLocated)
        //    {
        //        anchorStore.Save(anchorStoreName, attachingAnchor);
        //    }
        //    else
        //    {
        //        attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
        //    }
        //}
    }


    void LoadObjects() {
        string[] ids = anchorStore.GetAllIds();

        for (int index = 0; index < ids.Length; index++)
        {          
            if (ids[index].Contains(Settings.GetAnchorName_TestBox()))
            {
                // get id number
                int thisId = int.Parse(ids[index].Substring(Settings.GetAnchorName_TestBox().Length));

                // set spawn id to highest anchorIdNum
                if (thisId > _NumberOfPlacedTestBoxes)
                {
                    _NumberOfPlacedTestBoxes = thisId;
                }

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
            testBoxes.Add(InstantiateObject_OnHEAP(TestBoxObj, id));
        }

    }






    public void Removing(PersistoNab pScript)
    {      
       if (pScript.GetBaseName().Contains(Settings.GetAnchorName_TestBox()))
        {
            testBoxes.Remove(pScript.gameObject);
        } 
    }


}
