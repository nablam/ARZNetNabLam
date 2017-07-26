using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;



public class NetPlacer : NetworkBehaviour {
    /// <summary>
    /// The transform of the shared world anchor.
    /// </summary>
    private Transform sharedWorldAnchorTransform;

    public GameObject Nettobj;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public override void OnStartServer()
    {
        sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
        transform.SetParent(sharedWorldAnchorTransform);
        FindSpawnpointINScene();

        
        GameObject NetSpawner=   Instantiate(Nettobj, foundSpawn.transform.localPosition, Quaternion.identity) as GameObject;
        NetSpawner.transform.SetParent(sharedWorldAnchorTransform);
        NetSpawner.GetComponent<GrabPathsStar>().OKstarDoInitWillbeCalledFromONserverstart();
    }

  

    GameObject foundSpawn;
    void FindSpawnpointINScene() {
        foundSpawn = GameObject.FindGameObjectWithTag("Respawn");
    } 
}
