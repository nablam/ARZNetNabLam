// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    public GameObject[] waves;                      // array of waves prefab objects
    public bool isWaveLoaded { get; private set; }  // flag set when wave is loaded

    GameObject currentWave;                         // the currently loaded wave
    IWave wave;                                     // the script of the currently loaded wave
    int numberOfWaves;                              // the total number of waves
    int waveNum = 0;                                // the number we are on

    WaveStandard WS;

    [Tooltip("Setting this value will allow infinitammo ")]
    public bool isInfinitAmo;

    // Use this for initialization
    void Start () {
        isWaveLoaded = false;
        numberOfWaves = waves.Length;

        if (numberOfWaves == 0) return;

        currentWave = Instantiate(waves[waveNum]);
        wave = currentWave.GetComponent<IWave>();
        isWaveLoaded = true;

        WS = GetCurrWave();

    }

    bool canmakeZombies = true;

    public void OnGameOver_WaveManager(bool stop)
    {
        canmakeZombies = !stop;
        currentWave.GetComponent<WaveStandard>().ResetSpawnStates();
        isWaveLoaded = false;
        Destroy(currentWave);

        // don't reload wave if game over
        if (!stop)
        {
            currentWave = Instantiate(waves[waveNum]);
            wave = currentWave.GetComponent<IWave>();
            WS = GetCurrWave();
            isWaveLoaded = true;

            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(5.0f, ResetWave);

         
            t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(10.0f, StartWave);        
        }
    }


    public void ResetWave()
    {
        if (canmakeZombies) { GameManager.Instance.ResetWave();
        currentWave.GetComponent<WaveStandard>().ResetSpawnStates(); }
           
    }



    public void StartWave()
    {
        if(canmakeZombies)
        wave.StartWave();
    }




    public void WaveCompleted_soPopANewOne()
    {
        //if (currentWave != null)
        //    currentWave.GetComponent<WaveStandard>().toggleSpawning(false);
        //else
        //    Debug.Log("NO CURRWAVE1");
        Destroy(currentWave);  //12345
        waveNum++;
        currentWave = Instantiate(waves[waveNum]);
        wave = currentWave.GetComponent<IWave>();
        isWaveLoaded = true;
        //if (currentWave != null)
        //    currentWave.GetComponent<WaveStandard>().toggleSpawning(false);
        //else
        //    Debug.Log("NO CURRWAVE2");
        BeginNextWave(10.0f);
    }




    public void BeginNextWave(float time)
    {
        // start next wave in time seconds
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(time, StartWave);
    }































    public WaveStandard GetCurrWave() { return currentWave.GetComponent<WaveStandard>(); }

 

    public string GetWaveRomanNumeral()
    {
        string romanNumeral;

        switch (waveNum)
        {
            case 0:
                romanNumeral = "I";
                break;
            case 1:
                romanNumeral = "II";
                break;
            case 2:
                romanNumeral = "III";
                break;
            case 3:
                romanNumeral = "IV";
                break;
            case 4:
                romanNumeral = "V";
                break;
            case 5:
                romanNumeral = "VI";
                break;
            case 6:
                romanNumeral = "VII";
                break;
            case 7:
                romanNumeral = "VIII";
                break;
            case 8:
                romanNumeral = "IX";
                break;
            case 9:
                romanNumeral = "X";
                break;
            default:
                romanNumeral = "";
                break;
        }

        return romanNumeral;
    }

    public IWave GetWave()
    {
        return wave;
    }



    public void OnReload()
    {
        wave.OnReload();
    }

    public void OnOutOfAmmo()
    {
        wave.OnOutOfAmmo();
    }

    public void OnKill(GameObject enemy)
    {
        wave.OnKill(enemy);
    }

    public void OnTouchObject(GameObject touched)
    {
        wave.OnTouchObject(touched);
    }

    public void OnTrigger(Collider c)
    {
        wave.OnTrigger(c);
    }

   




}
