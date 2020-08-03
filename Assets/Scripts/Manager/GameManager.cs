using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Loader loader;
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

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        loader.ActiveLoading();
        yield return new WaitUntil( () => loader.Loaded);

        mapLoader.LoadMap(mapLoader.StartMapName, false);
        yield return new WaitUntil( () => mapLoader.IsLoadedMap);

        loader.ActiveFadeOut();
        yield return null;
    }

    public void LoadMap(string mapName, bool isPrevious)
    {
        mapLoader.LoadMap(mapName, isPrevious);
    }
}
