using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthCombat : NetworkBehaviour {

    [SyncVar(hook = "OnTakeDamage")]
    int health = 100;
    public TextMesh tmhealth;
    public GameObject bloodSplat;

    public GameObject bloodSplatNET;
    ScoreNet scronet;
    GameObject ScoreBox;
    private void Start()
    {
        ScoreBox = GameObject.Find("ScoreOBJNET");
        scronet = ScoreBox.GetComponent<ScoreNet>();
        //health = GameSettings.Instance.zombieHealth;
        tmhealth.text = GameSettings.Instance.zombieHealth.ToString();
    }


    void OnTakeDamage(int argNewHealth) {
        Debug.Log(gameObject.name + "  hooked" + argNewHealth );
      //  if (argNewHealth < health) { Die(); }
        tmhealth.text = argNewHealth.ToString(); 
        health = argNewHealth;
      //  DoBloodOnMe();
    }
    void DoBloodOnMe() { Instantiate(bloodSplat, tmhealth.transform.localPosition, Quaternion.identity); }
    public void TakeDamage(int damage, uint argIDofShooter)
    {
        health -= damage;

        Debug.Log(gameObject.name+  "  i am taking damage " + damage + " from player id "+ argIDofShooter );
       
        if (health <= 0)
        {
            Die();
            scronet.AddScoreForPlayer(argIDofShooter);
        }
    }

    public void Die()
    {
        // TODO: Do this more safely!
        //let spawnSingle know about this
        Destroy(gameObject);
    }
}

