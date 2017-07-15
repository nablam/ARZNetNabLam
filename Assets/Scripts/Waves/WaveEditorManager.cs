// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class WaveEditorManager : MonoBehaviour {

    public GameObject worldManagerObject;   // the world manager object in scene
    public float guiDistance;               // the distance from the camera where the gui appears
    public GameObject[] waveEditPrefabs;    // array of editable waves prefab objects
   
    GameObject currentWaveEditorObject;     // the current wave edit object loaded in the scene
    WaveEditor currentWaveEditor;           // the wave edit script component attached to the wave edit object
    WorldManager worldManager;              // the world manager object script
    int currentWaveIdx = 0;                 // the index of the current wave prefab loaded in the scene
    
    // Use this for initialization
    void Start () {
        worldManager = worldManagerObject.GetComponent<WorldManager>();
        Debug.Log("WaveEditormanager is on " + gameObject.name);
        if (waveEditPrefabs.Length > 0)
        {
            LoadNewWaveObject(currentWaveIdx);
            currentWaveEditor.HideGUI();
            //worldManager.WaveLoaded();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject[] GetWaves()
    {
        return waveEditPrefabs;
    }

    public WaveEditor GetWaveEditor()
    {
        return currentWaveEditor;
    }

    public void LoadNewWaveObject(int index)
    {
        currentWaveEditorObject = Instantiate(waveEditPrefabs[currentWaveIdx]) as GameObject;
        currentWaveEditor = currentWaveEditorObject.GetComponent<WaveEditor>();
        currentWaveEditor.SetWaveEditorManager(this);
        currentWaveEditor.SetWorldManager(worldManager);
    }

    public void ChangeWave(int value)
    {
        if (waveEditPrefabs.Length <= value)
            return;

        Destroy(currentWaveEditorObject);
        currentWaveIdx = value;
        LoadNewWaveObject(currentWaveIdx);
        currentWaveEditor.ShowGUI(guiDistance);
        //worldManager.ChangeWave();
    }

    public void UpdateEditorGUI()
    {
        currentWaveEditor.UpdateGUI();
    }

    public void ShowGUI()
    {
        currentWaveEditor.ShowGUI(guiDistance);
    }
}
