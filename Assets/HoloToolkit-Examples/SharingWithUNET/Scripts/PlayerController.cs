// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Examples.SharingWithUNET
{
    /// <summary>
    /// Controls player behavior (local and remote).
    /// </summary>
    [NetworkSettings(sendInterval = 0.033f)]
    public class PlayerController : NetworkBehaviour, IInputClickHandler
    {

        public TextMesh tm;
        /// <summary>
        /// The game object that represents the 'bullet' for 
        /// this player. Must exist in the spawnable prefabs on the
        /// NetworkManager.
        /// </summary>
        public GameObject bullet;

        /// <summary>
        /// The transform of the shared world anchor.
        /// </summary>
        private Transform sharedWorldAnchorTransform;

        /// <summary>
        /// The position relative to the shared world anchor.
        /// </summary>
        [SyncVar]
        private Vector3 localPosition;

        /// <summary>
        /// The rotation relative to the shared world anchor.
        /// </summary>
        [SyncVar]
        private Quaternion localRotation;

        /// <summary>
        /// Sets the localPosition and localRotation on clients.
        /// </summary>
        /// <param name="postion">the localPosition to set</param>
        /// <param name="rotation">the localRotation to set</param>
        [Command]
        public void CmdTransform(Vector3 postion, Quaternion rotation)
        {
            if (!isLocalPlayer)
            {
                localPosition = postion;
                localRotation = rotation;
            }
        }

        private NetworkInstanceId _playerNetId;

        string sID;

        public GameObject bloood1;
        public GameObject bloood1Net;
        private void SetID()
        {
            _playerNetId = GetComponent<NetworkIdentity>().netId;
            sID = _playerNetId.ToString();
            tm.text = sID;
            if (isServer)
                RpcSetClientID(sID);
        }

        void RpcSetClientID(string str)
        {
            tm.text = str;
           // this.gameObject.name = "ship_" + str;
        }




        GameObject ScoreBox;
  


        private void Start()
        {
            if (SharedCollection.Instance == null)
            {
                Debug.LogError("This script required a SharedCollection script attached to a gameobject in the scene");
                Destroy(this);
                return;
            }
            SetID();
            damage = GameSettings.Instance.damage ;
            range = GameSettings.Instance.range;


            if (isLocalPlayer)
            {


                ScoreBox = GameObject.Find("ScoreOBJNET");
                ScoreBox.GetComponent<ScoreNet>().RegisterPlayer(_playerNetId.Value);
                // If we are the local player then we want to have airtaps 
                // sent to this object so that projeciles can be spawned.
                InputManager.Instance.AddGlobalListener(gameObject);

            }
            else
            {
                Debug.Log("remote player");
                //GetComponentInChildren<MeshRenderer>().material.color = Color.red;


                if (isServer)
                {
                    GetComponentInChildren<MeshRenderer>().material.color = Color.green;

                }
                else
                    GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
            }



            sharedWorldAnchorTransform = SharedCollection.Instance.gameObject.transform;
            transform.SetParent(sharedWorldAnchorTransform);
        }

        private void OnDestroy()
        {
            if (isLocalPlayer)
            {
                InputManager.Instance.RemoveGlobalListener(gameObject);
            }
        }

        private void Update()
        {
            // If we aren't the local player, we just need to make sure that the position of this object is set properly
            // so that we properly render their avatar in our world.
            if (!isLocalPlayer)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, 0.3f);
                transform.localRotation = localRotation;
                return;
            }

            // if we are the remote player then we need to update our worldPosition and then set our 
            // local (to the shared world anchor) position for other clients to update our position in their world.
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;

            // Depending on if you are host or client, either setting the SyncVar (client) 
            // or calling the Cmd (host) will update the other users in the session.
            // So we have to do both.
            localPosition = transform.localPosition;
            localRotation = transform.localRotation;
            CmdTransform(localPosition, localRotation);
        }

        /// <summary>
        /// Called when the local player starts.  In general the side effect should not be noticed
        /// as the players' avatar is always rendered on top of their head.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        }

        /// <summary>
        /// Called on the host when a bullet needs to be added. 
        /// This will 'spawn' the bullet on all clients, including the 
        /// client on the host.
        /// </summary>
        [Command]
        void CmdFire()
        {
            Vector3 bulletDir = transform.forward;
            Vector3 bulletPos = transform.position + bulletDir * 1.5f;

            // The bullet needs to be transformed relative to the shared anchor.
            GameObject nextBullet = (GameObject)Instantiate(bullet, sharedWorldAnchorTransform.InverseTransformPoint(bulletPos), Quaternion.Euler(bulletDir));
            nextBullet.GetComponentInChildren<Rigidbody>().velocity = bulletDir * 1.0f;
            NetworkServer.Spawn(nextBullet);

            // Clean up the bullet in 8 seconds.
            Destroy(nextBullet, 8.0f);
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (isLocalPlayer)
            {
                //CmdFire();
                LazerShoot();
            }
        }

        private RaycastHit hit;
        private int damage = 26;
        private float range = 200;
        void LazerShoot()
        {
            Debug.Log("pew pew");
            if (Physics.Raycast(transform.TransformPoint(0, 0, 0.5f), transform.forward, out hit, range))
            {
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag == "ZombieTag")
                {
                    string uIdentity = hit.transform.gameObject.name;
                    CmdTellServerWhichZombieWasShot(uIdentity, damage, _playerNetId.Value);
                }
            }
        }

     

        [Command]
        void CmdTellServerWhichZombieWasShot(string uniqueID, int dmg, uint argplayerID)
        {
            Debug.Log("zclient sent this to server to invoke ");
            GameObject go = GameObject.Find(uniqueID);
            if (go != null) { Debug.Log("server found ths Zombie hit "); } else { Debug.Log("server Cannot find "); }
            go.GetComponent<HealthCombat>().TakeDamage(dmg, argplayerID);
        }
    }
}
