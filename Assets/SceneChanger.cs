using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    string localScenename;
    private void Start()
    {
        localScenename= SceneManager.GetActiveScene().name;
    }
    public void LoadScene(string argSceneName)
    {
        

        if (string.Compare(argSceneName, localScenename) == 0) {
            return;
        }
    
        SceneManager.LoadScene(argSceneName);
    }

}
