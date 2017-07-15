// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class WeaponRack : MonoBehaviour {

    // weapon objects on rack
    public GameObject pistol;
    public GameObject magnum;
    public GameObject shotgun;
    public GameObject uzi;

    // ammo objects on rack
    public GameObject[] magnumAmmo;
    public GameObject[] uziAmmo;
    public GameObject[] shotgunAmmo;

    PlayerGun playerGun;

	// Use this for initialization
	private void Start () {
        // get references to critical objects
        playerGun = FindObjectOfType<PlayerGun>();
	}

    private void ReturnWeapon(GunType type)
    {
        // return weapon to rack
        switch(type)
        {
            case GunType.PISTOL:
                pistol.SetActive(true);
                break;
            case GunType.MAGNUM:
                magnum.SetActive(true);
                break;
            case GunType.UZI:
                uzi.SetActive(true);
                break;
            case GunType.SHOTGUN:
                shotgun.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        // when game has started disable all ammo on rack

        for (int i = 0; i < magnumAmmo.Length; i++)
            magnumAmmo[i].SetActive(false);

        for (int i = 0; i < uziAmmo.Length; i++)
            uziAmmo[i].SetActive(false);

        for (int i = 0; i < shotgunAmmo.Length; i++)
            shotgunAmmo[i].SetActive(false);
    }

    public void ReturnAmmo(OffHand.Ammunition a)
    {
        switch (a)
        {
            case OffHand.Ammunition.UZI:
                for (int i = 0; i < uziAmmo.Length; i++)
                {
                    if (!uziAmmo[i].activeInHierarchy)
                    {
                        uziAmmo[i].SetActive(true);
                        break;
                    }
                }
                break;
            case OffHand.Ammunition.MAGNUM:
                for (int i = 0; i < magnumAmmo.Length; i++)
                {
                    if (!magnumAmmo[i].activeInHierarchy)
                    {
                        magnumAmmo[i].SetActive(true);
                        break;
                    }
                }
                break;
            case OffHand.Ammunition.SHOTGUN:
                for (int i = 0; i < shotgunAmmo.Length; i++)
                {
                    if (!shotgunAmmo[i].activeInHierarchy)
                    {
                        shotgunAmmo[i].SetActive(true);
                        break;
                    }
                }
                break;
        }
    }

    public void AwardMagnum()
    {
        // if magnum is not equipped
        // activate magnum on rack
        if (playerGun.GetGunType() != GunType.MAGNUM)
            magnum.SetActive(true);

        playerGun.Reload(OffHand.Ammunition.MAGNUM);

        // activate all magnum ammo on rack
        for (int i = 0; i < magnumAmmo.Length; i++)
            magnumAmmo[i].SetActive(true);
    }

    public void AwardUzi()
    {
        // if uzi is not equipped
        // activate uzi on rack
        if (playerGun.GetGunType() != GunType.UZI)
            uzi.SetActive(true);

        playerGun.Reload(OffHand.Ammunition.UZI);

        // activate all uzi ammo on rack
        for (int i = 0; i < uziAmmo.Length; i++)
            uziAmmo[i].SetActive(true);
    }

    public void AwardShotgun()
    {
        // if shotgun is not equipped
        // activate shotgun on rack
        if (playerGun.GetGunType() != GunType.SHOTGUN)
            shotgun.SetActive(true);

        playerGun.Reload(OffHand.Ammunition.SHOTGUN);

        // activate all shotgun ammo on rack
        for (int i = 0; i < shotgunAmmo.Length; i++)
            shotgunAmmo[i].SetActive(true);
    }

    public void Equip(GunType type)
    {
        // attempt to return equipped weapon back to the rack
        ReturnWeapon(playerGun.GetGunType());

        if (playerGun == null)
            playerGun = FindObjectOfType<PlayerGun>();

        // equip new gun
        playerGun.Equip(type);
    }
}
