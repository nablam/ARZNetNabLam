using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHand : MonoBehaviour {

    public enum Ammunition
    {
        NONE,
        PISTOL,
        MAGNUM,
        UZI,
        SHOTGUN
    };

    public GameObject pistolMag, uziMag, magnumSpeedLoader, shotgunSpeedLoader;
    public PlayerGun playerGun;

    private UAudioManager audioManager;
    private Ammunition heldAmmo = Ammunition.NONE;
    private WeaponRack rack;
    private GameObject heldObject;

	private void Start () {

        audioManager = gameObject.GetComponent<UAudioManager>();

        rack = GameObject.FindObjectOfType<WeaponRack>();

        // set all ammo game objects to inactive
        pistolMag.SetActive(false);
        uziMag.SetActive(false);
        magnumSpeedLoader.SetActive(false);
        shotgunSpeedLoader.SetActive(false);
	}
	
	private void Update () {
		
	}

    public Ammunition GetHeldAmmo()
    {
        return heldAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool takeAmmo = false;

        switch (other.gameObject.tag)
        {
            case "Ammo":
                if (heldAmmo != Ammunition.PISTOL)
                {
                    if (playerGun.GetGunType() != GunType.PISTOL) return;

                    other.gameObject.SendMessage("Take");
                    ReturnHeldAmmo();
                    heldAmmo = Ammunition.PISTOL;
                    if (heldObject != null)
                        heldObject.SetActive(false);
                    pistolMag.SetActive(true);
                    heldObject = pistolMag;
                    takeAmmo = true;
                }
                break;
            case "UziClip":
                if (heldAmmo != Ammunition.UZI)
                {
                    if (playerGun.GetGunType() != GunType.UZI) return;

                    other.gameObject.SendMessage("Take");
                    ReturnHeldAmmo();
                    heldAmmo = Ammunition.UZI;
                    if (heldObject != null)
                        heldObject.SetActive(false);
                    uziMag.SetActive(true);
                    heldObject = uziMag;
                    takeAmmo = true;
                }
                break;
            case "MagnumAmmo":
                if (heldAmmo != Ammunition.MAGNUM)
                {
                    if (playerGun.GetGunType() != GunType.MAGNUM) return;

                    other.gameObject.SendMessage("Take");
                    ReturnHeldAmmo();
                    heldAmmo = Ammunition.MAGNUM;
                    if (heldObject != null)
                        heldObject.SetActive(false);
                    magnumSpeedLoader.SetActive(true);
                    heldObject = magnumSpeedLoader;
                    takeAmmo = true;
                }
                break;
            case "ShotgunAmmo":
                if (heldAmmo != Ammunition.SHOTGUN)
                {
                    if (playerGun.GetGunType() != GunType.SHOTGUN) return;

                    other.gameObject.SendMessage("Take");
                    ReturnHeldAmmo();
                    heldAmmo = Ammunition.SHOTGUN;
                    if (heldObject != null)
                        heldObject.SetActive(false);
                    shotgunSpeedLoader.SetActive(true);
                    heldObject = shotgunSpeedLoader;
                    takeAmmo = true;
                }
                break;
            case "PlayerGun":
                if (heldAmmo != Ammunition.NONE)
                    Reload();
                break;
        }

        // if ammo was taken then play pickup sound
        if (takeAmmo)
        {
            audioManager.PlayEvent("_Pickup");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerGun") && heldAmmo != Ammunition.NONE)
        {
            Reload();
        }
    }
    private void Reload()
    {
        if (playerGun.Reload(heldAmmo))
        {
            heldAmmo = Ammunition.NONE;
            heldObject.SetActive(false);
            heldObject = null;
        }
    }

    private void ReturnHeldAmmo()
    {
        if (rack == null)
            rack = GameObject.FindObjectOfType<WeaponRack>();

        rack.ReturnAmmo(heldAmmo);
    }
}
