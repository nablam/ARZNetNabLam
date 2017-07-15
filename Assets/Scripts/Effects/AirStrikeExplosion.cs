// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public class AirStrikeExplosion : MonoBehaviour {

    public GameObject fireObject;
    public int numberOfFires = 50;
    public float radius = 1.5f;
    public float fireDelay = 0.25f;

	// Use this for initialization
	void Start () {
        GetComponent<UAudioManager>().PlayEvent("_Explosion");
        //TimerBehaviour t = gameObject.AddComponent<TimerBehaviour>();
        //t.StartTimer(fireDelay, SpreadFire);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpreadFire()
    {
        for (int i = 0; i < numberOfFires; i++)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-radius, radius), transform.position.y, transform.position.z + Random.Range(-radius, radius));
            Instantiate(fireObject, position, Quaternion.identity);
        }
    }
}
