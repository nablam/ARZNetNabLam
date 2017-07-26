using HoloToolkit.Examples.SharingWithUNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FollowPath : NetworkBehaviour
{
    public TextMesh tm;
 
    Vector3? targetPathNode_POS=null;
    int pathNodeIndex = 0;
    List<Vector3> ATTACKPATH; List<Vector3> ATTConvertedBack;
    float ZombieMoveSpeed;//= 1f;
    float ZombieRotatSpeed;// = 10f;

    public float health = 1f;

    void Start() {
        ZombieMoveSpeed = GameSettings.Instance.Zspeed;
        ZombieRotatSpeed = GameSettings.Instance.ZRotateSpeed;
       // this.transform.SetParent(SharedCollection.Instance.transform, false);
    }
    //this is set by Spawner. It is the one to decide what player will be attacked by this zombie that it spawned
    public void FollowThisPath(List<Vector3> argPATH) {

        List<Vector3> ATTConvertedBack = new List<Vector3>();

        foreach (Vector3 v3 in argPATH) {
            ATTConvertedBack.Add(SharedCollection.Instance.gameObject.transform.TransformPoint(v3));
        }

        ATTACKPATH =  argPATH;
        //Debug.Log("z i waz gives argPATH with -> nodes = " + ATTACKPATH.Count);
        //Debug.Log("I start at " + transform.position + " aka loc " + transform.localPosition);

        //if (argPATH.Count > 0) { Debug.Log("first   " + ATTACKPATH[0]);
        //                         Debug.Log("second  " + ATTACKPATH[1]); }
        //if (argPATH.Count > 1) { Debug.Log("third   " + ATTACKPATH[2]); }
        //if (argPATH.Count > 2) { Debug.Log("fourth  " + ATTACKPATH[3]); }
    }

    void GetNextPathNode()
    {
       // Debug.Log("getting next node");
        if (pathNodeIndex < ATTACKPATH.Count)
        {
            targetPathNode_POS = ATTACKPATH[pathNodeIndex];
           // Debug.Log("next node is "+ targetPathNode_POS);
            pathNodeIndex++;
        }
        else
        {
           // Debug.Log("reachedend");
            targetPathNode_POS = null;
            ReachedGoal();
        }
    }

    void FixedUpdate(){
        if (!isServer)
        {
           // Debug.Log(" z is not server!");
            return;
        }

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
