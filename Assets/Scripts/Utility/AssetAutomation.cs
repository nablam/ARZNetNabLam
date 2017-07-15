using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetAutomation : MonoBehaviour
{


    //ZombieAA_BoyDecay_
    //ZombieABv1_BoyFatUglyNakedGuy_
    //ZombieAC_A_BoyWhite_
    //ZombieAC_B_BoyBlack_
    //ZombieAD_A_GirlTall_
    //ZombieAEv1_GirlHair_
    //ZombieAEv2_GirlBald_

    //Hip_L
    //Hip_R

    //Knee_L
    //Ankle_L
    //Toes_L

    //Knee_R
    //Ankle_R
    //Toes_R

    //Spine1_M
    //Chest_M
    //Neck_M
    //Head_M

    //Shoulder_L
    //Elbow_L
    //Wrist_L

    //Shoulder_R
    //Elbow_R
    //Wrist_R

    public GameObject objToTest;
    public bool isStep2;

    string textFilePath = "ZombiePrefabs_indev/";
    List<string> prefabNames;
    List<string> Mids;
    List<string> Lefts;
    List<string> Rights;

    List<ColliderData> CollDataList;
    void Start()
    {
        InitLists();
        //clear file
        System.IO.File.WriteAllText(@"D:\_nabdir\AR_Z_main\research\FromUnity.txt", " ");

        if (isStep2)
        {
            Fase2BuildingLeftForAllInResources();

        }
        else
        {
            //places boxes 0.1 0.1 0.1 
            //on all prefabs in Resources/ZombiePrefabs_indev
            PlaceColliders_Right_Mid_Limbs();
        }

         
       

    }

    void Fase2BuildingLeftForAllInResources() {

        foreach (string s in prefabNames)
        {
            GameObject go = InstanntiateFromResources(s);
            if (go != null)
            {
                ReadLimbsColliderData(go.transform);
                //printColliderList();
                BuildLeft(go);
            }
            else
            {
                Debug.Log("Must place a prefab in public GO");
            }
        }   
    }
    void InitLists()
    {

        CollDataList = new List<ColliderData>();
        prefabNames = new List<string>();
        Lefts = new List<string>();
        Mids = new List<string>();
        Rights = new List<string>();
        prefabNames.Add("ZombieAA_BoyDecay");
        prefabNames.Add("ZombieABv1_BoyFatUglyNakedGuy");
        prefabNames.Add("ZombieABv2_miniboss1_tall");
        prefabNames.Add("ZombieABv2_miniboss2_short");
        prefabNames.Add("ZombieAC_A_BoyWhite");
        prefabNames.Add("ZombieAC_B_BoyBlack");
        prefabNames.Add("ZombieAD_A_GirlTall");
        prefabNames.Add("ZombieAEv1_GirlHair");
        prefabNames.Add("ZombieAEv2_GirlBald");

        Mids.Add("Spine1_M"); Mids.Add("Chest_M"); Mids.Add("Neck_M"); Mids.Add("Head_M"); Mids.Add("Head1_M");
        Lefts.Add("Hip_L"); Lefts.Add("Knee_L"); Lefts.Add("Ankle_L"); Lefts.Add("Shoulder_L"); Lefts.Add("Elbow_L"); Lefts.Add("Wrist_L");
        Rights.Add("Hip_R"); Rights.Add("Knee_R"); Rights.Add("Ankle_R"); Rights.Add("Shoulder_R"); Rights.Add("Elbow_R"); Rights.Add("Wrist_R");
        //16 total  
    }

    void PlaceColliders_Right_Mid_Limbs()
    {
        foreach (string s in prefabNames)
        {
            GameObject go = InstanntiateFromResources(s);
            AddCollidersToLimbList(go, Mids);
            AddCollidersToLimbList(go, Rights);
            //AddCollidersToLimbList(go, Lefts);
        }
    }

    GameObject InstanntiateFromResources(string argPrefabName)
    {
        GameObject aPrefab = Resources.Load(textFilePath + argPrefabName) as GameObject;
        //GameObject aPrefabOut= Instantiate(aPrefab) as GameObject;
        aPrefab.name = argPrefabName;
        return aPrefab;
    }


    void AddCollidersToLimbList(GameObject argPrefab, List<string> argList)
    {
        foreach (string tofind in argList)
        {
            if (argPrefab != null)
            {
                Transform bone = FindLimbByName(argPrefab.transform, tofind);
                if (bone != null)
                {
                    AddColliderToBone(bone);
                }

            }
        }
    }

    Transform FindLimbByName(Transform argTrans, string s)
    {
        if (string.Compare(argTrans.name, s) == 0)
        {
            return argTrans;
        }
        for (int childId = 0; childId < argTrans.childCount; childId++)
        {
            Transform result = FindLimbByName(argTrans.GetChild(childId), s);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }


    void AddColliderToBone(Transform argBone)
    {

        BoxCollider _bc = argBone.gameObject.GetComponent<BoxCollider>();
        if (_bc == null)
        {
            BoxCollider _bc2 = argBone.gameObject.AddComponent<BoxCollider>();
            _bc2.center = Vector3.zero;
            _bc2.size = new Vector3(0.1f, 0.1f, 0.1f);

            argBone.gameObject.layer = LayerMask.NameToLayer("Enemy");
            SetLimbColliderTag(ref _bc2);
        }

    }





    void DoWRITE(string argtxt)
    {

        System.IO.File.AppendAllText(@"D:\_nabdir\AR_Z_main\research\FromUnity.txt", argtxt);
    }


    void ReadLimbsColliderData(Transform argTrans)
    {
        if (argTrans == null)
        {
            return;
        }
        if (argTrans.gameObject.GetComponent<BoxCollider>() != null)
        {
            BoxCollider aBC = argTrans.gameObject.GetComponent<BoxCollider>();
            ColliderData cd = new ColliderData(aBC.center, aBC.size, aBC.gameObject.tag, aBC.gameObject.layer.ToString(), aBC.gameObject.name);
            CollDataList.Add(cd);
        }

        for (int childId = 0; childId < argTrans.childCount; childId++)
        {

            ReadLimbsColliderData(argTrans.GetChild(childId));
        }
    }

    void printColliderList()
    {

        foreach (ColliderData cd in CollDataList) { Debug.Log(cd.ToString()); }
    }

    void BuildLeft(GameObject argGO)
    {
        foreach (string s in Lefts)
        {
            string baseRightBoneName = s.TrimEnd('L') + "R";
            Debug.Log(baseRightBoneName);
            Transform theLeftBone = FindLimbByName(argGO.transform, s);
            ColliderData DataForRight = CollDataList.Find(x => x.BC_BoneName == baseRightBoneName);
            if (DataForRight != null)
            {
                AddColliderToBone_fromData(theLeftBone, DataForRight);
            }
            else
                Debug.Log("NO FOUND DATA");
        }
    }

    void AddColliderToBone_fromData(Transform argBone, ColliderData argcd)
    {
        argcd.FlipLeft();
        BoxCollider _bc = argBone.gameObject.GetComponent<BoxCollider>();
        if (_bc == null)
        {
            BoxCollider _bc2 = argBone.gameObject.AddComponent<BoxCollider>();
            _bc2.center = argcd.BC_center;
            _bc2.size = argcd.BC_size;

            argBone.gameObject.layer = LayerMask.NameToLayer("Enemy");
            argBone.gameObject.tag = argcd.BC_Tag;
            //SetLimbColliderTag(ref _bc2);
        }
        else
            Debug.Log("there is a collider on " + argBone.name + " already");

    }


    void SetLimbColliderTag(ref BoxCollider _bc2)
    {
        string argBoneName = _bc2.gameObject.name;
        string first3 = argBoneName.Substring(0, 3);
        Debug.Log("limb starts with " + first3);


        if (string.Compare(first3, "Hea") == 0)
        { _bc2.gameObject.tag = "ZombieHead"; }
        else
        if (string.Compare(first3, "Hip") == 0 ||
            string.Compare(first3, "Sho") == 0)
        { _bc2.gameObject.tag = "ZombieLimb"; }
        else
        if (string.Compare(first3, "Nec") == 0 ||
            string.Compare(first3, "Che") == 0 ||
            string.Compare(first3, "Spi") == 0)
        { _bc2.gameObject.tag = "ZombieTorso"; }
        else
            _bc2.gameObject.tag = "ZombieNonLethal";
    }

}


/*     void setlayer() {
       // gameObject.layer = LayerMask.NameToLayer("YourLayerName");
    }

    void settag()
    {
        //gameObject.tag = "Player";
    }
    
     */
