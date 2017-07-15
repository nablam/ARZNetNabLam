// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodFX : MonoBehaviour {

    [Tooltip("List of prefabs randomly generated on head shots.")]
    public GameObject[] HeadShotBloodPrefabs;

    [Tooltip("List of prefabs randomly generated on torso shots.")]
    public GameObject[] TorsoShotBloodPrefabs;

    [Tooltip("List of prefabs randomly generated on limb shots.")]
    public GameObject[] LimbShotBloodPrefabs;

    [Tooltip("How long the effect will remail active before destroying it.")]
    public float killDelay = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HeadShotFX(RaycastHit hitInfo)
    {
        int index = Random.Range(0, HeadShotBloodPrefabs.Length);
        GameObject obj = Instantiate(HeadShotBloodPrefabs[index], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal), hitInfo.collider.transform) as GameObject;
        KillTimer t = obj.AddComponent<KillTimer>();
        t.StartTimer(killDelay);
    }

    public void TorsoShotFX(RaycastHit hitInfo)
    {
        int index = Random.Range(0, TorsoShotBloodPrefabs.Length);
        GameObject obj = Instantiate(TorsoShotBloodPrefabs[index], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal), hitInfo.collider.transform) as GameObject;
        KillTimer t = obj.AddComponent<KillTimer>();
        t.StartTimer(killDelay);
    }

    public void LimbShotFX(RaycastHit hitInfo)
    {
        int index = Random.Range(0, LimbShotBloodPrefabs.Length);
        GameObject obj = Instantiate(LimbShotBloodPrefabs[index], hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal), hitInfo.collider.transform) as GameObject;
        KillTimer t = obj.AddComponent<KillTimer>();
        t.StartTimer(killDelay);
    }
}
