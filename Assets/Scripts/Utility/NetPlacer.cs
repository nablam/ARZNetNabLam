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
        //NetSpawner.transform.parent = SharedCollection.Instance.gameObject.transform;
        NetSpawner.transform.SetParent(sharedWorldAnchorTransform);
        NetSpawner.GetComponent<GrabPathsStar>().OKstarDoInitWillbeCalledFromONserverstart();
    }

    //void Start()
    //{

    //    sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
    //    transform.SetParent(sharedWorldAnchorTransform);

    //    keywords.Add("Put it there", () =>
    //    {
    //        PlaceNetObject();


    //    });


    //    // Tell the KeywordRecognizer about our keywords.
    //    keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

    //    // Register a callback for the KeywordRecognizer and start recognizing!
    //    keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
    //    keywordRecognizer.Start();
    //}



    //private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    //    System.Action keywordAction;
    //    if (keywords.TryGetValue(args.text, out keywordAction))
    //    {
    //        keywordAction.Invoke();
    //    }
    //}



    //void PlaceNetObject() {
    //    Debug.Log("putting it there");
    //    FindSpawnpointINScene();
    //    // PlaceItOnRespawn();
    //    simpleInstanceOnMeAndOther();
    //}

    GameObject foundSpawn;
    void FindSpawnpointINScene() {
        foundSpawn = GameObject.FindGameObjectWithTag("Respawn");
    }

    //void simpleInstanceOnMeAndOther()
    //{
    //    if (isServer) {
    //        Vector3 relativePosition = SharedCollection.Instance.gameObject.transform.InverseTransformPoint(foundSpawn.transform.position);
    //        RpcSimpleInstance(relativePosition);
    //    } 
    //}

    ////thsi is how we willplace ammoboxes and stuff on the client 
    //[ClientRpc]
    //void RpcSimpleInstance(Vector3 argrelativePosition)
    //{
    //    Vector3 Clientposition = SharedCollection.Instance.gameObject.transform.TransformPoint(argrelativePosition);
    //    GameObject NetSpawner=   Instantiate(Nettobj, Clientposition, Quaternion.identity) as GameObject;
    //    NetSpawner.transform.parent = SharedCollection.Instance.gameObject.transform;
         
    //}




    //void PlaceItOnRespawn() {
    //    Nettobj.transform.position = foundSpawn.transform.position;
    //}
}
