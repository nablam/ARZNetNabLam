// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    [Tooltip("How many mags of ammo does this object hold? (0 for infinite)")]
    public int count = 1;

    [Tooltip("Is this ammo box unlimited?")]
    public bool unlimited = false;

    [Tooltip("If unlimited, the amount of time it remains unlimited.")]
    public float unlimitedTime;

    private float oneMinLeft;


    private bool countdownStarted = false;

	// Use this for initialization
	void Start () {
        float mastertime = GameManager.Instance.GetMasterTime();
        if (mastertime > 60)
        {
            oneMinLeft = mastertime - 60;
        }
        else
            oneMinLeft = mastertime / 2;

    }
	
	// Update is called once per frame
	void Update () {
	    if (countdownStarted == false)
        {
            if (GameManager.Instance.IsGameStarted())
            {
                TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
                t.StartTimer(oneMinLeft, MakeLimited);
                countdownStarted = true;
            }
        }
	}

    public void Take()
    {
        if (unlimited)
            return;

        count--;
        if (count < 1)
        {
            Destroy(gameObject);
        }
    }

    public void MakeLimited()
    {
        unlimited = false;
    }
}
