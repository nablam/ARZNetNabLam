// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class AirStrike : MonoBehaviour {

    [Tooltip("Prefab of plane object.")]
    public GameObject plane;

    [Tooltip("Prefab of explosion effect.")]
    public GameObject explosion;

    [Tooltip("Delay in seconds between explosion effects.")]
    public float explosionDelay;

    GameObject airstrikeStart;
    GameObject airstrikeEnd;
    Vector3 midStrikePosition;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetAirStrikeStart(GameObject airstrikeStart)
    {
        this.airstrikeStart = airstrikeStart;
    }

    public void SetAirStrikeEnd(GameObject airstrikeEnd)
    {
        this.airstrikeEnd = airstrikeEnd;
    }

    public void StartAirStrike()
    {
        if (airstrikeStart == null)
            Debug.Log("Airstrike Start == null");
        else
            Debug.Log("Airstrike Start position: " + airstrikeStart.transform.position);

        if (airstrikeEnd == null)
            Debug.Log("Airstrike End == null");
        else
            Debug.Log("Airstrike End position: " + airstrikeEnd.transform.position);

        GameObject obj = Instantiate(plane, airstrikeStart.transform.position, Quaternion.identity) as GameObject;
        JetFighter jetFighter = obj.GetComponent<JetFighter>();
        jetFighter.StartMoving(airstrikeEnd.transform.position, Vector3.Distance(airstrikeStart.transform.position, airstrikeEnd.transform.position), 7.0f);
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(3.5f, FirstStrike);
    }

    public void FirstStrike()
    {
        Instantiate(explosion, airstrikeStart.transform.position, Quaternion.identity);
        float distance = Vector3.Distance(airstrikeStart.transform.position, airstrikeEnd.transform.position);
        midStrikePosition = Vector3.MoveTowards(airstrikeStart.transform.position, airstrikeEnd.transform.position, distance * 0.5f);
        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(explosionDelay, SecondStrike);
        t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(explosionDelay * 2.0f, FinalStrike);
    }

    public void SecondStrike()
    {
        Instantiate(explosion, midStrikePosition, Quaternion.identity);
        ZombieBehavior[] zoms = FindObjectsOfType<ZombieBehavior>();
        for (int i = 0; i < zoms.Length; i++)
        {
            zoms[i].Kill();
        }
    }

    public void FinalStrike()
    {
        Instantiate(explosion, airstrikeEnd.transform.position, Quaternion.identity);
    }
}
