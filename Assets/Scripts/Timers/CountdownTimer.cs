// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

    public Text timerTxt;

   
    private float timeInSeconds;

    float timer;
    string timerText;
    bool timesUpThrown;

    const int maxSecondCount = 12; //this cannot change

	// Use this for initialization
	void Start () {
        timeInSeconds = GameManager.Instance.GetMasterTime();
        timer = timeInSeconds;
        timesUpThrown = false;
        DisplayTimerText();
    }


    float OneSec = 1.01f;
	void Update () {
        if (timesUpThrown)
            return;

        if (timer <= 0)
        {
            timesUpThrown = true;
           // GameManager.Instance.TimesUp();
            GameManager.Instance.HardStop();
            timerText = "00:00";
            return;
        }

        if (GameManager.Instance.IsGameStarted())
        {
            timer -= Time.deltaTime;
            {
                if ((int)timer >= maxSecondCount ? false : true)
                {
                    OneSec -= Time.deltaTime;
                    if (OneSec < 0)
                    {
                        OneSec = 1f;
                        GameManager.Instance.call_CountDownAudioVideo((int)timer);
                    }
                }
            }
        }

        DisplayTimerText();

    }


    void DisplayTimerText()
    {
        int t = (int)timer;
        int minutes = t / 60;
        int seconds = t % 60;
        string min = (minutes < 10) ? ("0" + minutes) : ("" + minutes);
        string s = (seconds < 10) ? ("0" + seconds) : ("" + seconds);
        timerText = min + ":" + s;
    }

    void LateUpdate()
    {
        timerTxt.text = timerText;
    }
}
