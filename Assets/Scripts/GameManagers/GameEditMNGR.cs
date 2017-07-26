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

    private void Start()
    {
        rl.LoadMeshed();
    }
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    #region Dependencies   
    // public GameSettings Settings;
    public ObjsToStore ObjsToStoreOBJ;
    public ObjsFromStore ObjsFromStoreOBJ;
    public ObjsManager ObjsMngr;
    public RoomLoader rl;
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

    public void PlaceEditorObject(string argObjName) {

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(ObjsMngr.GettheRightObjectFromAfullid(argObjName), GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;

            string thename = argObjName + DictoPlacedObjects.Instance.GetNumOfPlacedTestboxes().ToString();
            go.name = thename;

            DictoPlacedObjects.Instance.AddPlacedGOToListOfPlacedGos(go);
            //Debug.Log("placing " + thename);
        }
    }



 






}
