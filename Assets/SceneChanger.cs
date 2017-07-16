using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void LoadScene(string argSceneName)
    {
        string CurSceneName = SceneManager.GetActiveScene().name;

        if (string.Compare(argSceneName, CurSceneName) == 0) {
            return;
        }
    
        SceneManager.LoadScene(argSceneName);
    }

}
