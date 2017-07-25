using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabPathsStar : NetworkBehaviour
{
    List<List<Vector3>> Paths_to_ChosenZone_Grabbed;
    public GameObject Z;

    GameObject _Z1FOUND;
    GameObject _Z2Found;
    PathsManager GOpm;
    bool AttackingZone1;
    GameObject _MycorrespondingSpawnTagObject;
    void Start()
    {

        initmeWhenIAwakeOnServer();
    }
  
    [Server]
    void initmeWhenIAwakeOnServer() {


        this.transform.parent = SharedCollection.Instance.gameObject.transform;
        _MycorrespondingSpawnTagObject = GameObject.FindGameObjectWithTag("Respawn");

        GOpm = GameObject.FindObjectOfType<PathsManager>().GetComponent<PathsManager>();
        if (GOpm != null) { Debug.Log("found pathmanager YO"); } else { Debug.Log(" YO  NO pathmanager"); }

        string Z1TAGNAME = "ZoneOne";
        _Z1FOUND = GameObject.FindGameObjectWithTag(Z1TAGNAME);
        if (_Z1FOUND != null) { Debug.Log("found Z1"); } else { Debug.Log("NO Z1"); }

        string Z2TAGNAME = "ZoneTwo";
        _Z2Found = GameObject.FindGameObjectWithTag(Z2TAGNAME);
        if (_Z2Found != null) { Debug.Log("found Z2"); } else { Debug.Log("NO Z2"); }


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


    }

    void DecideWhoToAttack()
    {
        AttackingZone1 = Random.Range(0, 100) % 2 == 0 ? true : false;
    }

    IEnumerator Startin3()
    {
        yield return new WaitForSeconds(1);
        BuildStakatoOfZombies();
    }
    void BuildStakatoOfZombies()
    {
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

    //or do the rpc Relative thing
    public void SpawnZonNetwork_OneFirstPath()
    {
        GameObject go = Instantiate(Z, SharedCollection.Instance.gameObject.transform.InverseTransformPoint( transform.position), Quaternion.identity);



        Debug.Log("spawner puts z on path of " + Paths_to_ChosenZone_Grabbed[0].Count + " nodes");
        go.AddComponent<FollowPath>();

        go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[0]);

        NetworkServer.Spawn(go);
    }

 




}
