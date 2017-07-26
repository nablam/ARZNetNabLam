// @Author Nabil Lamriben ©2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings Instance = null;
    private void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            InitColors();
            Instance = this;

        }
        else
            Destroy(gameObject);
    }

    #region AnchorNames
    string AnchorName_TestBox = "ARZTestBox";
    public string GetAnchorName_TestBox() { return AnchorName_TestBox; }


    string AnchorName_ConsoleObject = "ARZConsoleObject";
    public string GetAnchorName_ConsoleObject() { return AnchorName_ConsoleObject; }

    string AnchorName_StemBase = "ARZStemBase";
    public string GetAnchorName_StemBase() { return AnchorName_StemBase; }

    string AnchorName_SpawnPoint = "ARZSpawnPoint";
    public string GetAnchorName_SpawnPoint() { return AnchorName_SpawnPoint; }


    string AnchorName_SpawnPointDummy = "ARZSpawnPointDummy";
    public string GetAnchorName_SpawnPointDummy() { return AnchorName_SpawnPointDummy; }


    string AnchorName_Barrier = "ARZBarrier";
    public string GetAnchorName_Barrier() { return AnchorName_Barrier; }

    string AnchorName_ScoreBoard = "ARZScoreBoard";
    public string GetAnchorName_ScoreBoard() { return AnchorName_ScoreBoard; }

    string AnchorName_WeaponRack = "ARZWeaponRack";
    public string GetAnchorName_WeaponRack() { return AnchorName_WeaponRack; }


    string AnchorName_PistoleMag = "ARZPistoleMag";
    public string GetAnchorName_PistoleMag() { return AnchorName_PistoleMag; }

    string AnchorName_AmmoBox = "ARZAmmoBox";
    public string GetAnchorName_AmmoBox() { return AnchorName_AmmoBox; }

    string AnchorName_AmmoBoxInfinite = "ARZAmmoBoxInfinite";
    public string GetAnchorName_AmmoBoxInfinite() { return AnchorName_AmmoBoxInfinite; }

    string AnchorName_GridMap = "ARZGridMap";
    public string GetAnchorName_GridMap() { return AnchorName_GridMap; }

    //ZONE1
     string AnchorName_ZoneOne = "ARZZoneOne";
    public string GetAnchorName_ZoneOne() { return AnchorName_ZoneOne; }
    //ZONE2
    string AnchorName_ZoneTwo = "ARZZoneTwo";
    public string GetAnchorName_ZoneTwo() { return AnchorName_ZoneTwo; }

    string AnchorName_WalkieTalkie = "ARZWalkieTalkie";
    public string GetAnchorName_WalkieTalkie() { return AnchorName_WalkieTalkie; }

    string AnchorName_MistEmitter = "ARZMistEmitter";
    public string GetAnchorName_MistEmitter() { return AnchorName_MistEmitter; }

    string AnchorName_MistEnd = "ARZMistEnd";
    public string GetAnchorName_MistEnd() { return AnchorName_MistEnd; }

    string AnchorName_HotSpot = "ARZHotSpot";
    public string GetAnchorName_HotSpot() { return AnchorName_HotSpot; }

    string AnchorName_AirStrikeStart = "ARZAirStrikeStart";
    public string GetAnchorName_AirStrikeStart() { return AnchorName_AirStrikeStart; }

    string AnchorName_AirStrikeEnd = "ARZAirStrikeEnd";
    public string GetAnchorName_AirStrikeEnd() { return AnchorName_AirStrikeEnd; }

    #endregion

    #region ColorUtils
    List<Color> PathColors;
    void InitColors()
    {
        PathColors = new List<Color>();
        ColorIndex = 0;
        PathColors.Add(Color.red);
        PathColors.Add(Color.green);
        PathColors.Add(Color.blue);
        PathColors.Add(Color.yellow);
        PathColors.Add(Color.cyan);
        PathColors.Add(Color.magenta);
        PathColors.Add(Color.gray);
        PathColors.Add(Color.white);
    }
    int ColorIndex = 0;
    public Color GETRand_color()
    {
        Color c = new Color();
        c = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        return c;
    }
    public Color GetNExtColor()
    {
        ColorIndex++;
        if (ColorIndex >= PathColors.Count) ColorIndex = 0;
        return PathColors[ColorIndex];
    }
    #endregion

    #region GamePlaySettings
    public int latMaster;
    public int heiMaster;
    //segment size is used in gridpoint, gridmap and pathfinder
    public float SegmentSizeMaster;
    public float BottomSegmentSizeMaster;

    public bool IsShowGridPointMesh;
    public bool IsShowNodeTag;

    public float Zspeed;
    public float ZRotateSpeed;

    // spawnpoint spawns TotalZombiesToSpawn zombies . One Zombiew evry SpawnInterval 
    public float spawndelay;
    public float SpawnInterval;
    public int TotalZombiesToSpawn;

    public bool applyCostTweek;
    public bool IsLeanPath;

    public int numberOfPAthsPerSpawnPoint;
    #endregion
    void Start()
    {
        // ShowGridPointMesh = true;
        // ShowNodeTag = true;

        latMaster = 7;
        heiMaster = 4;
        BottomSegmentSizeMaster = 3f;

        SegmentSizeMaster = .25f;

        spawndelay = 10f;
        SpawnInterval = 10f;
        TotalZombiesToSpawn = 20;


        Zspeed = 0.2f;
        ZRotateSpeed = 10f;

        numberOfPAthsPerSpawnPoint = 2;

        applyCostTweek = true;
        IsLeanPath = true;
    }

}
