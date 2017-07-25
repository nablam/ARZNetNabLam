using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyMNGR : MonoBehaviour {

    public RoomLoader rmlrd;
    private void Awake()
    {
        ObjsFromStoreOBJ.SetbjcksManager(ObjsMngr);
    }

    private void Start()
    {
  

    }

    bool clickedServerdo = false;
    public void OkOKdoWillServ()
    {
        if (!clickedServerdo)
        {
            rmlrd.LoadMeshed();
            OkLoadALlALLOBBY();
            InitGridMap();
            clickedServerdo = true;
        }
        else {
            Debug.Log("already clicked !");
        }

    }


    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    #region Dependencies   
    public ObjsFromStore ObjsFromStoreOBJ;
    public ObjsManager ObjsMngr;

    public PathsManager _PathMANAGER;
    GridMap GridMapObj;
    #endregion



    bool lobbyloaded = false;
    public void OkLoadALlALLOBBY()
    {
        if (!lobbyloaded)
        {
            ObjsFromStoreOBJ.InitWorldAnchorStore(true);
            lobbyloaded = true;
        }
        else
            Debug.Log("Fuckoff ,,the btn was already pressed");
        
      

    }


    //public GameObject SpawnUnit1;

    //public GameObject SpawnUnit2;
    //public GameObject SpawnUnit3;
    //public GameObject SpawnUnit4;


    //public GameObject Zone_P1;
    //public GameObject Zone_P2;

    public GameObject genNode;
    GridMap map;
    List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> Zones = new List<GameObject>();

    PathNode _head;


    bool gridinited = false;
   public void InitGridMap()
    {
        if (!gridinited) {

            GameObject go = GameObject.FindGameObjectWithTag("GridMap");
            GridMapObj = go.GetComponent<GridMap>();

            GameObject goz1 = GameObject.FindGameObjectWithTag("ZoneOne");
            GameObject goz2 = GameObject.FindGameObjectWithTag("ZoneTwo");
            GameObject gotx = GameObject.FindGameObjectWithTag("Respawn");

            spawnPoints.Add(gotx);
            Zones.Add(goz1); Zones.Add(goz2);

            map = GridMapObj;
            map.CreateGrid();

            _PathMANAGER.Init_SP_DST(spawnPoints, Zones);
            _PathMANAGER.BuildDataModel(map, GameSettings.Instance.numberOfPAthsPerSpawnPoint);
            CLIIIIK();
            gridinited = true;
        }
        else
            Debug.Log("Fuckoff ,,the btn was already pressed");


    }

    public void CLIIIIK() {

        _PathMANAGER.GiveMeMyPathsForIamNetworkedSpawn(Zones[0], spawnPoints[0]);
        _PathMANAGER.GiveMeMyPathsForIamNetworkedSpawn(Zones[1], spawnPoints[0]);
 
    }
    

}
