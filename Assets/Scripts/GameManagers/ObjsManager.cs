using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjsManager : MonoBehaviour {
    public GameObject TestBoxObj;

    public GameObject PlaceHolder_TestBox;
    public GameObject PlaceHolder_GridMap;
    public GameObject PlaceHolder_ZoneOne;
    public GameObject PlaceHolder_ZoneTwo;

    public GameObject RealObj_TestBox;
    public GameObject RealObj_GridMap;
    public GameObject RealObj_ZoneOne;
    public GameObject RealObj_ZoneTwo;

    public List<GameObject> PlaceHolders;

    public List<GameObject> RealObjs;

    private void Awake()
    {
        PlaceHolders = new List<GameObject>();
        PlaceHolders.Add(PlaceHolder_TestBox);
        PlaceHolders.Add(PlaceHolder_GridMap);
        PlaceHolders.Add(PlaceHolder_ZoneOne);
        PlaceHolders.Add(PlaceHolder_ZoneTwo);

        RealObjs = new List<GameObject>();
        RealObjs.Add(RealObj_TestBox);
        RealObjs.Add(RealObj_GridMap);
        RealObjs.Add(RealObj_ZoneOne);
        RealObjs.Add(RealObj_ZoneTwo);
    }


    public GameObject GettheRightObjectFromAfullid(string fullid2) {

        if (fullid2.Contains(GameSettings.Instance.GetAnchorName_TestBox())) { return PlaceHolder_TestBox; }
        else
                if (fullid2.Contains(GameSettings.Instance.GetAnchorName_GridMap())) { return PlaceHolder_GridMap; }
        else

            if (fullid2.Contains(GameSettings.Instance.GetAnchorName_ZoneOne())) { return PlaceHolder_ZoneOne; }
        else

            if (fullid2.Contains(GameSettings.Instance.GetAnchorName_ZoneTwo())) { return PlaceHolder_ZoneTwo; }

        else
            return null;


    }


    public GameObject GettheRightREALObject_For_LOBBY(string fullid2)
    {

        if (fullid2.Contains(GameSettings.Instance.GetAnchorName_TestBox())) { return RealObj_TestBox; }
        else
                if (fullid2.Contains(GameSettings.Instance.GetAnchorName_GridMap())) { return RealObj_GridMap; }
        else

            if (fullid2.Contains(GameSettings.Instance.GetAnchorName_ZoneOne())) { return RealObj_ZoneOne; }
        else

            if (fullid2.Contains(GameSettings.Instance.GetAnchorName_ZoneTwo())) { return RealObj_ZoneTwo; }

        else
            return null;


    }


}
