using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LogWirter logWirter;
    private void Awake()
    {
        logWirter = new LogWirter(Application.dataPath + "/Log.txt");
    }

    private void OnEnable()
    {
        logWirter.OpenStreams();
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        logWirter.CloseStreams();
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logMessage, string stackTrace, LogType type)
    {
        string log = DateTime.Now.ToString("mm-dd hh:mm:ss") + " -- " + logMessage;

        logWirter.WirteLine(log);
    }

    private void OnDestroy()
    {
        logWirter.Dispose();
    }
}
