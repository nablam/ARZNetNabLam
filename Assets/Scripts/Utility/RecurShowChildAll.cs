using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecurShowChildAll : MonoBehaviour, IFocusable, IInputClickHandler
{
    public void OnFocusEnter()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public void OnFocusExit()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }


    void toggledrawclear()
    {
        if (linesDrawn)
        {
            RemoveAllLines();
        }
        else
        {
            MapAllChildren();
        }
    }
    public void OnInputClicked(InputClickedEventData eventData)
    {
        toggledrawclear();
    }

    GameObject[] AllgosInScnene;
    List<GameObject> SomegosInScene;
    GameObject[] SomeGosIOnScneFiltered;
  LineRenderer line;
    int i;
    bool linesDrawn;
    private void Start()
    {
        linesDrawn = false;       
        LineObjects = new List<GameObject>();
        SomegosInScene = new List<GameObject>();

    }

    void GetObjectsWeNeed()
    {
       
        var exceptions = new string[] { "GridMap", "NodeTag", "SpatialMesh" };
       

        AllgosInScnene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        int allcount = AllgosInScnene.Length;

        SomeGosIOnScneFiltered   = AllgosInScnene.Where(x => !exceptions.Contains(x.tag)).ToArray<GameObject>();
        int cnt = 0;
        foreach (GameObject g in SomeGosIOnScneFiltered) {
            cnt++;
        }
        Debug.Log("used to have " + allcount + " after filter ->" + cnt);
    }

    public void MapAllChildren() {
        if (!linesDrawn)
        {
          //  AllgosInScnene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            GetObjectsWeNeed();
           
            foreach (GameObject g in SomeGosIOnScneFiltered)
            {
                ShowChildrenRecur(g.transform);
            }
            linesDrawn = true;
        }
    }

    void RemoveAllLines()
    {


        for (int x = LineObjects.Count - 1; x >= 0; x--)
        {
            GameObject tokill = LineObjects[x];
            LineObjects.RemoveAt(x);
             Destroy(tokill);
        }
        LineObjects = new List<GameObject>();
        linesDrawn = false;
    }

    public List<GameObject> LineObjects;
    

    void LineFromAtoB(Transform A, Transform B) {
        i++;
        LineRenderer line = new GameObject("Line " + i.ToString()).AddComponent<LineRenderer>();
        LineObjects.Add(line.gameObject);
        line.SetWidth(0.025F, 0.025F);
        line.SetColors(Color.red, Color.green);
        line.SetVertexCount(2);
        line.SetPosition(0, A.position);
        line.SetPosition(1, B.position);
    }


    Transform ShowChildrenRecur(Transform parent)
    {
        Debug.Log("on " + parent.gameObject.name);
        if (parent.childCount == 0) return parent;

        foreach (Transform c in parent)
        {
            Transform result = ShowChildrenRecur(c);

            if (result != null) { LineFromAtoB(result, parent); }
        }
        return parent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { toggledrawclear(); };

    }

}
