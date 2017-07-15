// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections;
using HoloToolkit.Unity.InputModule;

public class GameGestureManager : MonoBehaviour {

    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public GameObject gun;

    GestureRecognizer gestureRecognizer;
    PlayerGun playerGun;
    WaveManager waveManager;

    // Use this for initialization
    void Start () {

        // get gun instance
        playerGun = gun.GetComponent<PlayerGun>();

        // get wave manager instance
        waveManager = FindObjectOfType<WaveManager>();

        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        // tap event handler
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;

        // hold event handlers - cancel and complete should do the same thing
        //gestureRecognizer.HoldStartedEvent += GestureRecognizer_HoldStartedEvent;
        //gestureRecognizer.HoldCompletedEvent += GestureRecognizer_HoldCompletedEvent;
        //gestureRecognizer.HoldCanceledEvent += GestureRecognizer_HoldCompletedEvent;
       
        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();
    }

    private void OnTap()
    {
        // don't register taps if player is dead
        if (GameManager.Instance.isDead || !playerGun.gameObject.activeInHierarchy)
            return;

        if (GazeManager.Instance.IsGazingAtObject)
        {
            if (GazeManager.Instance.HitObject.CompareTag("Ammo"))
            {
                // removed distance condition
                // Vector3.Distance(Camera.main.transform.position, GazeManager.Instance.HitInfo.point) < 1.25f &&
                if (!playerGun.IsReloading() && playerGun.GetGunType() == GunType.PISTOL)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGun.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("MagnumAmmo"))
            {
                if (!playerGun.IsReloading() && playerGun.GetGunType() == GunType.MAGNUM)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGun.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("UziClip"))
            {
                if (!playerGun.IsReloading() && playerGun.GetGunType() == GunType.UZI)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGun.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("ShotgunAmmo"))
            {
                if (!playerGun.IsReloading() && playerGun.GetGunType() == GunType.SHOTGUN)
                {
                    GazeManager.Instance.HitObject.SendMessage("Take");
                    playerGun.Reload();
                    return;
                }
            }
            else if (GazeManager.Instance.HitObject.CompareTag("RackWeapon"))
            {
                GazeManager.Instance.HitObject.SendMessage("Take");
                return;
            }
            else if (GazeManager.Instance.HitObject.CompareTag("Interactive"))
            {
                // removed distance condition
                //if (Vector3.Distance(Camera.main.transform.position, GazeManager.Instance.HitInfo.point) < 1.25f)
                //{
                    waveManager.OnTouchObject(GazeManager.Instance.HitObject);
                    return;
                //}
            }
        }

        playerGun.Fire();
    }

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        OnTap();
    }

    private void GestureRecognizer_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {
        if (!playerGun.enabled)
            return;

        playerGun.Fire();
    }

    private void GestureRecognizer_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {
        if (!playerGun.enabled)
            return;

        playerGun.StopFiring();
    }

    // Update is called once per frame   playerGun.Fire();
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) OnTap();

    }
}
