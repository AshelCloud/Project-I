using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Loader loader;
    [SerializeField]
    private MapLoader mapLoader;

    public string StartMapName
    {
        get
        {
            return mapLoader.StartMapName;
        }
    }
    public string CurrentMapName
    {
        get
        {
            return mapLoader.currentMapName;
        }
    }

    private UnityEvent _loadCompleteMapEvent;
    public UnityEvent LoadCompleteMapEvent 
    {
        private set
        {
            _loadCompleteMapEvent = value;
        }
        get
        {
            if(_loadCompleteMapEvent == null)
            {
                _loadCompleteMapEvent = new UnityEvent();
            }

            return _loadCompleteMapEvent;
        }
    }

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
        LoadCompleteMapEvent.Invoke();

        loader.ActiveFadeOut();
    }
}
