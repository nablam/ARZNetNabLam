
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathsManager : MonoBehaviour {
 
   
    //a quad mesh to debug paths
 
    public GameObject PF1node;

 
    #region Dependencies
    public PathMaker _PathMaker;
    #endregion

    //List of spawnpoints and playerzones initialized by GAmeMabager
    List<GameObject> SpawnTagObjects;
    List<GameObject> ZonesTagObjects;
    public void Init_SP_DST(List<GameObject> argspawns, List<GameObject> argDets) {
        SpawnTagObjects = argspawns;
        ZonesTagObjects = argDets;
    }
 

 

    void Start()
    {
        FullDataModel = new DataFull("MP");
    }

    DataFull FullDataModel;
    /// <summary>
    /// BuildDataModel() must be called from Gamemanager to to create a datamodel of all the paths to all the zones
    /// this "FullDataModel" will be called by NETWORKED Spawnpoints 
    /// </summary>
    /// <param name="argMap"></param>
    //to be called from GAmeMAnager to creat the entre datamap:
    // 
    //  FULL
    //     |\Dz1
    //     | |\path1
    //     | |
    //     |  \path2
    //     DZ2
    //      |\path1 ->pos1,pos2,pos3
    //      |
    //       \path2
    //
    //
    //
    public void BuildDataModel(GridMap argMap, int argPathsToMake) {

        foreach (GameObject ZoneObj in ZonesTagObjects)
        {
            DataZone Dz = new DataZone(ZoneObj.name);
            foreach (GameObject spObj in SpawnTagObjects)
            {
                for(int ptm=0; ptm < argPathsToMake; ptm++ )
                BuildDatapathForStartNode(argMap, spObj, ZoneObj, Dz);
        
            }
            FullDataModel.AddToZones(Dz);
        }
 
    }


    // a NETWORK spawnpoint will invike this askingg: given my name (comming from my anchorpoint name + number) 
    // what paths are available for zone named Zone1
    public void GiveMeMyPathsForIamNetworkedSpawn(GameObject ZoneGo, GameObject SpawnGO)
    {
        List<List<Vector3>> temp = getPAthsTOZone(ZoneGo.name, SpawnGO.name);
        foreach ( List<Vector3> templist in temp )
        ShowPath(GameSettings.Instance.GetNExtColor(), templist.ToList<Vector3>());
    }

    // a NETWORK spawnpoint will invike this askingg: given my name (comming from my anchorpoint name + number) 
    // what paths are available for zone named Zone1
    public List<List<Vector3>> GiveMeMyPathsForIamNetworkedSpawnNET(GameObject ZoneGo, GameObject SpawnGO)
    {
        List<List<Vector3>> temp = getPAthsTOZone(ZoneGo.name, SpawnGO.name);
        return temp;
    }

    void BuildDatapathForStartNode(GridMap argManp, GameObject GO_Spawn, GameObject ZoneObj,DataZone argDz)
    {
        _PathMaker.BuildPathsToPlayerarea(argManp, GO_Spawn,ZoneObj );
        DataPath dp = new global::DataPath(_PathMaker.GEtPath_V3(), GO_Spawn.name, ZoneObj.name);
        argDz.AddToPaths(dp);
    }



 
 

    List<List<Vector3>> getPAthsTOZone(string zonename, string spawnname) {
        List<List<Vector3>> temp = new List<List<Vector3>>();
        foreach (DataZone dz in FullDataModel.GetZones())
            foreach (DataPath dp in dz.GetPAths())
            {
               // Debug.Log(dp.NameOfStartObject);
                if (dp.NameOfStartObject == spawnname && dp.NameOfENDObject == zonename)
                    temp.Add(dp.GEtTheListOfV3());
            }
        return temp;
    }


    void ShowPath(Color argColor, List<Vector3> argPathV3)
    {


        for (int n = 0; n < argPathV3.Count; n++)
        {
            GameObject go_nP1 = Instantiate(PF1node, argPathV3[n], Quaternion.identity) as GameObject;
            go_nP1.GetComponentInChildren<TextMesh>().text = "-" + n.ToString() + "-";
            go_nP1.name = "p1n" + n.ToString();
            go_nP1.transform.parent = this.transform;
            go_nP1.GetComponentInChildren<Renderer>().material.color = argColor;//
            // Path_P1_Tags_OBJ.Add(go_nP1);
        }
    }
}
