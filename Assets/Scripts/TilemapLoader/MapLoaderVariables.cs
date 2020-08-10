﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

public partial class MapLoader : MonoBehaviour
{
    private Dictionary<string, MapData> mapDatas;
    private Dictionary<string, MapData> MapDatas
    {
        get
        {
            if(mapDatas == null)
            {
                mapDatas = new Dictionary<string, MapData>();
            }

            return mapDatas;
        }
        set
        {
            mapDatas = value;
        }
    }

    private AssetBundle localAssetBundle;
    private AssetBundle LocalAssetBundle
    {
        get
        {
            localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
            if(localAssetBundle == null)
            {
                Log.PrintError("Failed to load AssetBundle!");
            }

            return localAssetBundle;
        }
    }

    const string PrefabFilePath = "Prefabs/";

    Tilemap[] tilemaps;

    [SerializeField]
    private string startMapName = "";
    public string StartMapName { get { return startMapName; } }

    public string CurrentMapName { get; set; }

    public bool IsLoadedMap { get; set; }

    private bool Initialized { get; set; }
}
