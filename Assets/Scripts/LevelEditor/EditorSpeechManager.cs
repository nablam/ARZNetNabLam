// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EditorSpeechManager : MonoBehaviour
{
    public GameObject worldManagerObject;
    //public GameObject waveEditorManagerObject;

    WorldManager worldManager;
    RoomLoader roomLoader;
    WaveEditorManager waveEditorManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


    void EditorKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.Z)) { worldManager.CreateSpawnPoint(); }
        if (Input.GetKeyDown(KeyCode.X)) { worldManager.CreatePathFinder(); }
        if (Input.GetKeyDown(KeyCode.C)) { worldManager.CreateBarrier(); }
        if (Input.GetKeyDown(KeyCode.V)) { worldManager.CreateScoreboard(); }
        if (Input.GetKeyDown(KeyCode.B)) { worldManager.CreateWeaponsRack(); }
        if (Input.GetKeyDown(KeyCode.N)) { worldManager.CreateInfiniteAmmoBox(); }
        if (Input.GetKeyDown(KeyCode.M)) { worldManager.LoadScene("MainMenu"); }
        if (Input.GetKeyDown(KeyCode.Comma)) { worldManager.CreateConsole(); }
        if (Input.GetKeyDown(KeyCode.L)) { worldManager.CreateStemBase(); }
        if (Input.GetKeyDown(KeyCode.LeftBracket)) { worldManager.CreateMist(); }
        if (Input.GetKeyDown(KeyCode.RightBracket)) { worldManager.CreateMistEnd(); }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PersistoMatic[] objects = (PersistoMatic[])GameObject.FindObjectsOfType(typeof(PersistoMatic));
            foreach (PersistoMatic obj in objects)
            {
                obj.SendMessage("OnRemove");
            }
        }
       
        if (Input.GetKeyDown(KeyCode.K)) { worldManager.CreateHotspot(); }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null) { focusObject.SendMessage("OnRemove"); }
        }
 

    }
    
    void Update()
    {
        EditorKeyboardInputs();
    }

    // Use this for initialization
    void Start()
    {
        if (worldManagerObject == null)
        {
            worldManager = FindObjectOfType<WorldManager>();
        }
        else
            worldManager = worldManagerObject.GetComponent<WorldManager>();

        roomLoader = FindObjectOfType<RoomLoader>();

        // load main menu scene
        keywords.Add("Main Menu", () =>
        {
            worldManager.LoadScene("MainMenu");
        });

        // Call the OnReset method on every gameobject.
        keywords.Add("Reset world", () =>
        {
            PersistoMatic[] objects = (PersistoMatic[])GameObject.FindObjectsOfType(typeof(PersistoMatic));
            foreach (PersistoMatic obj in objects)
            {
                obj.SendMessage("OnRemove");
            }  
        });

        keywords.Add("Toggle wireframe", () =>
        {
            worldManager.ToggleWireframe();
        });

        keywords.Add("Toggle room", () =>
        {
            roomLoader.ToggleRoom();
        });

        keywords.Add("Start scan", () =>
        {
            SpatialMappingManager.Instance.StartObserver();
        });

        keywords.Add("Stop scan", () =>
        {
            SpatialMappingManager.Instance.StopObserver();
        });

        keywords.Add("Place stem base", () =>
        {
            worldManager.CreateStemBase();
        });

        keywords.Add("Place console", () =>
        {
            worldManager.CreateConsole();
        });

        keywords.Add("Place spawn", () =>
        {
            worldManager.CreateSpawnPoint();
        });

        keywords.Add("Place dummy spawn", () =>
        {
            worldManager.CreateDummySpawnPoint();
        });

        keywords.Add("Place barrier", () =>
        {
            worldManager.CreateBarrier();
        });

        keywords.Add("Place scoreboard", () =>
        {
            worldManager.CreateScoreboard();
        });

        keywords.Add("Place weapons rack", () =>
        {
            worldManager.CreateWeaponsRack();
        });

        keywords.Add("Place mag", () =>
        {
            worldManager.CreateMag();
        });

        keywords.Add("Place ammo box", () =>
        {
            worldManager.CreateAmmoBox();
        });

        keywords.Add("Place infinite ammo box", () =>
        {
            worldManager.CreateInfiniteAmmoBox();
        });

        keywords.Add("Place path finder", () =>
        {
            worldManager.CreatePathFinder();
        });

        keywords.Add("Place radio", () =>
        {
            worldManager.CreateWalkieTalkie();
        });

        keywords.Add("Place mist", () =>
        {
            worldManager.CreateMist();
        });

        keywords.Add("Place mist end", () =>
        {
            worldManager.CreateMistEnd();
        });
       

        keywords.Add("Place hot spot", () =>
        {
            worldManager.CreateHotspot();
        });

        keywords.Add("Place airstrike start", () =>
        {
            worldManager.CreateAirstrikeStart();
        });

        keywords.Add("Place airstrike end", () =>
        {
            worldManager.CreateAirstrikeEnd();
        });

        keywords.Add("Remove", () =>
        {
            var focusObject = GazeManager.Instance.HitObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnRemove");
            }
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