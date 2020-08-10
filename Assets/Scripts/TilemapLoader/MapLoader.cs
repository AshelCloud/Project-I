using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public partial class MapLoader : MonoBehaviour
{
    public void Initialize()
    {
        if(Initialized)
        {
            return;
        }

        tilemaps = GetComponentsInChildren<Tilemap>();

        Initialized = true;

        Log.Print("MapLoader Initialize");
    }

    public void LoadMap(string fileName, bool isPrevious)
    {
        IsLoadedMap = false;

        StartCoroutine(JsonToTilemap(fileName, isPrevious));
    }

    public IEnumerator JsonToTilemap(string fileName, bool isPrevious)
    {
        Log.Print("MapLoader Load Start");

        TextAsset json = LocalAssetBundle.LoadAsset<TextAsset>(fileName);

        MapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(json).ToDictionary();
        if(MapDatas == null)
        {
            Log.PrintError("Failed to load Json MapData");
            yield return null;
        }

        //현재 있는 맵들 다 삭제하고 진행
        DestroyAllTilemaps();
        yield return new WaitUntil(() => transform.childCount == 0);

        //데이터기반 맵 배치
        foreach (var data in MapDatas)
        {
            Log.Print("Load to mapData");

            foreach (var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, ResourcesContainer.LoadInCache<Tile>(tile.Name));
                tilemap.SetTransformMatrix(tile.LocalPlace, tile.Matrix);
            }

            foreach (var prefab in data.Value.Prefabs)
            {
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                if(prefab.Tag.Contains("Portal"))
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + "Portal"), prefab.Position, prefab.Rotation, tilemap.transform);
                    go.name = prefab.Name;

                    var boxCollider = go.GetComponent<BoxCollider2D>();
                    if(boxCollider == null)
                    {
                        boxCollider = go.AddComponent<BoxCollider2D>();
                    }

                    boxCollider.isTrigger = prefab.BoxCollider.IsTrigger;
                    boxCollider.offset = prefab.BoxCollider.Offest;
                    boxCollider.size = prefab.BoxCollider.Size;
                    boxCollider.edgeRadius = prefab.BoxCollider.EdgeRadius;

                    int mapIndex = int.Parse(go.name);

                    var portal = go.GetComponent<Portal>();

                    if(mapIndex <= 0)
                    {
                        portal.MapName = data.Value.PreviousMapName;
                        portal.IsPrevious = true;
                    }
                    else
                    {
                        portal.MapName = data.Value.NextMapName[mapIndex - 1];
                        portal.IsPrevious = false;
                    }
                }
                else
                {
                    Instantiate(ResourcesContainer.Load(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                }
            }

            //플레이어 위치 재조정
            Log.Print("Player position replaced");

            Player curPlayer = FindObjectOfType(typeof(Player)) as Player;
            if(curPlayer == null)
            {
                Player resource = ResourcesContainer.Load<Player>(PrefabFilePath + "Player");
                curPlayer = Instantiate(resource, data.Value.PlayerStartPosition, Quaternion.identity);
            }

            if (isPrevious)
            {
                var position = GetEndPositionOfMap(fileName);

                curPlayer.transform.position = position;
            }
            else
            {
                curPlayer.transform.position = data.Value.PlayerStartPosition;
            }

            CurrentMapName = fileName;
        }

        IsLoadedMap = true;
        tilemaps = GetComponentsInChildren<Tilemap>();
        Log.Print("Success to Map load");

        yield return null;
    }
}
