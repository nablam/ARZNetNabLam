using System;
using UnityEngine;
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
public class Console3D : Singleton<Console3D>
{

    public Text DebugText;
    public Text StackText;

    TextMesh DebugTextmesh;
    TextMesh StackTraceTextmesh;
    int linenum = 0;

    void Start() {
        DebugTextmesh = GetComponentInChildren<TextMesh>();
    }

    #region CallbackHook
    public string output = "";
    public string stack = "";
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
       // output = logString;
        stack = stackTrace;

        LogToQueue(logString);
    }
    #endregion


    void Process_DebugMessage(string str) {
        LogToQueue(str);
    }
    void Process_StackMessage(string str)
    {
        LogToQueue(str);
    }




    int _curLineNum = -1;
    public int _MaxLines = 22;
    Queue<string> _MyDebugQueue = new Queue<string>();


    public bool DisplayOn = true;

    void LogToQueue(string str)
    {
        AddLineToDebugQueue(str);
        if (DisplayOn)
            RefreshDebugPanel();
    }



    void AddLineToDebugQueue(string str)
    {
        _curLineNum++;
        CheckOverflow2();
        str = _curLineNum + " " + str;
        _MyDebugQueue.Enqueue(str);
    }


    void CheckOverflow2()
    {
        if (_MyDebugQueue.Count >= _MaxLines)
        {
            _MyDebugQueue.Dequeue();
        }
    }



    void RefreshDebugPanel()
    {
        StringBuilder sb = new StringBuilder();

        foreach (string str in _MyDebugQueue)
        {
            sb.Append(str);
            sb.Append(Environment.NewLine);
        }

        DebugText.text = "";
        DebugText.text = sb.ToString();
    }







































    public void LOGit(string str)
    {
        LogToQueue(str);
    }

    public void LOGitError(string argstr)
    {
        LOGit(argstr, "ERROR!");
    }
    public void LOGitFormat(string ArgFormat, string argstr)
    {
        string s = string.Format(ArgFormat, argstr);
        LOGit(s);

    }
    public void LOGitWarning(string argstr)
    {
        LOGit(argstr, "WARNING!");
    }
    void LOGit(string str, string messageType)
    {
        LogToQueue(messageType + "|" + str);
    }


}
