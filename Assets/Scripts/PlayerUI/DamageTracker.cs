// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;

public class DamageTracker : MonoBehaviour {

    public CanvasGroup hit1, hit2, hit3;
    public int hp;
    public float healingScalar;

    Animator damageAnimator;
    UAudioManager audioManager;

    float damage = 0f;

    bool readyForHit = true;

	private void Start () {
        damageAnimator = GetComponent<Animator>();
        audioManager = GetComponent<UAudioManager>();
	}
	
	private void Update () {
        // if dead then don't update
        if (damage > hp)
            return;

        // recover from hits
	    if (damage > 0f)
        {
            damage -= Time.deltaTime * healingScalar;
            if (damage <= 0f)
                damage = 0f;
        }

        // show blood splatters
        hit3.alpha = Mathf.Clamp(damage - 2f, 0f, 1f);
        hit2.alpha = Mathf.Clamp(damage - 1f, 0f, 1f);
        hit1.alpha = Mathf.Clamp(damage, 0f, 1f);
	}

    public void ReadyForHit()
    {
        readyForHit = true;
    }

    public void ResetDamage()
    {
        damage = 0;
    }

    public void KillPlayer()
    {
       
        damageAnimator.SetTrigger("Hit");
        audioManager.PlayEvent("_Scratch");

        damage = 10000000000;
        if (damage > hp)
        {
            GameManager.Instance.GameOver_GameManager();
        }
    }
    public void TakeHit()
    {
        if (readyForHit)
        {
            damage += 1f;
            readyForHit = false;
            damageAnimator.SetTrigger("Hit");
            audioManager.PlayEvent("_Scratch");
            if (damage > hp)
            {
                GameManager.Instance.GameOver_GameManager();
            }
        }
    }
}
