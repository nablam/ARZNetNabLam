// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public interface IGun{

    // method to return number of rounds in gun
    int GetRounds();

    // method to return gun type
    GunType GetGunType();

    // method to check if gun is reloading
    bool IsReloading();

    // method for firing weapon
    void Fire();

    // method to stop firing weapon (only implemented on automatic weapons)
    void StopFiring();

    // method for reloading weapon
    void Reload();

    // method for reload callback
    void ReloadComplete();

    // method to tell gun to stop reloading (called if gun is switched via reload)
    void InterruptReload();
}
