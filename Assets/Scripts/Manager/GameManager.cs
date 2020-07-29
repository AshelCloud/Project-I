using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MapLoader mapLoader;

    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;

        Log.Print("GameManager Initialize");
    }

    public void LoadMap(string mapName, bool isPrevious)
    {
        mapLoader.LoadMap(mapName, isPrevious);
    }
}
