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
    //[ServerCallback]
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


        DecideWhoToAttack();

       // Debug.Log("decided to attack zoneone " + AttackingZone1);
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
       // Debug.Log("path grabbed has " + Paths_to_ChosenZone_Grabbed.Count + " verts in it ");


    }
  //  [ServerCallback]
    void DecideWhoToAttack()
    {
        AttackingZone1 = true;// Random.Range(0, 100) % 2 == 0 ? true : false;
    }
   // [ServerCallback]
    IEnumerator Startin3()
    {
        Debug.Log("waiting for delay");
        yield return new WaitForSeconds(GameSettings.Instance.spawndelay);
        BuildStakatoOfZombies();
    }
  //  [ServerCallback]
    void BuildStakatoOfZombies()
    {
        StartCoroutine(MakeZEvryXForTicks());
    }
  //  [ServerCallback]
    IEnumerator MakeZEvryXForTicks()
    {
        for (int x = 0; x < GameSettings.Instance.TotalZombiesToSpawn; x++)
        {
            yield return new WaitForSeconds(GameSettings.Instance.SpawnInterval);
            //Instantiate(zombietospawn);
            Debug.Log("Time to make aZZZZZZZ");
            SpawnZonNetwork_OneFirstPath(x, ZbetterrootONly);
            //SpawnZonNetwork_InOrder();
        }
    }

    //or do the rpc Relative thing
   // [ServerCallback]
    public void SpawnZonNetwork_OneFirstPath(int argx, GameObject argZ)
    {
        //  Quaternion rotation = anchor.transform.localRotation * relativeOrientation;
        //GameObject go = Instantiate(Znon, sharedWorldAnchorTransform.InverseTransformPoint(transform.position) , Quaternion.identity);
        GameObject go;
        if (argx == 0)
        {
            go = Instantiate(argZ, transform.position, Quaternion.identity);         
        }
        else if (argx == 1)
        {

            go = Instantiate(argZ, sharedWorldAnchorTransform.TransformPoint(transform.position), Quaternion.identity);
        }
        else if (argx == 2)
        {
            go = Instantiate(argZ, sharedWorldAnchorTransform.InverseTransformPoint(transform.position), Quaternion.identity);
        }
        else if (argx == 3)
        {
            go = Instantiate(argZ, sharedWorldAnchorTransform.InverseTransformPoint(sharedWorldAnchorTransform.TransformPoint(transform.position)), Quaternion.identity);
        }

        else if (argx == 4)
        {
            go = Instantiate(argZ, transform.localPosition, Quaternion.identity);
        }


        else if (argx == 5)
        {
            go = Instantiate(argZ, sharedWorldAnchorTransform.TransformPoint(transform.localPosition), Quaternion.identity);
        }
        else if (argx == 6)
        {
            go = Instantiate(argZ, sharedWorldAnchorTransform.InverseTransformPoint(transform.localPosition), Quaternion.identity);
        }

        else if(argx==7)
        {
            go = Instantiate(argZ, sharedWorldAnchorTransform.InverseTransformPoint(sharedWorldAnchorTransform.TransformPoint(transform.localPosition)), Quaternion.identity);
        }
        else
        {
            go = Instantiate(argZ, transform.position, Quaternion.identity);
        }
        //   Debug.Log("spawner puts z on path of " + Paths_to_ChosenZone_Grabbed[0].Count + " nodes");
        //  go.transform.SetParent(sharedWorldAnchorTransform);

       

        List<Vector3> conertedToinversShared = new List<Vector3>();
        foreach (Vector3 v3 in Paths_to_ChosenZone_Grabbed[0]) {
            conertedToinversShared.Add(sharedWorldAnchorTransform.InverseTransformPoint(v3));
        }


       // go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[0]);
        go.GetComponent<FollowPath>().FollowThisPath(Paths_to_ChosenZone_Grabbed[0]);
       // go.GetComponent<FollowPath>().tm.text = argx.ToString();
        NetworkServer.Spawn(go);
        

        //if (isServer) { Debug.Log("pathstargrab XXX---IAMSERVER"); } else { Debug.Log("pathstargrab XXX--- not server"); }
        //if (isClient) { Debug.Log("pathstargrab XXX---IAM CLIENT"); } else { Debug.Log("pathstargrab XXX--- not client"); }
        //if (isLocalPlayer) { Debug.Log("pathstargrab XXX---IAM Localplayer"); } else { Debug.Log("pathstargrab XXX--- not loclplayer"); }

    }




}
