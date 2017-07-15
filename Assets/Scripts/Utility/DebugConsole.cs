// @Author Jeffrey M. Paquette ©2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DebugConsole : MonoBehaviour {

    public Text console;

    private int logCounter;

    public void OnEnable()
    {
        logCounter = 0;
        Application.logMessageReceived += HandleLog;
    }

    public void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string message, string stackTrace, LogType type)
    {
        if (logCounter == 100)
            return;

        console.text += message + "\n";
      //  console.text += stackTrace + "\n";
        logCounter++;
    }
}
