// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class RackWeapon : MonoBehaviour {

    public GameObject weaponRack;
    public GunType gunType;

    WeaponRack rack;

	// Use this for initialization
	void Start () {
        rack = weaponRack.GetComponent<WeaponRack>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Take()
    {
        // tell rack that we are equipping this new gun
        rack.Equip(gunType);

        // deactivate this gun
        gameObject.SetActive(false);
    }
}
