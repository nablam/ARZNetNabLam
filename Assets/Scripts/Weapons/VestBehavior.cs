using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VestBehavior : MonoBehaviour {

    public GameObject Explosion;

    GameObject ZombieWearingthevest;
    ZombieBehavior _zb;
    public float _radius=2.3f;
    public List<GameObject> objectsToExplode;
    public LayerMask mask;

    void Start () {
		//print(this.transform.parent.parent)
        FindZombieWearer();
        GetZombieBehavior();

    }

    private void LateUpdate()
    {
        Collider[] arra_colliders= Physics.OverlapSphere(this.transform.position, _radius, mask);
        objectsToExplode = arra_colliders.Select(x => x.gameObject).ToList();
    }
    void Update()
    {
        TriggerExplosion();
    }

    void FindZombieWearer() {
        Transform t = this.transform;
        while (t.parent != null) {
            t = t.parent;
        }
        ZombieWearingthevest = t.gameObject; 
    }

    void GetZombieBehavior()
    {
        if (ZombieWearingthevest != null) {
           _zb=ZombieWearingthevest.GetComponent<ZombieBehavior>();
        }
    }

    public void TakeHit(Bullet bullet)
    {
        //todo, use location of bullet hit to generate push 
       // DebugConsole.print("V: vest took hit");
        ExplodeAndDie();
    }

    public void ExplodeAndDie()
    {


        Instantiate(Explosion, transform.position, Quaternion.identity);

        foreach (GameObject go in objectsToExplode)
        {
            go.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
        }
        Destroy(gameObject);
        _zb.Kill();



    }
    /*
    public void Explode()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        if (ZombieWearingthevest != null)
        {
            _zb.Kill();
            Destroy(ZombieWearingthevest);

        }

    }*/

    void TriggerExplosion()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print("allauakbaaar!!!");
            ExplodeAndDie();

        }

    }
}
