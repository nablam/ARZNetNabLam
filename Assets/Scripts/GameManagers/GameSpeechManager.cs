// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GameSpeechManager : MonoBehaviour{
    public GameObject gameManagerObject;
    public GameObject playerGun;

    GameManager gameManager;
    PlayerGun gun;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void keyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.G)) { gameManager.LoadScene("MainMenu"); }
        if (Input.GetKeyDown(KeyCode.H)) { gameManager.LoadScene("Game_NoStem"); }
        if (Input.GetKeyDown(KeyCode.Backslash)) { gameManager.LoadScene("GameShort"); }
        if (Input.GetKeyDown(KeyCode.Slash)) { gameManager.LoadScene("GameApocalypse"); }
        if (Input.GetKeyDown(KeyCode.J)) { GameManager.Instance.CheckStartGame(); }     
    }
    void Update()
    {
        keyboardInputs();
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.Instance;

        if (playerGun != null)
            gun = playerGun.GetComponent<PlayerGun>();

        keywords.Add("Total Respawn Admin Exit Game", () =>
        {
            // load main menu scene
            gameManager.LoadScene("MainMenu");
        });

        keywords.Add("Reload", () =>
        {
            GameObject focused = GazeManager.Instance.HitObject;
            if (focused.CompareTag("Ammo"))
            {
                if (Vector3.Distance(Camera.main.transform.position, focused.transform.position) <= 1.25)
                {
                    focused.SendMessage("Take");
                    if (gun != null)
                        gun.Reload();
                }
            }
        });

        keywords.Add("Survivor Ready", () =>
        {
            GameManager.Instance.CheckStartGame();
        });

        keywords.Add("Total Respawn Admin Reset Game", () =>
        {
            gameManager.LoadScene("Game");
        });


        keywords.Add("Total Respawn Admin Reset Game no stem", () =>
        {
            gameManager.LoadScene("Game_NoStem");
        });

        keywords.Add("Total Respawn Admin Reset Apocalypse", () =>
        {
            gameManager.LoadScene("GameApocalypse");
        });
        
        keywords.Add("Total Respawn Admin Reset Short game", () =>
        {
            gameManager.LoadScene("GameShort");
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

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Dispose();
        }
    }
}
