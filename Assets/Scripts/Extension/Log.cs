using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Debug.Log 대체 클래스
/// 이 클래스를 사용하지 않으면 Log.txt에 로그가 찍히지않으니 사용하기를 권장.
/// </summary>
public static class Log
{
    static string logPath = Application.dataPath + "/Log.txt";

    public static void Print(string message)
    {
        #if UNITY_EDITOR
            Debug.Log(message);
        #endif
        LogWithWriteFile(message, LogType.Log);
    }

    public static void PrintWarning(string message)
    {
        #if UNITY_EDITOR
            Debug.LogWarning(message);
        #endif
        LogWithWriteFile(message, LogType.Warning);
    }

    public static void PrintError(string message)
    {
        #if UNITY_EDITOR
            Debug.LogError(message);
        #endif
        LogWithWriteFile(message, LogType.Error);
    }

    private static void LogWithWriteFile(string logMessage, LogType logType)
    {
        #if !UNITY_EDITOR
        FileStream fs = new FileStream(logPath, FileMode.Append);
        if(fs.Length > 2048000)
        {
            fs.Close();

            fs = new FileStream(logPath, FileMode.Create, FileAccess.Write);
        }

        StreamWriter writer = new StreamWriter(fs);
        string log = DateTime.Now.ToString("MM-dd hh:mm:ss") + " -- " + "[" + logType.ToString() + "]" + logMessage;
        writer.WriteLine(log);

        writer.Close();
        fs.Close();
        #endif
    }
}
