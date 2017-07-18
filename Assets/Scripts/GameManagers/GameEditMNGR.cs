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
    public ObjsToStore ObjsToStore;
    //ObjsManager.Instance is global just for testing
    #endregion

    //REMOVE FROM HERE 
    public GameObject TestBoxObj;
    //DONE list of things to gemove

    #region Things to init
    int _NumberOfPlacedTestBoxes;
    List<string> testBoxIds;
    List<GameObject> testBoxes;
    List<GameObject> thingsplaced;
    void InitVars()
    {
        _NumberOfPlacedTestBoxes = 0;
        testBoxIds = new List<string>();
        testBoxes = new List<GameObject>();
        thingsplaced = new List<GameObject>();
    }
    #endregion

 
    public void PlaceTestBox() {
        Debug.Log("placing test box");

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            GameObject go = Instantiate(TestBoxObj, GazeManager.Instance.HitInfo.point, Quaternion.identity) as GameObject;
            string thename = Settings.GetAnchorName_TestBox() + testBoxes.Count.ToString();
            go.name = thename;
            testBoxes.Add(go);
            thingsplaced.Add(go);
            //   _NumberOfPlacedTestBoxes++;
            //  string id = Settings.GetAnchorName_TestBox() + _NumberOfPlacedTestBoxes;
            // testBoxes.Add(InstantiateObject_toBePlacedInTheWorld(TestBoxObj, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));

        }
    }

    public void OKSAveAll() {

        ObjsToStore.SaveAllTheseBAdBoysToStore(thingsplaced);
    }

    public void OkLoadALl() {
        ObjsToStore.InitWorldAnchorStore();
    }











    public void Removing(PersistoNab pScript)
    {      
       if (pScript.GetBaseName().Contains(Settings.GetAnchorName_TestBox()))
        {
            testBoxes.Remove(pScript.gameObject);
        } 
    }


}
