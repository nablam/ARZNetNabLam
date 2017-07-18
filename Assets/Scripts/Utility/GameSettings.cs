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
            Instance = this;

        }
        else
            Destroy(gameObject);
    }
    //TestBox
    //public GameObject E_TestBox;                   //obj.name=TestBox_ES
    string AnchorName_TestBox = "ARZTestBox";
    public string GetAnchorName_TestBox() { return AnchorName_TestBox; }
    


    //ConsoleObject
    //public GameObject E_ConsoleObject;                   //obj.name=ConsoleObject_ES
    string AnchorName_ConsoleObject = "ARZConsoleObject";
    public string GetAnchorName_ConsoleObject() { return AnchorName_ConsoleObject; }

    //StemBase
    //public GameObject E_StemBase;                        //obj.name=StemBase_ES
    string AnchorName_StemBase = "ARZStemBase";
    public string GetAnchorName_StemBase() { return AnchorName_StemBase; }

    //SpawnPoint
    // public GameObject E_SpawnPoint;                     //obj.name=SpawnPoint_ES
    string AnchorName_SpawnPoint = "ARZSpawnPoint";
    public string GetAnchorName_SpawnPoint() { return AnchorName_SpawnPoint; }



    //SpawnPointDummy
    // public GameObject E_SpawnPointDummy;                     //obj.name=SpawnPointDummy_ES
    string AnchorName_SpawnPointDummy = "ARZSpawnPointDummy";
    public string GetAnchorName_SpawnPointDummy() { return AnchorName_SpawnPointDummy; }


    //Barrier
    //public GameObject E_Barrier;                         //obj.name=Barrier_ES
    string AnchorName_Barrier = "ARZBarrier";
    public string GetAnchorName_Barrier() { return AnchorName_Barrier; }


    //ScoreBord
    //public GameObject E_ScoreBoard;                     //obj.name=ScoreBoard_ES
    string AnchorName_ScoreBoard = "ARZScoreBoard";
    public string GetAnchorName_ScoreBoard() { return AnchorName_ScoreBoard; }


    //WeaponRack
    //public GameObject E_WeaponRack;                     //obj.name=WeaponRack_ES
    string AnchorName_WeaponRack = "ARZWeaponRack";
    public string GetAnchorName_WeaponRack() { return AnchorName_WeaponRack; }


    //PistoleMag
    //public GameObject E_PistoleMag;                     //obj.name=PistoleMag_ES
    string AnchorName_PistoleMag = "ARZPistoleMag";
    public string GetAnchorName_PistoleMag() { return AnchorName_PistoleMag; }


    //AmmoBox
    //public GameObject E_AmmoBox;                           //obj.name=AmmoBox_ES
    string AnchorName_AmmoBox = "ARZAmmoBox";
    public string GetAnchorName_AmmoBox() { return AnchorName_AmmoBox; }


    //AmmoBoxInfinite
    //public GameObject E_AmmoBoxInfinite;                 //obj.name=AmmoBoxInfinite_ES
    string AnchorName_AmmoBoxInfinite = "ARZAmmoBoxInfinite";
    public string GetAnchorName_AmmoBoxInfinite() { return AnchorName_AmmoBoxInfinite; }


    //PathFinder
    //public GameObject E_PathFinder;                    //obj.name=PathFinder_ES
    string AnchorName_GridMap = "ARZGridMap";
    public string GetAnchorName_GridMap() { return AnchorName_GridMap; }

    //ZONE1
     string AnchorName_ZoneOne = "ARZZoneOne";
    public string GetAnchorName_ZoneOne() { return AnchorName_ZoneOne; }
    //ZONE2
    string AnchorName_ZoneTwo = "ARZZoneTwo";
    public string GetAnchorName_ZoneTwo() { return AnchorName_ZoneTwo; }



    //WalkieTalkie
    //public GameObject E_WalkieTalkie;                    //obj.name=WalkieTalkie_ES
    string AnchorName_WalkieTalkie = "ARZWalkieTalkie";
    public string GetAnchorName_WalkieTalkie() { return AnchorName_WalkieTalkie; }


    //MistEmitter
    //public GameObject E_MistEmitter;                     //obj.name=MistEmitter_ES
    string AnchorName_MistEmitter = "ARZMistEmitter";
    public string GetAnchorName_MistEmitter() { return AnchorName_MistEmitter; }


    //mISTeND
    //public GameObject E_MistEnd;                        //obj.name=MistEnd_ES
    string AnchorName_MistEnd = "ARZMistEnd";
    public string GetAnchorName_MistEnd() { return AnchorName_MistEnd; }

    //HotSpot
    //public GameObject E_HotSpot;                        //obj.name=HotSpot_ES
    string AnchorName_HotSpot = "ARZHotSpot";
    public string GetAnchorName_HotSpot() { return AnchorName_HotSpot; }


    //AirStrikeStart
    //public GameObject E_AirStrikeStart;                        //obj.name=AirStrikeStart_ES
    string AnchorName_AirStrikeStart = "ARZAirStrikeStart";
    public string GetAnchorName_AirStrikeStart() { return AnchorName_AirStrikeStart; }

    //AirStrikeEnd
    //public GameObject E_AirStrikeEnd;                        //obj.name=AirStrikeEnd_ES
    string AnchorName_AirStrikeEnd = "ARZAirStrikeEnd";
    public string GetAnchorName_AirStrikeEnd() { return AnchorName_AirStrikeEnd; }




    public int latMaster;
    public int heiMaster;
    //segment size is used in gridpoint, gridmap and pathfinder
    public float SegmentSizeMaster;
    public float BottomSegmentSizeMaster;

    public bool IsTestMode;
    public float Zspeed;
    public float ZRotateSpeed;

    // spawnpoint spawns TotalZombiesToSpawn zombies . One Zombiew evry SpawnInterval 
    public float SpawnInterval;
    public int TotalZombiesToSpawn;

  
    void Start()
    {
        // IsTestMode = false;
        SpawnInterval = 1f;
        TotalZombiesToSpawn = 3;
        BottomSegmentSizeMaster = 3f;

        Zspeed = 3f;
        ZRotateSpeed = 10f;
    }
}
