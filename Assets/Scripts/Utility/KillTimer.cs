// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class KillTimer : MonoBehaviour {

    float time;
    bool isStarted;

    // Use this for initialization
    void Start()
    {
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            time -= Time.deltaTime;
            if (time <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StartTimer(float time)
    {
        this.time = time;
        isStarted = true;
    }
}
