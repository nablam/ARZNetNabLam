using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthCombat : NetworkBehaviour {

    [SyncVar(hook ="OnTakeDamage")]
    int health = 100;
    public TextMesh tmhealth;
    public GameObject bloodSplat;

    public GameObject bloodSplatNET;

    void OnTakeDamage(int argdmg) {
        Debug.Log(gameObject.name + "  hooked" + argdmg);
        if (argdmg < health) { Die(); }

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log(gameObject.name+  "  i am taking damage " + damage);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: Do this more safely!
        //let spawnSingle know about this
        Destroy(gameObject);
    }
}

