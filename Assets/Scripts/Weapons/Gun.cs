// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, IGun {

    [Tooltip("The kind of gun this script is attached to")]
    public GunType gunType;

    [Tooltip("Barrel of the gun")]
    public GameObject barrel;

    [Tooltip("GameObject(s) from which bullet(s) will project")]
    public GameObject[] muzzle;

    [Tooltip("Prefab instantiated on fire")]
    public GameObject bullet;

    [Tooltip("Prefab MuzzleFlash")]
    public GameObject MuzzleFlash;

    [Tooltip("The number of rounds in a clip for this weapon")]
    public int roundsInClip;

    [Tooltip("The amount of time it takes to reload this weapon")]
    public float reloadTime;

    [Tooltip("This time is only used if the weapon is an Uzi")]
    public float repeatTime;

    [HideInInspector]
    public int rounds { get; private set; }     // how many rounds currently in clip

    TimerBehavior reloadTimer;                  // object that keeps track of reload time
    TimerBehavior repeatTimer;                  // object that keeps track of repeat fire
    bool reloading;                             // reloading flag
    UAudioManager audioManager;                 // this gun's UAudioManager
    WaveManager waveManager;                    // the game WaveManager
    FlashScript _fs;                            // flashscript used to trigger particlesystems attached 

    // Use this for initialization
    void Start () {
        // get references to managers
        audioManager = GetComponent<UAudioManager>();
        waveManager = FindObjectOfType<WaveManager>();

        // set rounds in clip
        rounds = roundsInClip;

        // attach reload and repeat timer objects
        reloadTimer = gameObject.AddComponent<TimerBehavior>();
        repeatTimer = gameObject.AddComponent<TimerBehavior>();

        _fs = MuzzleFlash.GetComponent<FlashScript>();

    }

 

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        Fire();
    //    }
    //}

    public GunType GetGunType()
    {
        return gunType;
    }

    public int GetRounds()
    {
        return rounds;
    }

    public void Fire()
    {
        // don't fire if reloading
        if (reloading) return;

        if (rounds > 0)
        {
            // shoot gun
            //if it is infinitamo, we skip decrementing rounds 
            if (!waveManager.isInfinitAmo) { rounds--; }
           
            _fs.Flash();
            audioManager.PlayEvent("_Fire");
            GameManager.Instance.IncrementCounter(2); //shotCount
            

            if (gunType == GunType.SHOTGUN)
            {
                //rotate barrel (this game object) on z axis randomly to simulate random bbs
                barrel.transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            }

            // for each muzzle object
            for (int i = 0; i < muzzle.Length; i++)
            {
                // instantiate bullet
                Instantiate(bullet, muzzle[i].transform.position, muzzle[i].transform.rotation);
            }

        }
        else
        {
            // out of ammo
            audioManager.PlayEvent("_Empty");
            waveManager.OnOutOfAmmo();
        }

        // repeat fire if gun type is uzi
        if (gunType == GunType.UZI && rounds > 0)
        {
            repeatTimer.StartTimer(repeatTime, Fire, false);
        }
    }

    public void StopFiring()
    {
        // does nothing for single fire weapons
        if (gunType == GunType.UZI)
        {
            repeatTimer.StopTimer();
        }
    }

    public void Reload()
    {
        if (reloading) return;

        reloading = true;
        audioManager.PlayEvent("_Reload");
        reloadTimer.StartTimer(reloadTime, ReloadComplete, false);
    }

    public bool IsReloading()
    {
        return reloading;
    }

    public void InterruptReload()
    {
        reloadTimer.StopTimer();
        reloading = false;
    }

    public void ReloadComplete()
    {
        rounds = roundsInClip;
        reloading = false;
    }
}
