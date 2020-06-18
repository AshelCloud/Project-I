using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logMessage, string stackTrace, LogType type)
    {
        string log = DateTime.Now.ToString("MM-dd hh:mm:ss") + " -- " + logMessage;

        text.text = log;
    }
}
