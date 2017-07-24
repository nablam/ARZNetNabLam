using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;



public class NetPlacer : NetworkBehaviour {


    public GameObject Nettobj;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
  

        keywords.Add("Put it there", () =>
        {
            PlaceNetObject();


        });
 

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }



    void PlaceNetObject() {
        Debug.Log("putting it there");
        FindSpawnpointINScene();
        // PlaceItOnRespawn();
        simpleInstance();

    }

    GameObject foundSpawn;
    void FindSpawnpointINScene() {
        foundSpawn = GameObject.FindGameObjectWithTag("Respawn");
    }

    void simpleInstance()
    {
        if (isServer) {
            Vector3 relativePosition = SharedCollection.Instance.gameObject.transform.InverseTransformPoint(foundSpawn.transform.position);
            RpcSimpleInstance(relativePosition);
        } 
    }


    [ClientRpc]
    void RpcSimpleInstance(Vector3 argrelativePosition)
    {
        Vector3 Clientposition = SharedCollection.Instance.gameObject.transform.TransformPoint(argrelativePosition);
        Instantiate(Nettobj, Clientposition, Quaternion.identity);
        

    }

    void PlaceItOnRespawn() {
        Nettobj.transform.position = foundSpawn.transform.position;
    }
}
