// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using System.Collections;

public class TimerBehavior : MonoBehaviour {

	public delegate void callbackFunction();

	callbackFunction function;
	bool isStarted = false;
    bool isReset = false;
	bool destroyOnComplete = true;
	float time = 0f;

	// Use this for initialization
	void Start (){
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (isReset)
        {
            isStarted = true;
            isReset = false;
        }

        if (isStarted) {
			time -= Time.deltaTime;
			if (time <= 0.0f) {
                time = 0.1f;
				function();
                if (destroyOnComplete)
                    Destroy(this);
                else
                    isStarted = false;
			}
		}
	}

	public void StartTimer(float timeInSeconds, callbackFunction function, bool destroyOnComplete = true){
        this.time = timeInSeconds;
        this.function = function;
		this.destroyOnComplete = destroyOnComplete;
        this.isReset = true;
	}

    public void StopTimer()
    {
        // completely stop timer
        isReset = false;
        isStarted = false;
    }
}
