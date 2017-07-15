// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsTracker : MonoBehaviour {

    public Text AccuracyText;
    public Text HeadShotCountText;
    public Text KillCountText;
    public Text pointsText;

    GameManager manager;

    // Use this for initialization
    void Start()
    {
        manager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //pointsText.text = manager.points.ToString();
        //Debug.Log(this.gameObject.name);
        UpdateUIText();
    }


    void UpdateUIText()
    {
        if (AccuracyText != null)
            AccuracyText.text = CalcAccuracy();
        if (HeadShotCountText != null)
            HeadShotCountText.text = manager.headShotCount.ToString();
        if (KillCountText != null)
            KillCountText.text = manager.killCount.ToString();
        if (pointsText != null)
            pointsText.text = manager.points.ToString();
    }
    string CalcAccuracy()
    {
        float fired = (float)manager.shotCount;
        float landed = (float)manager.targetHitCount;
        if (fired > 0)
        {
            float accuracy = (landed / fired) * 100;
            return accuracy.ToString("0.00") + " %";
        }

        return "0 %";


    }
}
