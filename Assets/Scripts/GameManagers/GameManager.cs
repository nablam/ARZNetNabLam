// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameSettings Settings;
    /// <summary>
    /// TODO:
    /// make Gamesettings static or holotoolkit.singelton
    /// </summary>

    public GameObject gridMap;
    string AnchorName_PathFinder;
    public GameObject stemSystem;
    string AnchorName_StemBase;
    public GameObject consoleObject;
    string AnchorName_ConsoleObject;
    public GameObject spawnPoint;
    string AnchorName_SpawnPoint;
    public GameObject hotspot;
    string AnchorName_HotSpot;
    public GameObject dummySpawnPoint;
    string AnchorName_SpawnPointDummy;
    public GameObject barrier;
    string AnchorName_Barrier;
    public GameObject scoreboard;
    string AnchorName_ScoreBoard;
    public GameObject mag;
    string AnchorName_PistoleMag;
    public GameObject ammoBox;
    string AnchorName_AmmoBox;
    public GameObject infiniteAmmoBox;
    string AnchorName_AmmoBoxInfinite;
    public GameObject walkieTalkie;
    string AnchorName_WalkieTalkie;
    public GameObject mist;
    string AnchorName_MistEmitter;
    string AnchorName_MistEnd;
    GameObject _placeHolderMistTarget;
    string AnchorName_AirStrikeStart;     
    public GameObject airstrike;
    string AnchorName_AirStrikeEnd;
    public GameObject placeholder;
    string AnchorName_WeaponRack;
    public GameObject weaponsRack;

    public GameObject playerGun;
    public GameObject reticle;
    public GameObject youDiedScreen;
    public GameObject gameOverScreen;
    public GameObject gameCanvasObject;
    public GameObject gameCountDownScreen;


    public GameObject magnumPickupPrefab;
    public GameObject uziPickupPrefab;
    public GameObject shotGunPickupPrefab;

    [Tooltip("Starting drop chance for special weapons (0-1)")]
    public float baseDropChance;
    [Tooltip("The chance that gets added to drop chance until a weapon drops, at which time it resets to base (0-1)")]
    public float dropChanceIncrease;

    [Tooltip("Setting thos value will show gridpoints as visible cubes")]
    public bool isTestMode;

    [Tooltip("Master Game Timer")]
    float MasterTime;//=66f; //15 sec
    public float GameDurationInSeconds;


    [Tooltip("Setting this value will allow 1 headshot kill")]
    public bool isHeadShotKill;

    [Tooltip("Setting this value for Apocalypse mode")]
    public bool isApocalypse;


    int enemyCorpseLimit = 3;

    [HideInInspector]
    public bool isDead { get; private set; }
    public PlayerBehavior player { get; private set; }
    public int points { get; private set; }
    int wavePoints;

 
    public int shotCount { get; private set; }
    public int headShotCount { get; private set; }
    public int torsoShotCount { get; private set; }
    public int limbShotCount { get; private set; }
    public int targetHitCount { get; private set; }
    public int killCount { get; private set; }
    public int enemyCreatedCount { get; private set; }


    public delegate void TriggerHandler(int argNum);
    public static TriggerHandler CountDownHandler;
    public void call_CountDownAudioVideo(int argNum) { if (NewMethod()) { CountDownHandler(argNum); } }

    private bool NewMethod()
    {
        return CountDownHandler != null;
    }

    WaveManager waveManager;
    GameCanvas gameCanvas;
    GridMap map;

    bool isLevelLoaded = false;
    bool isRoomLoaded = false;
    bool isGridBuilt = false;
    bool isGameStarted = false;
    bool gameTimeIsUp = false;

    List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> dummySpawnPoints = new List<GameObject>();
    List<GameObject> barriers = new List<GameObject>();
    List<GameObject> mags = new List<GameObject>();
    List<GameObject> ammoBoxes = new List<GameObject>();
    List<GameObject> walkieTalkies = new List<GameObject>();
    List<GameObject> mists = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> hotspots = new List<GameObject>();
    Queue<GameObject> deadEnemies = new Queue<GameObject>();
    WeaponRack rack;

    WorldAnchorStore anchorStore;

    int prevDropSelection = -1;
    float dropChance;

    void InitAnchorNameVariables()
    {

        AnchorName_ConsoleObject = Settings.GetAnchorName_ConsoleObject();

        AnchorName_StemBase = Settings.GetAnchorName_StemBase();

        AnchorName_SpawnPoint = Settings.GetAnchorName_SpawnPoint();

        AnchorName_SpawnPointDummy = Settings.GetAnchorName_SpawnPointDummy();

        AnchorName_Barrier = Settings.GetAnchorName_Barrier();

        AnchorName_ScoreBoard = Settings.GetAnchorName_ScoreBoard();

        AnchorName_WeaponRack = Settings.GetAnchorName_WeaponRack();

        AnchorName_PistoleMag = Settings.GetAnchorName_PistoleMag();

        AnchorName_AmmoBox = Settings.GetAnchorName_AmmoBox();

        AnchorName_AmmoBoxInfinite = Settings.GetAnchorName_AmmoBoxInfinite();

        AnchorName_PathFinder = Settings.GetAnchorName_PathFinder();

        AnchorName_WalkieTalkie = Settings.GetAnchorName_WalkieTalkie();

        AnchorName_MistEmitter = Settings.GetAnchorName_MistEmitter();

        AnchorName_MistEnd = Settings.GetAnchorName_MistEnd();

        AnchorName_HotSpot = Settings.GetAnchorName_HotSpot();
    }
    public float GetMasterTime() { return MasterTime; }
    private void Awake()
    {
        InitAnchorNameVariables();
        MasterTime = GameDurationInSeconds;// 780f;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        gameCanvas = gameCanvasObject.GetComponent<GameCanvas>();
        player = Camera.main.GetComponent<PlayerBehavior>();
        //Debug.Log(player.name);
        dropChance = baseDropChance;
        ResetScore();
    }

    // Update is called once per frame
   // void Update()
    //{
        //CheckStartGame();
        //CheckGameOver();
       // CheckCorpseCount(); <-- no need to check on evry frame, just when zombie dies
    //}

    public bool IsGameStarted()
    {
        return isGameStarted;
    }
 
    /// <summary>
    /// points = 0;
    /// wavePoints = 0;
    /// shotCount = 0;
    /// headShotCount = 0;
    /// torsoShotCount = 0;
    /// limbShotCount = 0;
    /// targetHitCount = 0;
    /// killCount = 0;
    /// enemiesCreatedCount = 0;
    /// </summary>
    /// <param name="argCounterName"></param>
    public void IncrementCounter(int argCounterName) {

        if (!isGameStarted)
            return;

        switch (argCounterName)
        {
            case 0:
                points++;
                break;
            case 1:
                wavePoints++;
                break;
            case 2:
                shotCount++;
                break;
            case 3:
                headShotCount++;
                targetHitCount++;
                break;
            case 4:
                torsoShotCount++;
                targetHitCount++;
                break;
            case 5:
                limbShotCount++;
                targetHitCount++;
                break;
            case 6:
                killCount++;
                break;
            default:
                break;

        }
    }

    public void DecrementCounter()
    {
        if(shotCount>0)
        shotCount--;
    }
    private void OnDestroy()
    {
        // clear instance variable if gamemanager is destroyed
        if (Instance == this)
            Instance = null;
    }

    public void AttackPlayer()
    {
        gameCanvas.TakeHit();
    }

    public void KillePlayer()
    {
        gameCanvas.KillePlayer();
    }

    public void AwardWeapon(GunType gun)
    {
        if (rack == null)
            return;

        switch (gun)
        {
            case GunType.MAGNUM:
                rack.AwardMagnum();
                break;
            case GunType.UZI:
                rack.AwardUzi();
                break;
            case GunType.SHOTGUN:
                rack.AwardShotgun();
                break;
        }
    }

    public List<GameObject> GetSpawnPoints()
    {
        return spawnPoints;
    }

    public List<GameObject> GetDummySpawnPoints()
    {
        return dummySpawnPoints;
    }

    public List<GameObject> GetBarriers()
    {
        return barriers;
    }

    public List<GameObject> GetHotspots()
    {
        return hotspots;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public bool IsLevelLoaded()
    {
        return isLevelLoaded;
    }

    public bool IsRoomLoaded()
    {
        return isRoomLoaded;
    }

    public bool IsGridBuilt()
    {
        return isGridBuilt;
    }

    public GridMap GetGridMap()
    {
        return map;
    }

    public void RoomLoaded()
    {
        isRoomLoaded = true;
        if (waveManager == null)
            waveManager = FindObjectOfType<WaveManager>();

        if (waveManager.isWaveLoaded)
            WorldAnchorStore.GetAsync(AnchorStoreReady);
        else
        {
            // if wave is not loaded check back in a half second
            TimerBehavior t = new TimerBehavior();
            t.StartTimer(0.5f, RoomLoaded);
        }
    }

    public void GridBuilt(GridMap map)
    {
        this.map = map;
        isGridBuilt = true;
    }

    public void LevelLoaded()
    {
        isLevelLoaded = true;

        // sort spawn points by distance
        SortSpawnPoints();

        if (!isGridBuilt)
        {
            CreateGrid();
        }
            
    }

    private void SortSpawnPoints()
    {
        spawnPoints.Sort(RankSpawnPointsByDistance);
    }

    private void CreateGrid()
    {
        GameObject.FindObjectOfType<GridMap>().CreateGrid(!isTestMode);
    }

    public GameObject CreateEnemy(GameObject spawnPoint, GameObject enemy)
    {
        GameObject e = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity) as GameObject;
        enemies.Add(e);
        enemyCreatedCount++;
        ClearAllToWhite();    
        return e;
    }


    public void ClearAllToWhite()
    {
        if (isTestMode) {
            foreach (GameObject p in map.GetGridMap())
             {
             p.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }


   
    public void KillEnemy(GameObject obj)
    {        
        enemies.Remove(obj);      
        deadEnemies.Enqueue(obj);
        waveManager.OnKill(obj);
        SpecialDrop(obj);
        CheckCorpseCount();
    }

    public void AddPoints(int value)
    {
        points += value;
        wavePoints += value;
    }

    public void ResetScore()
    {
        points = 0;
        wavePoints = 0;
        shotCount = 0;
        headShotCount = 0;
        torsoShotCount = 0;
        limbShotCount = 0;
        targetHitCount = 0;
        killCount = 0;
        enemyCreatedCount = 0;
    }

    public void TimesUp()
    {
        gameTimeIsUp = true;
    }
    public void GameOver_GameManager()
    {
        isDead = true;

        // turn off gun and reticle
        if (reticle != null)
            reticle.SetActive(false);

        if (playerGun != null)
            playerGun.SetActive(false);

       

        // pause all enemies
        foreach (GameObject g in enemies){
            g.GetComponent<ZombieBehavior>().Pause();
        }

        if (gameTimeIsUp||isApocalypse)
        {
            // game over
            gameCanvas.FinalScore(points);
            gameOverScreen.SetActive(true);
            // tell wave manager whether or not to reload wave via if time is up
            waveManager.OnGameOver_WaveManager(true); 
        }
        else
        {
            // you died

            // display points lost
            gameCanvas.PointsLost(wavePoints);

            // remove all points earned this wave and reset wave points
            points -= wavePoints;
            wavePoints = 0;

            //enable you died screen
            youDiedScreen.SetActive(true);

            // tell wave manager whether or not to reload wave via if time is up
            waveManager.OnGameOver_WaveManager(false); 
        }

        gameCanvas.PlayGameOverAudio();
    }


    public void HardStop()
    {
       // isDead = true;

        // turn off gun and reticle
        if (reticle != null)
            reticle.SetActive(false);

        if (playerGun != null)
            playerGun.SetActive(false);

        // tell wave manager whether or not to reload wave via if time is up
        waveManager.OnGameOver_WaveManager(true);

        // pause all enemies
        foreach (GameObject g in enemies)
        {
            g.GetComponent<ZombieBehavior>().Pause();
        }
      
       // game over
        gameCanvas.FinalScore(points);
       gameOverScreen.SetActive(true);
        gameCanvas.PlayGameOverAudio();   
    }

    public void ResetWave()
    {
        // destroy all enemies
        foreach (GameObject g in enemies)
        {
            Destroy(g);
        }
        enemies = new List<GameObject>();

        foreach(ZombieBehavior z in GameObject.FindObjectsOfType<ZombieBehavior>())
        {
            Destroy(z.gameObject);
        }

        // get rid of blood splatters
        gameCanvas.ResetDamage();

        // turn on gun and reticle
        if (reticle != null)
            reticle.SetActive(true);

        if (playerGun != null)
            playerGun.SetActive(true);

        isDead = false;

        //disable game over tag along screen
        youDiedScreen.SetActive(false);

        WaveStarted();
    }

    public void SetActiveInput(bool isActive)
    {
        if (reticle != null)
            reticle.gameObject.SetActive(isActive);
        if (playerGun != null)
            playerGun.gameObject.SetActive(isActive);
    }

    public void WaveComplete(int parKills, int kills)
    {
        // reset wave points
        wavePoints = 0;

        // calculate and award bonus points
        int bonus;
        if (parKills > kills)
            bonus = 0;
        else if (parKills == kills)
            bonus = 1000;
        else
            bonus = 1000 + (kills - parKills) * 100;

        AddPoints(bonus);

        // activate canvas spash
        gameCanvas.WaveComplete(parKills, kills, bonus);

        // load next wave and launch in 10 seconds
        waveManager.WaveCompleted_soPopANewOne();

        // activate next wave splash in 6 seconds
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(6.0f, WaveStarted);
    }

    public void WaveStarted()
    {
        gameCanvas.WaveStarted(waveManager.GetWaveRomanNumeral());
    }

    public void CheckStartGame()
    {
        if (!isGameStarted)
        {
            if (isRoomLoaded && isGridBuilt && isLevelLoaded)
            {
                StartGame();
            }
        }
    }

    void CheckCorpseCount()
    {
        if (deadEnemies.Count > enemyCorpseLimit)
        {
            DestroyDeadEnemy();
        }
    }

    void StartGame()
    {
        DebugConsole.print("Gamestarted");
        if (mists.Count > 0) {
            DebugConsole.print("we got mists");
            MistMover mm= mists[0].gameObject.GetComponent<MistMover>();
            if (mm) {
                DebugConsole.print("we got mistmover");
                mm.StartMistMove(_placeHolderMistTarget.transform.position);
            }
        }

        isGameStarted = true;
        if (player.gridPosition == null)
            player.SetGridPosition();

        if(!isApocalypse)
        rack.StartGame();
        playerGun.GetComponent<PlayerGun>().StartGame();

        // begin first wave in 5 seconds
        waveManager.BeginNextWave(5.0f);
        WaveStarted();
    }

    void DestroyDeadEnemy()
    {
        //DebugConsole.print("gm sending MELT");
        GameObject enemy = deadEnemies.Dequeue();
        if (enemy != null) {
            ZombieBehavior zb = enemy.GetComponent<ZombieBehavior>();
            if (zb != null) {
                zb.Melt();
            }
        }
        //enemy.SendMessage("Melt");
    }

    void LoadObjects()
    {
        if (waveManager.GetWave() == null)
        {
            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(0.5f, LoadObjects);
            return;
        }

        // gather all stored anchors
        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            if (ids[index] == AnchorName_StemBase)
            {
                // if anchor is stem system
                // instantiate stem system prefab
                GameObject obj = Instantiate(stemSystem) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

                //obj.transform.Rotate(transform.up, 180.0f);
            }
            else if (ids[index] == AnchorName_ConsoleObject)
            {
                // if anchor is console object
                // instantiate console prefab
                GameObject obj = Instantiate(consoleObject) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index] == AnchorName_ScoreBoard)
            {
                // if anchor is scoreboard
                // instantiate scoreboard from anchor data
                GameObject obj = Instantiate(scoreboard) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

            }
            else if (ids[index] == AnchorName_WeaponRack)
            {
                // if anchor is weapons rack
                // instantiate weapons rack from anchor data
                GameObject obj = Instantiate(weaponsRack) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

                obj.transform.Rotate(Vector3.up, 180.0f);
                rack = obj.GetComponent<WeaponRack>();
            }
            else if (ids[index].Contains(AnchorName_WalkieTalkie))
            {
                // if anchor is walkie talkie
                // instantiate walkie talkie from anchor data
                GameObject obj = Instantiate(walkieTalkie) as GameObject;
                anchorStore.Load(ids[index], obj);

                walkieTalkies.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_Barrier))
            {
                // if anchor is barrier
                // instantiate barrier from anchor data
                GameObject obj = Instantiate(barrier) as GameObject;
                anchorStore.Load(ids[index], obj);

                barriers.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_AmmoBoxInfinite))
            {
                // if anchor is infinite ammo box
                // instantiate infinite ammo box from anchor data
                GameObject obj = Instantiate(infiniteAmmoBox) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index] == AnchorName_PathFinder)
            {
                // if anchor is pathfinder
                // instantiate pathfinder from anchor data
                GameObject obj = Instantiate(gridMap) as GameObject;
                anchorStore.Load(ids[index], obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index] == AnchorName_AirStrikeStart)
            {
                // if anchor is airstrike
                // locate airstrike object
                AirStrike airstrikeScript = FindObjectOfType<AirStrike>();

                // if none exist then instantiate one
                if (airstrikeScript == null)
                {
                    GameObject obj = Instantiate(airstrike) as GameObject;
                    airstrikeScript = obj.GetComponent<AirStrike>();
                }

                // instantiate placeholder at world anchor position
                GameObject placeholderObject = Instantiate(placeholder) as GameObject;
                anchorStore.Load(ids[index], placeholderObject);

                // delete anchor component
                WorldAnchor attachedAnchor = placeholderObject.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

                // add placeholder to airstrike object as air strike start
                airstrikeScript.SetAirStrikeStart(placeholderObject);
            }
            else if (ids[index] == AnchorName_AirStrikeEnd)
            {
                // if anchor is airstrike
                // locate airstrike object
                AirStrike airstrikeScript = FindObjectOfType<AirStrike>();

                // if none exist then instantiate one
                if (airstrikeScript == null)
                {
                    GameObject obj = Instantiate(airstrike) as GameObject;
                    airstrikeScript = obj.GetComponent<AirStrike>();
                }

                // instantiate placeholder at world anchor position
                GameObject placeholderObject = Instantiate(placeholder) as GameObject;
                anchorStore.Load(ids[index], placeholderObject);

                // delete anchor component
                WorldAnchor attachedAnchor = placeholderObject.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

                // add placeholder to airstrike object as air strike end
                airstrikeScript.SetAirStrikeEnd(placeholderObject);
            }
            else if (ids[index].Contains(AnchorName_SpawnPoint))
            {
                // if anchor is a spawn point
                // instantiate spawn point from anchor data
                GameObject obj = Instantiate(spawnPoint) as GameObject;
                anchorStore.Load(ids[index], obj);

                // add spawn point to collection
                spawnPoints.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_HotSpot))
            {
                // if anchor is a hotspot
                // instantiate hotspot from anchor data
                GameObject obj = Instantiate(hotspot) as GameObject;
                anchorStore.Load(ids[index], obj);

                // add spawn point to collection
                hotspots.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_SpawnPointDummy))
            {
                // instantiate spawn point from anchor data
                GameObject obj = Instantiate(dummySpawnPoint) as GameObject;
                anchorStore.Load(ids[index], obj);

                // add spawn point to collection
                dummySpawnPoints.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_PistoleMag))
            {
                // if anchor is mag
                // instantiate mag from anchor data
                GameObject obj = Instantiate(mag) as GameObject;
                anchorStore.Load(ids[index], obj);

                mags.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_AmmoBox))
            {
                // if anchor is ammo box
                // instantiate ammo box from anchor data
                GameObject obj = Instantiate(ammoBox) as GameObject;
                anchorStore.Load(ids[index], obj);

                ammoBoxes.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_MistEmitter))
            {
                // if anchor is mist
                // instantiate mist from anchor data
                GameObject obj = Instantiate(mist) as GameObject;
                anchorStore.Load(ids[index], obj);

                mists.Add(obj);

                // delete anchor component
                WorldAnchor attachedAnchor = obj.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);
            }
            else if (ids[index].Contains(AnchorName_MistEnd))
            {
                //MistMover MistMoverScript = FindObjectOfType<MistMover>();

                //// if none exist then instantiate one
                //if (MistMoverScript == null)
                //{
                //    GameObject obj = Instantiate(mist) as GameObject;
                //    MistMoverScript = obj.GetComponent<MistMover>();
                //}

                // instantiate placeholder at world anchor position
                _placeHolderMistTarget = Instantiate(placeholder) as GameObject;
                anchorStore.Load(ids[index], _placeHolderMistTarget);

                // delete anchor component
                WorldAnchor attachedAnchor = _placeHolderMistTarget.GetComponent<WorldAnchor>();
                if (attachedAnchor != null)
                    DestroyImmediate(attachedAnchor);

                // add placeholder to airstrike object as air strike end
                //MistMoverScript.SetMistEnd(placeholderObject);
            }
        }
        if (_placeHolderMistTarget == null)
        {
            DebugConsole.print("mist target ? more like missed target");
        }
 



        LevelLoaded();
    }


    public void ClearEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        foreach (GameObject dead in deadEnemies)
        {
            Destroy(dead);
        }
        deadEnemies.Clear();
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
        LoadObjects();
    }

    private static int RankSpawnPointsByDistance(GameObject x, GameObject y)
    {
        float xDistance = GetDistanceToCamera(x);
        float yDistance = GetDistanceToCamera(y);

        if (xDistance < yDistance)
        {
            return 1;
        }
        else if (xDistance > yDistance)
        {
            return -1;
        }
        else return 0;
    }

    private static float GetDistanceToCamera(GameObject o)
    {
        // gets horizontal distance from o to Camera.main
        return Vector2.Distance(new Vector2(o.transform.position.x, o.transform.position.z),
            new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z));
    }

    private void SpecialDrop(GameObject obj)
    {
        // calculate special drop chance
        float drop = Random.Range(0f, 1f);
        if (drop <= dropChance)
        {
            // drop special weapon
            int selection = Random.Range(0, 3);

            if (selection == prevDropSelection) selection++;
            if (selection >= 3) selection = 0;
            prevDropSelection = selection;

            switch (selection)
            {
                case 0:
                    // drop magnum
                    Instantiate(magnumPickupPrefab, obj.transform.position, Quaternion.identity);
                    break;
                case 1:
                    // drop uzi
                    Instantiate(uziPickupPrefab, obj.transform.position, Quaternion.identity);
                    break;
                case 2:
                    // drop shotgun
                    Instantiate(shotGunPickupPrefab, obj.transform.position, Quaternion.identity);
                    break;
            }

            // reset drop chance
            dropChance = baseDropChance;
        }
        else
        {
            // increase chance of drop on next zombie killed
            dropChance += dropChanceIncrease;
        }
    }
}
