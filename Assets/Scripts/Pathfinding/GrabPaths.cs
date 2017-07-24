using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabPaths : NetworkBehaviour {

    List<List<Vector3>> Paths_to_ChosenZone_Grabbed;
    public GameObject Z;
    GameObject _MycorrespondingSpawnTagObject;

    GameObject _Z1FOUND;
    GameObject _Z2Found;
    PathsManager GOpm;

    //void Start() {
    //    //  DictoPlacedObjects.Instance._whatIdoWhenIHhrarExpDone += YoIHEardTereWasAnAnchoreExported;
    //    Debug.Log("yO I STARTED BUT NOT IN SERVER");
    //    // Debug.Log("my net name=" + name);
    //}

    string GetyourName() { return  gameObject.name.Substring(0, gameObject.name.Length - 3);    }
    Transform LocateSpawTagWithSameName()
    {
        string myname = "Respawn"; //GetyourName();

        GameObject spawnTagWithSameNameAsMine = GameObject.FindGameObjectWithTag(myname);
        _MycorrespondingSpawnTagObject = spawnTagWithSameNameAsMine;

         GOpm = GameObject.FindObjectOfType<PathsManager>().GetComponent<PathsManager>();
        if (GOpm != null) { Debug.Log("found pathmanager YO"); } else { Debug.Log(" YO  NO pathmanager"); }

        string Z1TAGNAME = "ZoneOne";
        _Z1FOUND = GameObject.FindGameObjectWithTag(Z1TAGNAME);
        if (_Z1FOUND != null) { Debug.Log("found Z1"); } else { Debug.Log("NO Z1"); }

        string Z2TAGNAME = "ZoneTwo";
        _Z2Found = GameObject.FindGameObjectWithTag(Z2TAGNAME);
        if (_Z2Found != null) { Debug.Log("found Z2"); } else { Debug.Log("NO Z2"); }



        return spawnTagWithSameNameAsMine.transform;
    }
    void PlaceYourselfOnSpawTagWithSameName() {
        this.transform.position = LocateSpawTagWithSameName().position;
        this.transform.parent = SharedCollection.Instance.gameObject.transform;
        // server has spawn parented to sharedcollection
        //scenario 1: client knows where to plcae it /parented
        //scenario2 : no parenting,
        // server side =>just grab relative pos 
        // serve side => syncvar
        // client updates from revers transform point
    }


    public override void OnStartServer()
    {
        DictoPlacedObjects.Instance._whatIdoWhenIHhrarExpDone += YoIHEardTereWasAnAnchoreExported;
        //  string name = gameObject.name.Substring(0, gameObject.name.Length - 3);
    }




    void YoIHEardTereWasAnAnchoreExported(Vector3 argWApos)
    {
       // PlaceYourselfOnSpawTagWithSameName();





        DecideWhoToAttack();
        Debug.Log("decided to attack zoneone " + AttackingZone1);
        if (AttackingZone1)
        {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET(_Z1FOUND, _MycorrespondingSpawnTagObject);
            // Debug.Log("grabbed " + Paths_to_ChosenZone_Grabbed.Count);
            StartCoroutine(Startin3());
        }
        else
        {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET(_Z2Found, _MycorrespondingSpawnTagObject);
            // Debug.Log("grabbed " + Paths_to_ChosenZone_Grabbed.Count);
            StartCoroutine(Startin3());
        }
        Debug.Log("path grabbed has " + Paths_to_ChosenZone_Grabbed.Count + " verts in it ");

        //only the server would run this since it is the one exporting an anchor,
        //btw exportanchorDone fires the event 
        //and this object 

    }



    bool AttackingZone1 = true;
    void DecideWhoToAttack() {
        AttackingZone1 = Random.Range(0, 100) % 2 == 0 ? true : false;
    }




    IEnumerator Startin3() {
        yield return new WaitForSeconds(1);
        BuildStakatoOfZombies();
    }


    void BuildStakatoOfZombies(){
        StartCoroutine(MakeZEvryXForTicks());
    }

    IEnumerator MakeZEvryXForTicks()
    {
        for (int x = 0; x < GameSettings.Instance.TotalZombiesToSpawn; x++)
        {
            yield return new WaitForSeconds(GameSettings.Instance.SpawnInterval);
            //Instantiate(zombietospawn);
            Debug.Log("Time to make aZZZZZZZ");
            SpawnZonNetwork_OneFirstPath();
            //SpawnZonNetwork_InOrder();
        }
    }

    public void SpawnZonNetwork_OneFirstPath()
    {
       

        GameObject go = Instantiate(Z, transform.position, Quaternion.identity);
        Debug.Log("spawner puts z on path of "+ Paths_to_ChosenZone_Grabbed[0].Count+" nodes");
        go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[0]);

        NetworkServer.Spawn(go);
    }

    int NumberOfZombiesSPawned = 0;
    public void SpawnZonNetwork_InOrder()
    {
        NumberOfZombiesSPawned++;
        if (NumberOfZombiesSPawned >= Paths_to_ChosenZone_Grabbed.Count) NumberOfZombiesSPawned = 0;

        GameObject go = Instantiate(Z, transform.position, Quaternion.identity);
        go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[NumberOfZombiesSPawned]);
        NetworkServer.Spawn(go);
    }

}
