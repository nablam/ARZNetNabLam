// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class WaveEditor : MonoBehaviour {

    public GameObject gui;                              // Wave Editor menu
    public string fileName;                             // Name of file (minus extension)
    public GameObject waveDropDownMenuObject;           // drop down menu on GUI for wave selection

    protected WorldManager worldManager;                // World Manager script attached to worldManagerObject
    protected WaveEditorManager waveEditorManager;      // Wave Edit Manager that instantiated this WaveEdit
    protected string filePath;                          // complete path including file name and extension
    protected string fileExtension = ".wcfg";           // extension for wave settings file (wave config)
    protected bool waveLoaded = false;                  // flag to set when wave is loaded

    protected virtual void Start()
    {
        filePath = Application.persistentDataPath + fileName + (fileName.Contains(fileExtension) ? "" : fileExtension);

        // setup canvas
        gui.GetComponent<Canvas>().worldCamera = Camera.main;

        // setup dropdown menu
        GameObject[] waves = waveEditorManager.GetWaves();
        Dropdown ddMenu = waveDropDownMenuObject.GetComponent<Dropdown>();
        ddMenu.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < waves.Length; i++)
        {
            options.Add(waves[i].name);
        }
        ddMenu.AddOptions(options);

        LoadWaveSettings();
    }

    public void SetWaveEditorManager(WaveEditorManager manager)
    {
        waveEditorManager = manager;
    }

    public void SetWorldManager(WorldManager manager)
    {
        worldManager = manager;
    }

    public void ShowGUI(float guiDistance)
    {
        // place menu at correct orientation to camera and display gui
        gameObject.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * guiDistance);
        gameObject.transform.rotation = Camera.main.transform.rotation;
        gui.SetActive(true);
    }

    public void HideGUI()
    {
        gui.SetActive(false);
    }

    public void OK()
    {
        // save data and hide window
        SaveWaveSettings();
        HideGUI();
    }

    public void ChangeWave(int value)
    {
        waveEditorManager.ChangeWave(value);
    }

    virtual public void LoadWaveSettings()
    {

    }

    virtual public void SaveWaveSettings()
    {

    }

    virtual public void UpdateGUI()
    {

    }

    void OnDestroy()
    {
        SaveWaveSettings();
    }
}