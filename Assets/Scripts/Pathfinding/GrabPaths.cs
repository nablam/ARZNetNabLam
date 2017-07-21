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


    void Start() {

       // Debug.Log("my net name=" + name);
    }

    string GetyourName() { return  gameObject.name.Substring(0, gameObject.name.Length - 3);    }
    Transform LocateSpawTagWithSameName()
    {
        string myname = "Respawn"; //GetyourName();

        GameObject spawnTagWithSameNameAsMine= GameObject.Find(myname);
        _MycorrespondingSpawnTagObject = spawnTagWithSameNameAsMine;
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
        PlaceYourselfOnSpawTagWithSameName();
        PathsManager GOpm = GameObject.FindObjectOfType<PathsManager>();
        _Z1FOUND = GameObject.FindGameObjectWithTag("ZoneOne");
        _Z2Found = GameObject.FindGameObjectWithTag("ZoneTwo");

        DecideWhoToAttack();
        if (AttackingZone1)
        {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET( _Z1FOUND, _MycorrespondingSpawnTagObject);
           // Debug.Log("grabbed " + Paths_to_ChosenZone_Grabbed.Count);
            StartCoroutine(Startin3());
        }
        else {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET( _Z2Found, _MycorrespondingSpawnTagObject);
           // Debug.Log("grabbed " + Paths_to_ChosenZone_Grabbed.Count);
            StartCoroutine(Startin3());
        }


        //  string name = gameObject.name.Substring(0, gameObject.name.Length - 3);


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
        for (int x = 0; x < GameManager.Instance.Settings.TotalZombiesToSpawn; x++)
        {
            yield return new WaitForSeconds(GameManager.Instance.Settings.SpawnInterval);
            //Instantiate(zombietospawn);
            // SpawnZonNetwork_OneFirstPath();
            SpawnZonNetwork_InOrder();
        }
    }

    public void SpawnZonNetwork_OneFirstPath()
    {
        //if server 

        GameObject go = Instantiate(Z, transform.position, Quaternion.identity);
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
