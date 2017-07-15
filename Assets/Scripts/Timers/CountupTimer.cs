using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class CountupTimer : MonoBehaviour {

    public Text timerIncreaseTXT;

    private float timeInSeconds;

    float timerIncrease;
    string timerText;
    bool timesUpThrown;

    // Use this for initialization
    void Start()
    {
       
        timerIncrease =0;
        timesUpThrown = false;
        DisplayTimerText();
    }

    public void resetTimer() { timerIncrease = 0; timerIncreaseTXT.text = "RESET"; }

    float OneSec = 1.01f;
    void Update()
    {
        if (timesUpThrown)
            return;

     
        if (GameManager.Instance.IsGameStarted())
        {
            timerIncrease += Time.deltaTime;         
        }

        DisplayTimerText();

    }


    void DisplayTimerText()
    {
        int t = (int)timerIncrease;
        int minutes = t / 60;
        int seconds = t % 60;
        string min = (minutes < 10) ? ("0" + minutes) : ("" + minutes);
        string s = (seconds < 10) ? ("0" + seconds) : ("" + seconds);
        timerText = min + ":" + s;
    }

    void LateUpdate()
    {
        timerIncreaseTXT.text = timerText;
    }
}

