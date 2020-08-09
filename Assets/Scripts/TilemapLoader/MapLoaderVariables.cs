using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class MapLoader : MonoBehaviour
{
    private Dictionary<string, MapData> mapDatas;

    const string TileAssetFilePath = "TileAssets/";
    const string PrefabFilePath = "Prefabs/";

    Tilemap[] tilemaps;

    [SerializeField]
    private string startMapName = "";
    public string StartMapName { get { return startMapName; } }

    public string CurrentMapName { get; set; }

    public bool IsLoadedMap { get; set; }
}
