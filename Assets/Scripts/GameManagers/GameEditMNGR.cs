using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class GameEditMNGR : MonoBehaviour {
   // public static GameEditMNGR Instance = null;
    private void Awake()
    {
        ObjsFromStoreOBJ.SetbjcksManager(ObjsMngr);

    }
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    #region Dependencies   
    // public GameSettings Settings;
    public ObjsToStore ObjsToStoreOBJ;
    public ObjsFromStore ObjsFromStoreOBJ;
    public ObjsManager ObjsMngr;
    //ObjsManager.Instance is global just for testing
    #endregion

    //REMOVE FROM HERE 
    public GameObject TestBoxObj;
    //DONE list of things to gemove



 
    public void PlaceTestBox() {
        Debug.Log("placing test box");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(TestBoxObj, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;
           
            string thename = GameSettings.Instance.GetAnchorName_TestBox() +  DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.PlacedATestbox(go);
            Debug.Log("placing " + thename);
            //   _NumberOfPlacedTestBoxes++;
            //  string id = Settings.GetAnchorName_TestBox() + _NumberOfPlacedTestBoxes;
            // testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(TestBoxObj, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));

        }
    }

    

    public void OKSAveAll( ) {
        DictoPlacedObjects.Instance.oksavingallplaced(ObjsToStoreOBJ);
    }

    public void OkLoadALl() {
        ObjsFromStoreOBJ.InitWorldAnchorStore();
    }














}
