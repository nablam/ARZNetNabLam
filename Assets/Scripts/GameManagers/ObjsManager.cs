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

    public List<GameObject> PlaceHolders;

    private void Awake()
    {
        PlaceHolders = new List<GameObject>();
        PlaceHolders.Add(PlaceHolder_TestBox);
        PlaceHolders.Add(PlaceHolder_GridMap);
        PlaceHolders.Add(PlaceHolder_ZoneOne);
        PlaceHolders.Add(PlaceHolder_ZoneTwo);

    }

}
