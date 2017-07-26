using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabPathsStar : NetworkBehaviour
{

    /// <summary>
    /// The transform of the shared world anchor.
    /// </summary>
    private Transform sharedWorldAnchorTransform;
    List<List<Vector3>> Paths_to_ChosenZone_Grabbed;
    public GameObject Znon;
    public GameObject ZWithFollow;

    public GameObject Zbetter;            //  
    public GameObject ZbetterCubeFirst;   //  Root[empty-syncmotion, followpath] -->child1-> netser cubes  Child2=text
    public GameObject ZbetterrootCubeTextAllNested; //  Root[empty-syncmotion, followpath] -->Child1=text -> netser cubes
    public GameObject ZbetterrootONly;     //  Rootobject is a mesh with syncmotion 

    GameObject _Z1FOUND;
    GameObject _Z2Found;
    PathsManager GOpm;
    bool AttackingZone1;
    GameObject _MycorrespondingSpawnTagObject;
    //public override void OnStartServer()
    //{
    //    sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
    //    initmeWhenIAwakeOnServer();
    //}


    public void OKstarDoInitWillbeCalledFromONserverstart()
    {
        sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
        initmeWhenIAwakeOnServer();
    }

    //maybe a command is better , it will execute on server 
    void initmeWhenIAwakeOnServer() {
      
        _MycorrespondingSpawnTagObject = GameObject.FindGameObjectWithTag("Respawn");

        GOpm = GameObject.FindObjectOfType<PathsManager>().GetComponent<PathsManager>();
        if (GOpm != null) { Debug.Log("found pathmanager YO"); } else { Debug.Log(" YO  NO pathmanager"); }

        string Z1TAGNAME = "ZoneOne";
        _Z1FOUND = GameObject.FindGameObjectWithTag(Z1TAGNAME);
        if (_Z1FOUND != null) { Debug.Log("found Z1"); } else { Debug.Log("NO Z1"); }

        string Z2TAGNAME = "ZoneTwo";
        _Z2Found = GameObject.FindGameObjectWithTag(Z2TAGNAME);
        if (_Z2Found != null) { Debug.Log("found Z2"); } else { Debug.Log("NO Z2"); }

        //todo: deciding who to attack should be done on each sawn
        DecideWhoToAttack();

       // Debug.Log("decided to attack zoneone " + AttackingZone1);
        if (AttackingZone1)
        {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET(_Z1FOUND, _MycorrespondingSpawnTagObject);
            StartCoroutine(Startin3());
        }
        else
        {
            Paths_to_ChosenZone_Grabbed = GOpm.GiveMeMyPathsForIamNetworkedSpawnNET(_Z2Found, _MycorrespondingSpawnTagObject);
            StartCoroutine(Startin3());
        }


    }
    void DecideWhoToAttack()
    {
        AttackingZone1 = Random.Range(0, 100) % 2 == 0 ? true : false;
    }
    IEnumerator Startin3()
    {
        Debug.Log("waiting for delay");
        yield return new WaitForSeconds(GameSettings.Instance.spawndelay);
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
            SpawnZonNetwork_OneFirstPath(x, ZbetterrootONly);
        }
    }

    public void SpawnZonNetwork_OneFirstPath(int argx, GameObject argZ)
    {
        GameObject go;
        go = Instantiate(argZ, transform.position, Quaternion.identity);

        //List<Vector3> conertedToinversShared = new List<Vector3>();
        //foreach (Vector3 v3 in Paths_to_ChosenZone_Grabbed[0]) {
        //    conertedToinversShared.Add(sharedWorldAnchorTransform.InverseTransformPoint(v3));
        //}

        go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[GetRandomPathIndex()]);
 
        NetworkServer.Spawn(go);
  
    }

    int GetRandomPathIndex() {

        return  Random.Range(0, Paths_to_ChosenZone_Grabbed.Count);
    }



}
