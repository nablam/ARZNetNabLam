using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
[NetworkSettings(sendInterval = 0.033f)]
public class ZombMotionSync : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private float syncYRot;
    [SyncVar]
    private Quaternion syncLocalRot;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform myTransform;
    private float lerpRate = 10;
    private float posThreshold = 0.25f;
    private float rotThreshold = 5;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isServer) return;
        //// TransmitMotion();
        //// LerpMotion();
        //syncPos = transform.localPosition;
        //syncLocalRot = transform.localRotation;
        //CmdNoCmdIalServerTransform(syncPos, syncLocalRot);
        simpleTransmit();
        simpleReceive();
    }


    void simpleTransmit()
    {
        if (!isServer)
        {
            return;
        }
        syncPos = myTransform.localPosition;     
    }
    void simpleReceive()
    {
        if (isServer)
        {
            return;
        }
        myTransform.position = syncPos;
    }

    [Command]
    public void CmdNoCmdIalServerTransform(Vector3 postion, Quaternion rotation)
    {
       
            syncPos = postion;
            syncLocalRot=rotation;
     
    }

    void TransmitMotion()
    {
        if (!isServer)
        {
            return;
        }

        if (Vector3.Distance(myTransform.position, lastPos) > posThreshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold)
        {
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;

            syncPos = myTransform.position;
            syncYRot = myTransform.localEulerAngles.y;
        }
    }

    void LerpMotion()
    {
        if (isServer)
        {
            return;
        }

        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);

        Vector3 newRot = new Vector3(0, syncYRot, 0);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
    }

}
