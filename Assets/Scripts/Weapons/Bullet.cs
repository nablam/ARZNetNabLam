// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class Bullet: MonoBehaviour{

    [Tooltip("Prefab of bullethole")]
    public GameObject bulletHole;

    [Tooltip("Damage this bullet causes")]
    public int damage;

    [Tooltip("Raycast layer of all objects this bullet should collide with")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    [Tooltip("The number of the enemy layer")]
    public int enemyLayer;

    [Tooltip("Pickup layer")]
    public int pickupLayer;

    [HideInInspector]
    public RaycastHit hitInfo;

    void Start()
    {
        // on start raycast from bullet position to hit enemies, pickups, or walls
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 40.0f, RaycastLayerMask))
        {
            if (hitInfo.collider.gameObject.layer == enemyLayer || hitInfo.collider.gameObject.layer == pickupLayer)
            {
                //DebugConsole.print("B: hit "+ hitInfo.collider.gameObject.name);
                hitInfo.collider.gameObject.SendMessageUpwards("TakeHit", this);
            }
            else
            {
                PlaceBulletHole(hitInfo);
            }
        }
        Destroy(gameObject);
    }

    void PlaceBulletHole(RaycastHit hitInfo)
    {
        Instantiate(bulletHole, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
    }
}
