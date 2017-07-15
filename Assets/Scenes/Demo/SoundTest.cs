using UnityEngine;
using System.Collections;

public class SoundTest : MonoBehaviour {

    public GameObject startAirStrike;
    public GameObject endAirStrike;

	// Use this for initialization
	void Start () {
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(2.0f, StartTest);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartTest()
    {
        AirStrike airStrike = FindObjectOfType<AirStrike>();
        airStrike.SetAirStrikeStart(startAirStrike);
        airStrike.SetAirStrikeEnd(endAirStrike);

        airStrike.StartAirStrike();
    }
}
