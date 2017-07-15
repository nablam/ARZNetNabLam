// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public interface IWave {
    //string GetFileName();
    void StartWave();
    void OnReload();
    void OnOutOfAmmo();
    void OnKill(GameObject enemy);
    void OnTouchObject(GameObject touched);
    void OnTrigger(Collider c);
    void OnGameOver();
    void OnComplete();
}
