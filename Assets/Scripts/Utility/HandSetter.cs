using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSetter : MonoBehaviour {

    public Text text;

    private int m_hand = 0;
     
	// Use this for initialization
	void Start () {
	    if (PlayerPrefs.HasKey("hand"))
        {
            m_hand = PlayerPrefs.GetInt("hand");
            if (m_hand == 1)
            {
                text.text = "Hand: Left";
            }
        }
        else
        {
            text.text = "Hand: Right";
            PlayerPrefs.SetInt("hand", m_hand);
            PlayerPrefs.Save();
        }
	}

    public void ToggleHand()
    {
        if (m_hand == 0)
        {
            m_hand = 1;
            text.text = "Hand: Left";            
        }
        else
        {
            m_hand = 0;
            text.text = "Hand: Right";
        }
        PlayerPrefs.SetInt("hand", m_hand);
        PlayerPrefs.Save();
    }
}
    