// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This standard wave spawns zombies for a certain amount of time, keeping no more
 * than x number of zombies on screen at any time. It uses y percent of all
 * spawn points. Bonus points are awarded for destroying more zombies than the alloted par
 */
public class WaveStandard : MonoBehaviour, IWave {

    [Tooltip("Total time this wave will be active.")]
    public float WaveTimerInMinutes;

    [Tooltip("Number of living zombies allowed at any one time.")]
    public int maxZombiesOnScreen;

    [Tooltip("The par number of kills to complete the wave.")]
    public int parKills;

    [Tooltip("Percentage of spawn points used. Range 0.0 - 1.0")]
    public float percentageOfSpawns;

    [Tooltip("Buffer before the same spawn can respawn a zombie")]
    public float spawnDelay = 2.0f;

    [Tooltip("The choice of zombies to spawn from.")]
    public GameObject[] zombies;

    [Tooltip("Hitpoint range of zombies this wave.")]
    public int minZombieHP, maxZombieHP;

    private GameManager manager;            // reference to game manager
    private List<SpawnPoint> spawnPoints;   // reference to all spawn points used by this wave

    private int maxSpawnPointIndex;         // the index of the closest spawn used by this wave
    private int numberOfZombies = 0;        // number of zombies living
    private int zombiesKilled = 0;          // number of zombies killed
    private bool timesUp = false;           // is the wave over?

	void Start ()
    {
        manager = GameManager.Instance;

        initSpawnPoints();
    }

    private void initSpawnPoints()
    {
        List<GameObject> spawns = manager.GetSpawnPoints();
        spawnPoints = new List<SpawnPoint>();

        // calculate max spawn point index used by this array
        if (percentageOfSpawns >= 0.0f && percentageOfSpawns <= 1.0f)
            maxSpawnPointIndex = (int)((spawns.Count) * percentageOfSpawns);
        else
            maxSpawnPointIndex = spawns.Count;

        for (int i=0; i<maxSpawnPointIndex; i++)
        {
            SpawnPoint spawnPoint = spawns[i].GetComponent<SpawnPoint>();
            spawnPoint.Init(this, spawnDelay);
            spawnPoints.Add(spawnPoint);
        }
    }


    public void ResetSpawnStates() {
       

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.ToggleSpawning(false);           
        }
    }
    private void toggleSpawning(bool toggle)
    {
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.ToggleSpawning(toggle);
        }
    }

    /**
     * Checks Spawns to see if any can spawn a zombie.
     **/
    public void AttemptZombieSpawn()
    {
        if (numberOfZombies >= maxZombiesOnScreen || timesUp || GameManager.Instance.isDead)
            return;
        List<SpawnPoint> AllSpawns = new List<SpawnPoint>();
        
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if (!spawnPoint.IsLocked)
            {
                // spawnZombie(spawnPoint);
                // spawnPoint.ToggleSpawning(true);
                //return;
                AllSpawns.Add(spawnPoint);
            }
        }

        if (AllSpawns.Count == 0) return;

        int i = Random.Range(0, AllSpawns.Count);
        spawnZombie(AllSpawns[i]);
        AllSpawns[i].ToggleSpawning(true);
    }


   

    private void spawnZombie(SpawnPoint spawnPoint)
    {
        int randZombieIndex = Random.Range(0, zombies.Length);
        int randZombieHP = Random.Range(minZombieHP, maxZombieHP);

        GameObject z = manager.CreateEnemy(spawnPoint.gameObject, zombies[randZombieIndex]);
        ZombieBehavior zombie = z.GetComponent<ZombieBehavior>();

        zombie.SetHP(randZombieHP);
        numberOfZombies++;
    }

    /**
     * Called when wave starts
     */
    public void StartWave()
    {
        StartCoroutine(ie_StartWaveTimer());
        toggleSpawning(true);
    }

    private IEnumerator ie_StartWaveTimer()
    {
        yield return new WaitForSeconds(WaveTimerInMinutes * 60);

        timesUp = true;
      //  toggleSpawning(false);
        if (numberOfZombies <= 0)
        {
            OnComplete();
        }
    }

    public void OnReload()
    {

    }

    public void OnOutOfAmmo()
    {

    }

    public void OnKill(GameObject enemy)
    {
        numberOfZombies--;
        zombiesKilled++;

        if (numberOfZombies <= 0 && timesUp)
        {
            OnComplete();
            return;
        }

        AttemptZombieSpawn();
    }

    public void OnTouchObject(GameObject touched)
    {

    }

    public void OnTrigger(Collider c)
    {

    }

    public void OnGameOver()
    {

    }

    public void OnComplete()
    {

        toggleSpawning(false);
        // tell the game manager that wave is completed
        manager.WaveComplete(parKills, zombiesKilled);
    }
}
