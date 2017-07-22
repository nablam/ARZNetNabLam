using HoloToolkit.Examples.SharingWithUNET;
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
public class LobbySpeech : MonoBehaviour {
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    // Use this for initialization

    public NetworkDiscoveryWithAnchors nd;
    void Start()
    {
        keywords.Add("Go Net One", () =>
        {
            nd.DoInitPlease();


        });

        keywords.Add("Go Net Two", () =>
        {
            nd.DoInit2();


        });

        keywords.Add("Create Anchor", () =>
        {
            
            UNetAnchorManager.Instance.CreateAnchor();
        });

        keywords.Add("all in one", () =>
        {

            nd.DOallInitsandMaybecreate();
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
}
