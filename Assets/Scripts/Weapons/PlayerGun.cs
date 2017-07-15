// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public enum GunType
{
    PISTOL,
    MAGNUM,
    UZI,
    SHOTGUN,
}

public class PlayerGun : MonoBehaviour {

    public GameObject pistol;
    public GameObject magnum;
    public GameObject uzi;
    public GameObject shotgun;

    GameObject weaponObject;
    IGun weapon;

	private void Start () {

        GameManager.Instance.playerGun = this.gameObject;

        // start game with pistol equipped
        weaponObject = pistol;
        weapon = pistol.GetComponent<IGun>();
        pistol.SetActive(true);

        // make all other weapons inactive
        magnum.SetActive(false);
        uzi.SetActive(false);
        shotgun.SetActive(false);
	}

    public GunType GetGunType()
    {
        if (weapon != null)
            return weapon.GetGunType();
        else
            return GunType.PISTOL;
    }

    public int GetRounds()
    {
        if (weapon != null)
            return weapon.GetRounds();
        else
            return 0;
    }

    public void StartGame()
    {
        // when game starts reload all weapons
        pistol.SetActive(true);
        pistol.GetComponent<IGun>().ReloadComplete();
        pistol.SetActive(false);

        uzi.SetActive(true);
        uzi.GetComponent<IGun>().ReloadComplete();
        uzi.SetActive(false);

        magnum.SetActive(true);
        magnum.GetComponent<IGun>().ReloadComplete();
        magnum.SetActive(false);

        shotgun.SetActive(true);
        shotgun.GetComponent<IGun>().ReloadComplete();
        shotgun.SetActive(false);

        weaponObject.SetActive(true);
    }

    public void Fire()
    {
        if (weapon != null)
            weapon.Fire();
    }

    public void StopFiring()
    {
        if (weapon != null)
            weapon.StopFiring();
    }

    public void Reload()
    {
        if (weapon != null)
            weapon.Reload();
    }

    public bool Reload(OffHand.Ammunition a)
    {
        switch (a)
        {
            case OffHand.Ammunition.PISTOL:
                if (weapon.GetGunType() == GunType.PISTOL)
                {
                    Reload();
                    return true;
                }
                break;
            case OffHand.Ammunition.MAGNUM:
                if (weapon.GetGunType() == GunType.MAGNUM)
                {
                    Reload();
                    return true;
                }
                break;
            case OffHand.Ammunition.UZI:
                if (weapon.GetGunType() == GunType.UZI)
                {
                    Reload();
                    return true;
                }
                break;
            case OffHand.Ammunition.SHOTGUN:
                if (weapon.GetGunType() == GunType.SHOTGUN)
                {
                    Reload();
                    return true;
                }
                break;
        }

        return false;
    }

    public bool IsReloading()
    {
        if (weapon != null)
            return weapon.IsReloading();
        else return true;
    }

    public void Equip(GunType type)
    {
        // unequip previous weapon
        if (weapon != null)
        {
            if (weapon.IsReloading())
                weapon.InterruptReload();

            weaponObject.SetActive(false);
        }

        // equip new weapon
        switch (type)
        {
            case GunType.PISTOL:
                weaponObject = pistol;
                break;
            case GunType.MAGNUM:
                weaponObject = magnum;
                break;
            case GunType.UZI:
                weaponObject = uzi;
                break;
            case GunType.SHOTGUN:
                weaponObject = shotgun;
                break;
        }

        // make equipped weapon active
        weaponObject.SetActive(true);

        // get script object
        weapon = weaponObject.GetComponent<IGun>();
    }
}
