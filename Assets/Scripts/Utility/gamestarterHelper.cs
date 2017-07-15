using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class gamestarterHelper : MonoBehaviour {

    GameManager gameManager;
    void keyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.M)) { SceneManager.LoadScene("MainMenu"); }
        if (Input.GetKeyDown(KeyCode.S)) { SceneManager.LoadScene("ScanRoom"); }
        if (Input.GetKeyDown(KeyCode.E)) { SceneManager.LoadScene("EditMap"); }
        if (Input.GetKeyDown(KeyCode.G)) { SceneManager.LoadScene("Game_NoStem"); }
        if (Input.GetKeyDown(KeyCode.H)) { SceneManager.LoadScene("Game"); }
        if (Input.GetKeyDown(KeyCode.I)) { SceneManager.LoadScene("GameShort"); }
        if (Input.GetKeyDown(KeyCode.Backslash)) { SceneManager.LoadScene("GameShort"); }
        if (Input.GetKeyDown(KeyCode.Slash)) { gameManager.LoadScene("GameApocalypse"); }
    }
    void Start () {
        gameManager = GameManager.Instance;
    }

    void Update () {
        keyboardInputs();
    }
}
