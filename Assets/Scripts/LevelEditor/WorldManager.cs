// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

public class WorldManager : MonoBehaviour
{
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public GameSettings Settings;
    public Material wireframeMaterial;
    public Material occlusionMaterial;

    public GameObject consoleObject;
    string AnchorName_ConsoleObject;

    public GameObject stemBase;
    string AnchorName_StemBase;

    public GameObject spawnPoint;
    string AnchorName_SpawnPoint;

    public GameObject dummySpawnPoint;
    string AnchorName_SpawnPointDummy;

    public GameObject barrier;
    string AnchorName_Barrier;

    public GameObject scoreboard;
    string AnchorName_ScoreBoard;

    public GameObject weaponsRack;
    string AnchorName_WeaponRack;

    public GameObject mag;
    string AnchorName_PistoleMag;

    public GameObject ammoBox;
    string AnchorName_AmmoBox;

    public GameObject infiniteAmmoBox;
    string AnchorName_AmmoBoxInfinite;

    public GameObject pathFinder;
    string AnchorName_PathFinder;

    public GameObject walkieTalkie;
    string AnchorName_WalkieTalkie;

    public GameObject mist;
    string AnchorName_MistEmitter;

    public GameObject mistEnd;
    string AnchorName_MistEnd;

    public GameObject hotspot;
    string AnchorName_HotSpot;

    public GameObject airstrikeStart;
    public GameObject airstrikeEnd;
    string AnchorName_AirStrikeStart;
    string AnchorName_AirStrikeEnd;

    List<GameObject> spawns;
    List<GameObject> dummySpawns;
    List<GameObject> barriers;
    List<GameObject> mags;
    List<GameObject> ammoBoxes;
    List<GameObject> infiniteAmmoBoxes;
    List<GameObject> walkieTalkies;
    List<GameObject> mists;
    List<GameObject> hotspots;

    bool drawWireframe = true;
    bool scoreboardPlaced = false;          // only one scoreboard is allowed
    bool weaponsRackPlaced = false;         // only one weapons rack is allowed
    bool pathFinderPlaced = false;          // only one pathfinder is allowed
    bool airstrikeStartPlaced = false;      // only one airstrike start position is allowed
    bool airstrikeEndPlaced = false;        // only one airstrike end position is allowed
    bool mistEndPlaced = false;             // only one mist end position is allowed
    bool stemBasePlaced = false;            // only one stem base allowed
    int magIdNum = 0;                       // id num associated with the number of mags in the room
    int ammoBoxIdNum = 0;                   // id num associated with the number of ammo boxes in the room
    int infiniteAmmoBoxIdNum = 0;           // id num associated with the number of infinite ammo boxes in the room
    int walkieTalkieIdNum = 0;              // id num associated with the number of walkie talkies in the room
    int mistIdNum = 0;                      // id num associated with the number of mist objects in the room
    int hotspotIdNum = 0;                   // id num associated with the number of hotspots in the room
    int spawnIdNum = 0;                     // id num associated with the number of spawn points in the room
    int dummySpawnIdNum = 0;                // id num associated with the number of dummy spawn points in the room
    int barrierIdNum = 0;                   // id num associated with the number of barriers in the room

    WorldAnchorStore anchorStore;
    bool roomLoaded = false;
    bool calledToAnchorStore = false;

    void InitAnchorNameVariables() {

        AnchorName_ConsoleObject = Settings.GetAnchorName_ConsoleObject();

        AnchorName_StemBase = Settings.GetAnchorName_StemBase();

        AnchorName_SpawnPoint = Settings.GetAnchorName_SpawnPoint();

        AnchorName_SpawnPointDummy = Settings.GetAnchorName_SpawnPointDummy();

        AnchorName_Barrier = Settings.GetAnchorName_Barrier();

        AnchorName_ScoreBoard = Settings.GetAnchorName_ScoreBoard();

        AnchorName_WeaponRack = Settings.GetAnchorName_WeaponRack();

        AnchorName_PistoleMag = Settings.GetAnchorName_PistoleMag();

        AnchorName_AmmoBox = Settings.GetAnchorName_AmmoBox();

        AnchorName_AmmoBoxInfinite = Settings.GetAnchorName_AmmoBoxInfinite();

        AnchorName_PathFinder = Settings.GetAnchorName_GridMap();

        AnchorName_WalkieTalkie = Settings.GetAnchorName_WalkieTalkie();

        AnchorName_MistEmitter = Settings.GetAnchorName_MistEmitter();

        AnchorName_MistEnd = Settings.GetAnchorName_MistEnd();

        AnchorName_HotSpot = Settings.GetAnchorName_HotSpot();
    }
    void Start()
    {
        InitAnchorNameVariables();
        Debug.Log("world managewr is on " + gameObject.name);
        spawns = new List<GameObject>();
        dummySpawns = new List<GameObject>();
        barriers = new List<GameObject>();
        mags = new List<GameObject>();
        ammoBoxes = new List<GameObject>();
        infiniteAmmoBoxes = new List<GameObject>();
        walkieTalkies = new List<GameObject>();
        mists = new List<GameObject>();
        hotspots = new List<GameObject>();
    }

    void Update()
    {
        if (!calledToAnchorStore)
        {
            if (roomLoaded)
            {
                calledToAnchorStore = true;
                WorldAnchorStore.GetAsync(AnchorStoreReady);
            }
        }
    }

    public void RoomLoaded()
    {
        roomLoaded = true;
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;

        // list of strings
        List<string> spawnIds = new List<string>();
        List<string> dummySpawnIds = new List<string>();
        List<string> barrierIds = new List<string>();
        List<string> magIds = new List<string>();
        List<string> ammoBoxIds = new List<string>();
        List<string> infiniteAmmoBoxIds = new List<string>();
        List<string> walkieTalkieIds = new List<string>();
        List<string> mistIds = new List<string>();
        List<string> hotspotIds = new List<string>();


        // gather all stored anchors
        string[] ids = anchorStore.GetAllIds();

        for (int index = 0; index < ids.Length; index++)
        {
            if (ids[index] == AnchorName_StemBase)
            {
                // if anchor is stem base
                GameObject obj = Instantiate(stemBase) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(false);
                stemBasePlaced = true;
            }
            else if (ids[index] == AnchorName_ConsoleObject)
            {
                // if anchor is console object
                GameObject obj = Instantiate(consoleObject) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
            }
            else if (ids[index] == AnchorName_ScoreBoard)
            {
                // if anchor is the scoreboard
                GameObject obj = Instantiate(scoreboard) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
                scoreboardPlaced = true;
            }
            else if (ids[index] == AnchorName_WeaponRack)
            {
                // if anchor is weapons rack
                GameObject obj = Instantiate(weaponsRack) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pscript.SetRotateOnNormals(true);
                pscript.KeepUpright(true);
                weaponsRackPlaced = true;
            }
            else if (ids[index] == AnchorName_PathFinder)
            {
                // if anchor is the pathfinder object
                GameObject obj = Instantiate(pathFinder) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                pathFinderPlaced = true;
            }
            else if (ids[index] == AnchorName_AirStrikeStart)
            {
                // if anchor is the scoreboard
                GameObject obj = Instantiate(airstrikeStart) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                airstrikeStartPlaced = true;
            }
            else if (ids[index] == AnchorName_AirStrikeEnd)
            {
                // if anchor is the scoreboard
                GameObject obj = Instantiate(airstrikeEnd) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                airstrikeEndPlaced = true;
            }
            else if (ids[index] == AnchorName_MistEnd)
            {
                // if anchor is the mistEnd
                GameObject obj = Instantiate(mistEnd) as GameObject;
                PersistoMatic pscript = obj.GetComponent<PersistoMatic>();
                pscript.SetAnchorStoreName(ids[index]);
                mistEndPlaced = true;
            }
            else if (ids[index].Contains(AnchorName_AmmoBoxInfinite))
            {
                // if anchor is an infinite ammo box
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_AmmoBoxInfinite.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > spawnIdNum)
                {
                    infiniteAmmoBoxIdNum = thisId;
                }

                // add id to string list to instantiate later
                infiniteAmmoBoxIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_WalkieTalkie))
            {
                // if anchor is walkieTalkie
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_WalkieTalkie.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > walkieTalkieIdNum)
                {
                    walkieTalkieIdNum = thisId;
                }

                // add id to string list to instantiate later
                walkieTalkieIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_Barrier))
            {
                // if anchor is barrier object
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_Barrier.Length));

                // set barrier id to highest anchorIdNum
                if (thisId > barrierIdNum)
                {
                    barrierIdNum = thisId;
                }

                // add id to string list to instantiate later
                barrierIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_SpawnPoint))
            {
                // if anchor is a spawn point
                // get spawn id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_SpawnPoint.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > spawnIdNum)
                {
                    spawnIdNum = thisId;
                }

                // add id to string list to instantiate later
                spawnIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_SpawnPointDummy))
            {
                // if anchor is a dummy spawn
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_SpawnPointDummy.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > spawnIdNum)
                {
                    dummySpawnIdNum = thisId;
                }

                // add id to string list to instantiate later
                dummySpawnIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_PistoleMag))
            {
                // if anchor is a mag
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_PistoleMag.Length));

                // set mag id to highest anchorIdNum
                if (thisId > magIdNum)
                {
                    magIdNum = thisId;
                }

                // add id to string list to instantiate later
                magIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_AmmoBox))
            {
                // if anchor is an ammo box
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_AmmoBox.Length));

                // set ammo box id to highest anchorIdNum
                if (thisId > ammoBoxIdNum)
                {
                    ammoBoxIdNum = thisId;
                }

                // add id to string list to instantiate later
                ammoBoxIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_MistEmitter))
            {
                // if anchor is mist
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_MistEmitter.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > mistIdNum)
                {
                    mistIdNum = thisId;
                }

                // add id to string list to instantiate later
                mistIds.Add(ids[index]);
            }
            else if (ids[index].Contains(AnchorName_HotSpot))
            {
                // if anchor is a hostpot
                // get id number
                int thisId = int.Parse(ids[index].Substring(AnchorName_HotSpot.Length));

                // set spawn id to highest anchorIdNum
                if (thisId > hotspotIdNum)
                {
                    hotspotIdNum = thisId;
                }

                // add id to string list to instantiate later
                hotspotIds.Add(ids[index]);
            }
        }
///////////////////////ENDFORLOOP


        // load and instantiate all infinite ammo boxes
        foreach (string id in infiniteAmmoBoxIds)
        {
            infiniteAmmoBoxes.Add(InstantiateObject(infiniteAmmoBox, id));
        }

        // load and instantiate all walkieTalkies
        foreach (string id in walkieTalkieIds)
        {
            walkieTalkies.Add(InstantiateObject(walkieTalkie, id, true));
        }

        // load and instantiate all barriers
        foreach (string id in barrierIds)
        {
            barriers.Add(InstantiateObject(barrier, id, true));
        }

        // load and instantiate all spawn points
        foreach (string id in spawnIds)
        {
            spawns.Add(InstantiateObject(spawnPoint, id));
        }

        // load and instantiate all dummy spawn points
        foreach (string id in dummySpawnIds)
        {
            dummySpawns.Add(InstantiateObject(dummySpawnPoint, id));
        }

        // load and instantiate all mags
        foreach (string id in magIds)
        {
            mags.Add(InstantiateObject(mag, id));
        }

        // load and instantiate all ammo boxes
        foreach (string id in ammoBoxIds)
        {
            ammoBoxes.Add(InstantiateObject(ammoBox, id));
        }

        // load and instantiate all mist objects
        foreach (string id in mistIds)
        {
            mists.Add(InstantiateObject(mist, id));
        }

        // load and instantiate all hostpot objects
        foreach (string id in hotspotIds)
        {
            hotspots.Add(InstantiateObject(hotspot, id));
        }
    }

    GameObject InstantiateObject(GameObject obj, string id, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj) as GameObject;
        PersistoMatic pscript = o.GetComponent<PersistoMatic>();
        pscript.SetAnchorStoreName(id);
        pscript.SetRotateOnNormals(rotateOnNormals);
        pscript.KeepUpright(keepUpright);
        return o;
    }

    GameObject InstantiateObject(GameObject obj, string id, Vector3 position, Quaternion rotation, bool rotateOnNormals = false, bool keepUpright = false)
    {
        GameObject o = Instantiate(obj, position, rotation) as GameObject;
        PersistoMatic pscript = o.GetComponent<PersistoMatic>();
        pscript.SetAnchorStoreName(id);
        pscript.SetRotateOnNormals(rotateOnNormals);
        pscript.KeepUpright(keepUpright);
        return o;
    }

    public int GetSpawnCount()
    {
        return spawns.Count;
    }

    public int GetDummySpawnCount()
    {
        return dummySpawns.Count;
    }

    public int GetBarriersCount()
    {
        return barriers.Count;
    }

    public int GetMagCount()
    {
        return mags.Count;
    }

    public int GetAmmoBoxCount()
    {
        return ammoBoxes.Count;
    }

    public int GetInfiniteAmmoBoxCount()
    {
        return infiniteAmmoBoxes.Count;
    }

    public int GetWalkieTalkieCount()
    {
        return walkieTalkies.Count;
    }

    public int GetMistCount()
    {
        return mists.Count;
    }

    public int GetHotspotCount()
    {
        return hotspots.Count;
    }

    public bool isScoreboadPlaced()
    {
        return scoreboardPlaced;
    }

    public bool isAirstrikeStartPlaced()
    {
        return airstrikeStartPlaced;
    }

    public bool isAirstrikeEndPlaced()
    {
        return airstrikeEndPlaced;
    }

    //used in demowave to checkoff placed things in ui
    public bool isMistEndPlaced()
    {
        return mistEndPlaced;
    }

    public bool isPathFinderPlaced()
    {
        return pathFinderPlaced;
    }

    public int GetSpawnIdNum()
    {
        spawnIdNum++;
        return spawnIdNum;
    }

    public int GetDummySpawnIdNum()
    {
        dummySpawnIdNum++;
        return dummySpawnIdNum;
    }

    public int GetBarrierIdNum()
    {
        barrierIdNum++;
        return barrierIdNum;
    }

    public int GetMagIdNum()
    {
        magIdNum++;
        return magIdNum;
    }

    public int GetHotspotIdNum()
    {
        hotspotIdNum++;
        return hotspotIdNum;
    }

    public int GetAmmoBoxIdNum()
    {
        ammoBoxIdNum++;
        return ammoBoxIdNum;
    }

    public int GetInfiniteAmmoBoxIdNum()
    {
        infiniteAmmoBoxIdNum++;
        return infiniteAmmoBoxIdNum;
    }

    public int GetWalkieTalkieIdNum()
    {
        walkieTalkieIdNum++;
        return walkieTalkieIdNum;
    }

    public int GetMistIdNum()
    {
        mistIdNum++;
        return mistIdNum;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleWireframe()
    {
        drawWireframe = !drawWireframe;
        if (drawWireframe)
            SpatialMappingManager.Instance.SurfaceMaterial = wireframeMaterial;
        else
            SpatialMappingManager.Instance.SurfaceMaterial = occlusionMaterial;
    }

    public void CreateStemBase()
    {
        if (stemBasePlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            GameObject obj = InstantiateObject(stemBase, AnchorName_StemBase, GazeManager.Instance.HitInfo.point, stemBase.transform.rotation) as GameObject;
            obj.GetComponent<PersistoMatic>().SetFaceCamera(true);
            stemBasePlaced = true;
        }
    }

    public void CreateSpawnPoint()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_SpawnPoint+ GetSpawnIdNum().ToString();
            spawns.Add(InstantiateObject(spawnPoint, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateDummySpawnPoint()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_SpawnPointDummy + GetDummySpawnIdNum().ToString();
            dummySpawns.Add(InstantiateObject(dummySpawnPoint, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateBarrier()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_Barrier + GetBarrierIdNum().ToString();
            walkieTalkies.Add(InstantiateObject(barrier, id, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true));
        }
    }

    public void CreateAmmoBox()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_AmmoBox + GetAmmoBoxIdNum().ToString();
            ammoBoxes.Add(InstantiateObject(ammoBox, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateConsole()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(consoleObject, AnchorName_ConsoleObject, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true);
        }
    }

    public void CreateHotspot()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            string id = AnchorName_HotSpot + GetHotspotIdNum().ToString();
            hotspots.Add(InstantiateObject(hotspot, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateInfiniteAmmoBox()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_AmmoBoxInfinite + GetInfiniteAmmoBoxIdNum().ToString();
            infiniteAmmoBoxes.Add(InstantiateObject(infiniteAmmoBox, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateScoreboard()
    {
        if (scoreboardPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(scoreboard, AnchorName_ScoreBoard, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            scoreboardPlaced = true;
        }
    }

    public void CreateWeaponsRack()
    {
        if (weaponsRackPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(weaponsRack, AnchorName_WeaponRack, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true, true);
            weaponsRackPlaced = true;
        }
    }

    public void CreateMag()
    {

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_PistoleMag + GetMagIdNum().ToString();
            mags.Add(InstantiateObject(mag, id, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true));
        }
    }

    public void CreatePathFinder()
    {
        if (pathFinderPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(pathFinder, AnchorName_PathFinder, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            pathFinderPlaced = true;
        }
    }

    public void CreateWalkieTalkie()
    {
        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_WalkieTalkie + GetWalkieTalkieIdNum().ToString();
            walkieTalkies.Add(InstantiateObject(walkieTalkie, id, GazeManager.Instance.HitInfo.point, Quaternion.FromToRotation(Vector3.forward, GazeManager.Instance.HitInfo.normal), true));
        }
    }

    public void CreateMist()
    {

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            string id = AnchorName_MistEmitter + GetMistIdNum().ToString();
            mists.Add(InstantiateObject(mist, id, GazeManager.Instance.HitInfo.point, Quaternion.identity));
        }
    }

    public void CreateMistEnd()
    {
        if (mistEndPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(mistEnd, AnchorName_MistEnd, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            mistEndPlaced = true;
        }
    }

    public void CreateAirstrikeStart()
    {
        if (airstrikeStartPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(airstrikeStart, AnchorName_AirStrikeStart, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            airstrikeStartPlaced = true;
        }
    }

    public void CreateAirstrikeEnd()
    {
        if (airstrikeEndPlaced)
            return;

        if (GazeManager.Instance.isActiveAndEnabled)
        {
            // instantiate object at raycast hit point
            InstantiateObject(airstrikeEnd, AnchorName_AirStrikeEnd, GazeManager.Instance.HitInfo.point, Quaternion.identity);
            airstrikeEndPlaced = true;
        }
    }

    public void Removing(PersistoMatic pScript)
    {
        if (pScript.AnchorStoreBaseName == AnchorName_StemBase)
        {
            stemBasePlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_ScoreBoard)
        {
            scoreboardPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_WeaponRack)
        {
            weaponsRackPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_PathFinder)
        {
            pathFinderPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_AirStrikeStart)
        {
            airstrikeStartPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_AirStrikeEnd)
        {
            airstrikeEndPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_SpawnPoint))
        {
            spawns.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_SpawnPointDummy))
        {
            dummySpawns.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_Barrier))
        {
            barriers.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_AmmoBox))
        {
            ammoBoxes.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_AmmoBoxInfinite))
        {
            infiniteAmmoBoxes.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_PistoleMag))
        {
            mags.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_MistEmitter))
        {
            mists.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName == AnchorName_MistEnd)
        {
            mistEndPlaced = false;
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_WalkieTalkie))
        {
            walkieTalkies.Remove(pScript.gameObject);
        }
        else if (pScript.AnchorStoreBaseName.Contains(AnchorName_HotSpot))
        {
            hotspots.Remove(pScript.gameObject);
        }
    }

    void OnReset()
    {
        stemBasePlaced = false;         // only one stem base is allowed
        spawnIdNum = 0;                 // id num associated with the number of spawn points in the room
        dummySpawnIdNum = 0;            // id num associated with the number of dummy spawn points in the room
        barrierIdNum = 0;               // id num associated with the number of barrier objects in the room
        scoreboardPlaced = false;       // only one scoreboard is allowed
        weaponsRackPlaced = false;      // only one weapons rack is allowed
        magIdNum = 0;                   // id num associated with the number of mags in the room
        ammoBoxIdNum = 0;               // id num associated with the number of ammo boxes in the room
        infiniteAmmoBoxIdNum = 0;       // id num associated with the number of infinite ammo boxes in the room
        pathFinderPlaced = false;       // only one pathfinder is allowed
        walkieTalkieIdNum = 0;          // id num associated with the number of walkie talkies in the room
        mistIdNum = 0;                  // id num associated with the number of mist objects in the room
        hotspotIdNum = 0;               // id num associated with the number of hotspot objects in the room
        airstrikeStartPlaced = false;   // only one airstrike start is allowed
        airstrikeEndPlaced = false;     // only one airstrike end is allowed
        mistEndPlaced = false;
    }
}