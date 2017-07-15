using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

public class CountDownAudioControl : MonoBehaviour {

    //Note:
    //This will start as soon as the player is in scene

    private UAudioManager audioManager;
    public Text CountdownTextBox;

    void OnEnable()
    {
         GameManager.CountDownHandler += DOCountDouwn; 
       
    }
    private void OnDisable()
    {
        GameManager.CountDownHandler -= DOCountDouwn;
    }
    void Start()
    {
        audioManager = GetComponent<UAudioManager>();
    }

    public void DOCountDouwn(int argNumber )
    {
        if (argNumber > 0 && argNumber < 11) {
            audioManager.PlayEvent("_" + argNumber.ToString());
            CountdownTextBox.text = argNumber.ToString();
        }
        if (argNumber == 1)
        {
            TimerBehavior t = gameObject.AddComponent<TimerBehavior>();
            t.StartTimer(1, HideCountDown);
        }
    }

    public void HideCountDown()
    {
        CountdownTextBox.text = "";
    }
}
