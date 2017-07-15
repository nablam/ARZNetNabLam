// @Author Jeffrey M. Paquette ©2016
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameCanvas : MonoBehaviour {

    public GameObject waveStartCanvas, waveCompleteCanvas, damageCanvas;
    public Text waveNumText, parKillsText, killsText, bonusText;
    public Text pointsLostText, finalScoreText;
    public float fadeSpeed;

    CanvasGroup waveStartGroup, waveCompleteGroup;
    DamageTracker dmgTracker;
    GameObject focusedCanvas;
    CanvasGroup focusedGroup;
    UAudioManager audioManager;
    bool firstWave = true;

	// Use this for initialization
	void Start () {
        waveStartGroup = waveStartCanvas.GetComponent<CanvasGroup>();
        waveCompleteGroup = waveCompleteCanvas.GetComponent<CanvasGroup>();
        dmgTracker = damageCanvas.GetComponent<DamageTracker>();
        audioManager = gameObject.GetComponent<UAudioManager>();
	}

    public void PlayGameOverAudio()
    {
        audioManager.PlayEvent("_Boat");
    }

    public void WaveComplete(int parKills, int kills, int bonus)
    {
        parKillsText.text =  "Par Kills:  " + parKills;
        killsText.text = "Your Kills: " + kills;

        // set bonus points
        bonusText.text = "Bonus Points:   " + bonus;

        // set focused group to wave complete group
        focusedGroup = waveCompleteGroup;
        // set focused canvas to wave complete canvas
        focusedCanvas = waveCompleteCanvas;

        StartCoroutine(FadeIn());
    }

    public void WaveStarted(string waveNum) 
    {
        // set wave number text
        waveNumText.text = waveNum;

        // set focused group to wave start group
        focusedGroup = waveStartGroup;
        // set focused canvas on wave start canvas
        focusedCanvas = waveStartCanvas;

        StartCoroutine(FadeIn());

        // play appropriate audio
        if (firstWave)
        {
            firstWave = false;
            audioManager.PlayEvent("_Boat");
        } else
        {
            audioManager.PlayEvent("_Boom");
        }
    }

    IEnumerator FadeIn()
    {
        // turn alpha to 0 then show canvas
        focusedGroup.alpha = 0.0f;
        focusedCanvas.SetActive(true);

        while (focusedGroup.alpha < 1.0f)
        {
            focusedGroup.alpha += Time.deltaTime;
            yield return null;
        }

        TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
        t.StartTimer(1.0f, StartFadeOut);
    }

    IEnumerator FadeOut()
    {
        while (focusedGroup.alpha > 0.0f)
        {
            focusedGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        // turn off canvas
        focusedCanvas.SetActive(false);
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void TakeHit()
    {
        dmgTracker.TakeHit();
    }

    public void KillePlayer() {
        dmgTracker.KillPlayer();
    }
    public void ResetDamage()
    {
        dmgTracker.ResetDamage();
    }

    public void PointsLost(int value)
    {
        pointsLostText.text = "- " + value;
    }

    public void FinalScore(int value)
    {
        finalScoreText.text = value.ToString();
    }
}
