using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictoPlacedObjects : MonoBehaviour {
    public static DictoPlacedObjects Instance = null;
    private void Awake()
    { 
        if (Instance == null)
        {
            
            InitVars();
            DontDestroyOnLoad(this.gameObject);
            Instance = this;

        }
        else
            Destroy(gameObject);
    }



    #region Things to init
    Dictionary<string, Transform> DIC;
    int _NumberOfPlacedTestBoxes;
    List<GameObject> testBoxes;
    List<string> testBoxIds;

    List<GameObject> thesePlaedObjects;
    void InitVars()
    {
        DIC = new Dictionary<string, Transform>();
        _NumberOfPlacedTestBoxes = 0;
        testBoxIds = new List<string>();
        testBoxes = new List<GameObject>();
        thesePlaedObjects = new List<GameObject>();
    }
    #endregion


    public void DICT_ReadAll() {
        foreach (KeyValuePair<string, Transform> kvp in DIC) {
            Debug.Log(" " + kvp.Key + " <-> " + kvp.Value.position.ToString() + " | name-" + kvp.Value.name);
        }
    }

    public void DICT_add(string argObjID, Transform argTran) {
        Debug.Log("trying to add " + argObjID + "---" + argTran.position.ToString()+ " to DIC");
        if (!DIC.ContainsKey(argObjID))
        {
            Transform t =   Transform.
            Debug.Log("add to DIC  " + argObjID);

            DIC.Add(argObjID, argTran);
        }
        else
        {
            Debug.Log("sorry " + argObjID + " already is in Dic");
        }
    }

    public Transform DICT_FindTrans(string argId) {
        //TODO:
        //fix this shit.. what do you mean if you cant find the key juust return this transfirm?
        Transform transtoget = this.transform;
        bool foundTrans = false; 
        foreach (KeyValuePair<string, Transform> kvp in DIC) {
            if (kvp.Key == argId) {
                foundTrans = true;
                Debug.Log("yay we found a transform for " + kvp.Key);
                transtoget = kvp.Value;
                break;
            }
        }
        if (foundTrans)
        {
            Debug.Log("we broke out of the loop cuz we found a transform ");
        }
        else {
            Debug.Log("SHIT we didnt find the transfom,, we ar using this.transf ");
        }
        return transtoget;
    }

    public void DICT_clear() {
        DIC.Clear();
    }











    //todo:
    //loook at this agaion
    public void Removing(PersistoNab pScript)
    {
        if (pScript.GetBaseName().Contains(GameSettings.Instance.GetAnchorName_TestBox()))
        {
            testBoxes.Remove(pScript.gameObject);
            thesePlaedObjects.Remove(pScript.gameObject);
        }
    }

    public void oksavingallplaced(ObjsToStore argObjstore) {
        Debug.Log("objects in thingsplaced" + thesePlaedObjects.Count);

        argObjstore.SaveAllTheseBAdBoysToStore(thesePlaedObjects);
    }

    public int GetNumOfPlacedTestboxes() { return _NumberOfPlacedTestBoxes; }
    public void PlacedATestbox(GameObject go) {
        _NumberOfPlacedTestBoxes++;
        testBoxes.Add(go);
        thesePlaedObjects.Add(go);
    }
}
