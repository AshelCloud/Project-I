using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Loader loader;
    [SerializeField]
    private MapLoader mapLoader;

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
        loader.Initialize();
        mapLoader.Initialize();

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        loader.ActiveLoading();
        yield return new WaitUntil(() => loader.Loaded);

        loader.Disable();
        yield return null;

        ActiveLoadMap(mapLoader.StartMapName, "SpawnPoint");
        yield return null;
    }

    public void ActiveLoadMap(string mapName, string linkingPortalName)
    {
        StartCoroutine(LoadMap(mapName, linkingPortalName));
    }

    private IEnumerator LoadMap(string mapName, string linkingPortalName)
    {
        loader.ActiveFadeIn();
        yield return new WaitUntil(() => loader.FadedValue >= 1f);

        mapLoader.LoadMap(mapName, linkingPortalName);
        yield return new WaitUntil( () => mapLoader.Loaded );

        loader.ActiveFadeOut();
    }
}
