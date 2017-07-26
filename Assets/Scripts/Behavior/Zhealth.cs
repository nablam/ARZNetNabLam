using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Zhealth : NetworkBehaviour
{

    private int health = 50;

    public void DeductHealth(int dmg)
    {
        health -= dmg;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
