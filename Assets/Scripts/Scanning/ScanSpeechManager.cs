// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.SpatialMapping;


public class ScanSpeechManager : MonoBehaviour {

    public GameObject scanManagerObject;
 
        ScanManager scanManager;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    void ScanKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            scanManager.SaveRoom();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            scanManager.LoadScene("MainMenu");
        }
    }
    void Update()
    {
        ScanKeyboardInputs();
    }
    // Use this for initialization
    void Start()
    {
        if (scanManagerObject == null)
        {
            scanManager = GameObject.Find("ScanManager").GetComponent<ScanManager>();
        }
        else
            scanManager = scanManagerObject.GetComponent<ScanManager>();

        keywords.Add("Main Menu", () =>
        {
            // load main menu scene
            scanManager.LoadScene("MainMenu");
        });

        keywords.Add("Save room", () =>
        {
            // save the current mesh
            scanManager.SaveRoom();
            Debug.Log("Saved room");
        });

        keywords.Add("Save and Quit", () =>
        {
            // save the current mesh
            scanManager.SaveRoom();

            // return to main menu
            scanManager.LoadScene("MainMenu");
        });

        keywords.Add("Stop scanner", () => {
            SpatialMappingManager.Instance.StopObserver();
        });

        keywords.Add("Start scanner", () =>
        {
            SpatialMappingManager.Instance.StartObserver();
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
