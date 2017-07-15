// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WaveDemo : MonoBehaviour, IWave {

    enum States {
        ASLEEP,
        STARTING,
        SPAWN_DUMMY,
        TEACH_RELOAD,
        GUN_RELOADED,
        ALPHA_SQUAD,
        AIRSTRIKE,
        COMPLETE
    };

    public GameObject dummyZombie;
    public GameObject[] zombies;
    public string fileName;

    States state = States.ASLEEP;

    GameManager manager;

    int reloadCount = 0;
    bool outOfAmmoPlayed = false;

    string filePath;

    WaveDemoData data;

    PlayerGun playerGun;
    WalkieTalkie walkieTalkie;
    List<GameObject> spawnPoints;
    List<GameObject> dummySpawnPoints;
    GameObject dz;

    // Use this for initialization
    void Start () {
        data = new WaveDemoData(fileName);
    }

    // Update is called once per frame
    void Update () {
	    
	}

    public string GetFileName()
    {
        return fileName;
    }

    public void StartWave()
    {
        if (playerGun == null)
            playerGun = FindObjectOfType<PlayerGun>();
        if (manager == null)
            manager = FindObjectOfType<GameManager>();
        if (walkieTalkie == null)
            walkieTalkie = FindObjectOfType<WalkieTalkie>();
       
        spawnPoints = manager.GetSpawnPoints();
        dummySpawnPoints = manager.GetDummySpawnPoints();

        state = States.STARTING;
        //playerGun.SetRounds(0);
        outOfAmmoPlayed = false;
        reloadCount = 0;

        walkieTalkie.audioManager.StopAllEvents();
        walkieTalkie.audioManager.PlayEvent("_Bravo");
    }

    public void OnReload()
    {
        reloadCount++;
        if (state == States.TEACH_RELOAD || state == States.SPAWN_DUMMY)
            GunReloaded();
    }

    public void OnOutOfAmmo()
    {
        if (state == States.SPAWN_DUMMY && walkieTalkie.GetComponent<AudioWatcher>().canPlay)
            TeachReload();
        else if (reloadCount == 1 && walkieTalkie.audioWatcher.canPlay && !outOfAmmoPlayed)
        {
            walkieTalkie.audioManager.PlayEvent("_OutOfAmmo");
            outOfAmmoPlayed = true;
        }
    }

    public void OnKill(GameObject enemy)
    {
        if (state == States.SPAWN_DUMMY || state == States.GUN_RELOADED)
            AlphaSquad();
    }

    public void OnTouchObject(GameObject touched)
    {
        if (state == States.STARTING)
        {
            SpawnDummy();
        }
    }

    public void OnTrigger(Collider c)
    {

    }

    public void OnGameOver()
    {

    }

    public void OnComplete()
    {
        walkieTalkie.audioManager.StopAllEvents();
        walkieTalkie.audioWatcher.SendMessage("Playing");
        walkieTalkie.audioManager.PlayEvent("_GreenZone");
        state = States.COMPLETE;
        //manager.WaveComplete();
    }

    void SpawnDummy()
    {
        state = States.SPAWN_DUMMY;

        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(10.0f, CheckProgress);
        
        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(20.0f, AirStrikeInbound);

        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(45.0f, IncreaseSpawnRate);

        //t = gameObject.AddComponent<TimerBehaviour>();
        //t.StartTimer(60.0f, AirStrikeWarning);

        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(55.0f, PlayMusic);

        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(60.0f, AirStrikeIminent);

        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(90.0f, RestartDemo);

        walkieTalkie.audioManager.StopAllEvents();
        walkieTalkie.gameObject.SendMessage("Playing");
        walkieTalkie.audioManager.PlayEvent("_NeedAssistance", walkieTalkie.gameObject, "Done");
        int spawnNum = UnityEngine.Random.Range(0, dummySpawnPoints.Count - 1);
        Debug.Log("spawnNum = " + spawnNum);

        GameObject spawnChoice = dummySpawnPoints[spawnNum];

        if (spawnChoice != null)
        {
            dz = manager.CreateEnemy(spawnChoice, dummyZombie);
        }
    }

    void TeachReload()
    {
        state = States.TEACH_RELOAD;
        walkieTalkie.audioManager.PlayEvent("_TeachReload");
    }

    void GunReloaded()
    {
        state = States.GUN_RELOADED;
        dz.GetComponent<ZombieBehavior>().Activate();
    }

    void CheckProgress()
    {
        if (dz != null)
        {
            if (dz.GetComponent<ZombieBehavior>().state == ZombieState.IDLE)
            {
                dz.GetComponent<ZombieBehavior>().Activate();
                AlphaSquad();
            }
        }
    }

    void AlphaSquad()
    {
        state = States.ALPHA_SQUAD;
        walkieTalkie.audioManager.StopAllEvents();
        walkieTalkie.gameObject.SendMessage("Playing");
        walkieTalkie.audioManager.PlayEvent("_MayDay", walkieTalkie.gameObject, "Done");
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(3.0f, StartContinuousSpawn);
    }

    void StartContinuousSpawn()
    {
        foreach (GameObject spawn in spawnPoints)
        {
            //spawn.GetComponent<SpawnPoint>().StartSpawning(data.spawnFrequency, zombies);
        }
    }

    void AirStrikeInbound()
    {
        walkieTalkie.audioWatcher.PlayEvent("_AirstrikeInbound");
    }

    void AirStrikeWarning()
    {
        walkieTalkie.audioWatcher.PlayEvent("_AirstrikeWarning");
    }

    void IncreaseSpawnRate()
    {
        // increase spawn rate by 40%
        foreach (GameObject spawn in spawnPoints)
        {
            //spawn.GetComponent<SpawnPoint>().StartSpawning(data.spawnFrequency * 0.6f, zombies);
        }
    }

    void AirStrikeIminent()
    {
        walkieTalkie.audioManager.StopAllEvents();
        walkieTalkie.gameObject.SendMessage("Playing");
        walkieTalkie.audioManager.PlayEvent("_AirstrikeIminent", walkieTalkie.gameObject, "Done");
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(2.0f, AirStrikeHit);
    }

    void AirStrikeHit()
    {
        foreach (GameObject spawn in spawnPoints)
        {
            spawn.SendMessage("StopSpawning");
        }
        AirStrike airstrike = FindObjectOfType<AirStrike>();
        airstrike.StartAirStrike();
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(8.0f, OnComplete);
    }

    void PlayMusic()
    {
        Camera.main.SendMessage("PlayMusic");
    }

    void RestartDemo()
    {
        //manager.LoadScene("Demo");
        Camera.main.SendMessage("StopMusic");
        manager.ClearEnemies();
        manager.ResetScore();
        //manager.ChangeWave();
    }
}
