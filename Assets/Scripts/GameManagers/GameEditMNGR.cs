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

    public void OKSAveAll( ) {
        DictoPlacedObjects.Instance.oksavingallplaced(ObjsToStoreOBJ);
    }

    public void OkLoadALl() {
        ObjsFromStoreOBJ.InitWorldAnchorStore(false);
    }

    public void OkLoadALlALLOBBY()
    {
        ObjsFromStoreOBJ.InitWorldAnchorStore(true);
    }






    public void PlaceTestBox()
    {
        Debug.Log("placing test box");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(ObjsMngr.PlaceHolder_TestBox, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;

            string thename = GameSettings.Instance.GetAnchorName_TestBox() + DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.PlacedATestbox(go);
            Debug.Log("placing " + thename);
        }
    }



    public void PlaceGridMap()
    {
        Debug.Log("placing GridMap");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(ObjsMngr.PlaceHolder_GridMap, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;

            string thename = GameSettings.Instance.GetAnchorName_GridMap() + DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.PlacedATestbox(go);
            Debug.Log("placing " + thename);
        }
    }


    public void PlaceZone1()
    {
        Debug.Log("placing GridMap");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(ObjsMngr.PlaceHolder_ZoneOne, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;

            string thename = GameSettings.Instance.GetAnchorName_ZoneOne() + DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.PlacedATestbox(go);
            Debug.Log("placing " + thename);
        }
    }

    public void PlaceZone2()
    {
        Debug.Log("placing GridMap");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(ObjsMngr.PlaceHolder_ZoneTwo, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;

            string thename = GameSettings.Instance.GetAnchorName_ZoneTwo() + DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.PlacedATestbox(go);
            Debug.Log("placing " + thename);
        }
    }









}
