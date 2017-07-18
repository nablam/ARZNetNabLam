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
    List<TransData> LISTOFALL;
    int _NumberOfDiversThingsInWolrd;
    List<string> TEMPtestBoxIds;

    List<GameObject> TEMPthesePlaedObjects;
    void InitVars()
    {
        LISTOFALL = new List<TransData>();
        _NumberOfDiversThingsInWolrd = 0;
        TEMPtestBoxIds = new List<string>();
        TEMPthesePlaedObjects = new List<GameObject>();
    }
    #endregion


    public void DICT_ReadAll() {
        foreach (TransData td in LISTOFALL) {
            Debug.Log(" " + td.GetID() + " <-> " + td.Getpos().ToString());
        }
    }
    bool Alreadyin(string argid) {
        bool found = false;
        foreach (TransData td in LISTOFALL) {
            if (td.GetID() == argid) {
                found = true;
                break;
            }
        }
        return found;
    }

    public void DICT_add(string argObjID, Transform argTran) {


        Debug.Log("trying to add " + argObjID + "---" + argTran.position.ToString()+ " to DIC");
        if (!Alreadyin(argObjID))
        {

            TransData td = new TransData(argTran, argObjID);
            Debug.Log("add to DIC  " + argObjID);

            LISTOFALL.Add(td);
        }
        else
        {
            Debug.Log("sorry " + argObjID + " already is in Dic");
        }
    }

    public TransData DICT_FindTrans(string argId) {
        //TODO:
        //fix this shit.. what do you mean if you cant find the key juust return this transfirm?
        TransData transtoget = new TransData();

        bool found = false;
        foreach (TransData td in LISTOFALL)
        {
            if (td.GetID() == argId)
            {
                found = true;
                Debug.Log("yay we found a transform for " + argId);
                transtoget = td;
                break;
            }
        }
     
        if (found)
        {
            Debug.Log("we broke out of the loop cuz we found a transform ");
        }
        else {
            Debug.Log("SHIT we didnt find the transfom,, we ar using this.transf ");
        }
        return transtoget;
    }

    public void DICT_clear() {
        LISTOFALL.Clear();
    }











    //todo:
    //loook at this agaion
    public void Removing(PersistoNab pScript)
    {
        if (pScript.GetBaseName().Contains(GameSettings.Instance.GetAnchorName_TestBox()))
        {
            TEMPthesePlaedObjects.Remove(pScript.gameObject);
        }
    }

    public void oksavingallplaced(ObjsToStore argObjstore) {
        // Debug.Log("objects in thingsplaced" + TEMPthesePlaedObjects.Count);
        AddToListofTransData();
        argObjstore.SaveAllTheseBAdBoysToStore(TEMPthesePlaedObjects);
    }

    void AddToListofTransData() {
        foreach (GameObject go in TEMPthesePlaedObjects)
        {
           DICT_add(go.name, go.transform);      
        }
    }

    public int GetNumOfPlacedTestboxes() { return _NumberOfDiversThingsInWolrd; }
    public void AddPlacedGOToListOfPlacedGos(GameObject go) {
        _NumberOfDiversThingsInWolrd++;
        TEMPthesePlaedObjects.Add(go);
    }
}
