using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

 
    Vector3? targetPathNode_POS=null;
    int pathNodeIndex = 0;
    List<Vector3> ATTACKPATH;
    float ZombieMoveSpeed;//= 1f;
    float ZombieRotatSpeed;// = 10f;

    public float health = 1f;

    void Awake() {
        GameSettings settings = GameManager.Instance.Settings;
        ZombieMoveSpeed = settings.Zspeed;
        ZombieRotatSpeed = settings.ZRotateSpeed;
    }
    //this is set by Spawner. It is the one to decide what player will be attacked by this zombie that it spawned
    public void FollowThisPath(List<Vector3> argPATH) {
        ATTACKPATH = argPATH;   
    }

    void GetNextPathNode()
    {
        if (pathNodeIndex < ATTACKPATH.Count)
        {
            targetPathNode_POS = ATTACKPATH[pathNodeIndex];
            pathNodeIndex++;
        }
        else
        {
            targetPathNode_POS = null;
            ReachedGoal();
        }
    }

    void Update(){
        if (targetPathNode_POS == null) {
            GetNextPathNode();
            if (targetPathNode_POS == null){
                ReachedGoal();
                return;
            }
        }
        Vector3? dir = targetPathNode_POS - this.transform.localPosition;

        float distThisFrame = ZombieMoveSpeed * Time.deltaTime;
        if (dir != null)
        {
            Vector3 dirV3 = (Vector3)dir;
            if (dirV3.magnitude <= distThisFrame)
            {
                // We reached the node
                targetPathNode_POS = null;
            }
            else
            {
                // Move to th node
                transform.Translate(dirV3.normalized * distThisFrame, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation(dirV3);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * ZombieRotatSpeed);
            }
        }
    }

    void ReachedGoal()
    {
         Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
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

    void OnTriggerEnter(Collider arCol)
    {
        //Debug.Log("Zhithit");
        if (arCol.gameObject.tag.CompareTo("BulletTag") == 0)
        {
            //Debug.Log("Z: bullet" + arCol.name + " hit me ");
            Destroy(arCol.gameObject);
            Destroy(gameObject);
        }
    }

}
