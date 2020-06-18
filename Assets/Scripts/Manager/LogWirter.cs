using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogWirter
{
    private string logPath;
    private FileStream fs;
    private StreamWriter sw;

    public LogWirter(string path)
    {
        logPath = path;
    }

    public void OpenStreams()
    {
        fs = new FileStream(logPath, FileMode.Create, FileAccess.Write);
        sw = new StreamWriter(fs);
    }

    public void CloseStreams()
    {
        fs.Close();
        sw.Close();
    }

    public void WirteLine(string str)
    {
        sw.WriteLine(str);
    }

    public void Dispose()
    {
        fs.Dispose();
        sw.Dispose();
    }
}
