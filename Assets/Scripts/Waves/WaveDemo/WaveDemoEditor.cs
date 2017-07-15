// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WaveDemoEditor : WaveEditor {

    public GameObject spawnToggleObject;
    public GameObject dummySpawnToggleObject;
    public GameObject walkieTalkieToggleObject;
    public GameObject infiniteAmmoBoxToggleObject;
    public GameObject pathFinderToggleObject;
    public GameObject airstrikeStartToggleObject;
    public GameObject airstrikeEndToggleObject;

    Toggle spawnToggle;
    Toggle dummySpawnToggle;
    Toggle walkieTalkieToggle;
    Toggle infiniteAmmoBoxToggle;
    Toggle pathFinderToggle;
    Toggle airstrikeStartToggle;
    Toggle airstrikeEndToggle;

    WaveDemoData data;

    protected override void Start()
    {
        base.Start();
        spawnToggle = spawnToggleObject.GetComponent<Toggle>();
        dummySpawnToggle = dummySpawnToggleObject.GetComponent<Toggle>();
        walkieTalkieToggle = walkieTalkieToggleObject.GetComponent<Toggle>();
        infiniteAmmoBoxToggle = infiniteAmmoBoxToggleObject.GetComponent<Toggle>();
        pathFinderToggle = pathFinderToggleObject.GetComponent<Toggle>();
        airstrikeEndToggle = airstrikeEndToggleObject.GetComponent<Toggle>();
        airstrikeStartToggle = airstrikeStartToggleObject.GetComponent<Toggle>();
    }

    public float spawnFrequency
    {
        get
        {
            return data.spawnFrequency;
        }

        set
        {
            if (value > 1.0f)
                data.spawnFrequency = value;
        }
    }

    public override void LoadWaveSettings()
    {
        data = new WaveDemoData(fileName);        
    }

    public override void SaveWaveSettings()
    {
        data.Save();
    }

    public override void UpdateGUI()
    {
        if (worldManager.GetSpawnCount() > 0)
            spawnToggle.isOn = true;
        else
            spawnToggle.isOn = false;

        if (worldManager.GetDummySpawnCount() > 0)
            dummySpawnToggle.isOn = true;
        else
            dummySpawnToggle.isOn = false;

        if (worldManager.GetWalkieTalkieCount() > 0)
            walkieTalkieToggle.isOn = true;
        else
            walkieTalkieToggle.isOn = false;

        if (worldManager.GetInfiniteAmmoBoxCount() > 0)
            infiniteAmmoBoxToggle.isOn = true;
        else
            infiniteAmmoBoxToggle.isOn = false;

        if (worldManager.isPathFinderPlaced())
            pathFinderToggle.isOn = true;
        else
            pathFinderToggle.isOn = false;

        if (worldManager.isAirstrikeEndPlaced())
            airstrikeEndToggle.isOn = true;
        else
            airstrikeEndToggle.isOn = false;

        if (worldManager.isAirstrikeStartPlaced())
            airstrikeStartToggle.isOn = true;
        else
            airstrikeStartToggle.isOn = false;
    }

    public void UpdateSpawnFrequency(float value)
    {
        spawnFrequency = value;
    }
}