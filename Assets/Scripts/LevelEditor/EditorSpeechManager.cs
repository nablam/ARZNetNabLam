 

using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EditorSpeechManager : MonoBehaviour
{
 

    public GameEditMNGR gameEditorManager;
    RoomLoader roomLoader;
    WaveEditorManager waveEditorManager;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


    void EditorKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_TestBox()); }
        if (Input.GetKeyDown(KeyCode.W)) { gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_GridMap()); }
        if (Input.GetKeyDown(KeyCode.E)) { gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_ZoneOne()); }
        if (Input.GetKeyDown(KeyCode.R)) { gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_ZoneTwo()); }

            //if (Input.GetKeyDown(KeyCode.Delete))
            //{
            //    PersistoMatic[] objects = (PersistoMatic[])GameObject.FindObjectsOfType(typeof(PersistoMatic));
            //    foreach (PersistoMatic obj in objects)
            //    {
            //        obj.SendMessage("OnRemove");
            //    }
            //}

            //if (Input.GetKeyDown(KeyCode.K)) { worldManager.CreateHotspot(); }

            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    GameObject focusObject = GazeManager.Instance.HitObject;
            //    if (focusObject != null) { focusObject.SendMessage("OnRemove"); }
            //}


        }

    void Update()
    {
       EditorKeyboardInputs();
    }

    // Use this for initialization
    void Start()
    {
        


        keywords.Add("Test box", () =>
        {
            gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_TestBox());
        });

        keywords.Add("Grid map", () =>
        {
            gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_GridMap());
        });

        keywords.Add("Zone one", () =>
        {
            gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_ZoneOne());
        });

        keywords.Add("Zone two", () =>
        {
            gameEditorManager.PlaceEditorObject(GameSettings.Instance.GetAnchorName_ZoneTwo());
        });

        //keywords.Add("Remove", () =>
        //{
        //    var focusObject = GazeManager.Instance.HitObject;
        //    if (focusObject != null)
        //    {
        //        // Call the OnDrop method on just the focused object.
        //        focusObject.SendMessage("OnRemove");
        //    }
        //});

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