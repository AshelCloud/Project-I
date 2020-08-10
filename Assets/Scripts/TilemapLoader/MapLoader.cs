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
    private void Awake()
    {
        tilemaps = GetComponentsInChildren<Tilemap>();

        Log.Print("MapLoader Initialize");
    }

    //private void Start()
    //{
    //    LoadMap(startMapName, false);
    //    //StartCoroutine(JsonToTilemap("Forest_1"));
    //}

    public void LoadMap(string fileName, bool isPrevious)
    {
        IsLoadedMap = false;

        StartCoroutine(JsonToTilemap(fileName, isPrevious));
    }

    public IEnumerator JsonToTilemap(string fileName, bool isPrevious)
    {
        Log.Print("MapLoader Load Start");

        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        if(localAssetBundle == null)
        {
            Log.PrintError("Failed to load AssetBundle!");
            yield return null;
        }

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>(fileName);

        MapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(json).ToDictionary();
        if(MapDatas == null)
        {
            Log.PrintError("Failed to load Json MapData");
            yield return null;
        }

        //현재 있는 맵들 다 삭제하고 진행
        var curPlayer = FindObjectOfType(typeof(Player)) as Player;
        
        if(curPlayer != null)
        {
            Destroy(curPlayer.gameObject);
        }

        foreach (var tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                Destroy(tilemap.gameObject);
            }
        }
        tilemaps = null;

        //Destroy가 느리기때문에 다 삭제될때까지 기다림
        yield return new WaitUntil(() => transform.childCount == 0 && curPlayer == null);

        //데이터 로드
        //데이터테이블 변경시 같이 변경해야함
        Log.Print("Load to mapData");
        foreach (var data in MapDatas)
        {
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
                    GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                }
            }

            //플레이어 생성코드
            //임시
            //필요시 삭제
            Log.Print("Instantiate Player");

            var playerReosurce = Resources.Load<GameObject>(PrefabFilePath + "Player");

            //TODO: 플레이어 위치 설정
            //TODO: 플레이어 재생성이 아닌 위치만 재조정하도록 변경
            var player = Instantiate(playerReosurce, data.Value.PlayerStartPosition, Quaternion.identity);
            if (player == null)
            {
                Log.PrintError("NullExeption Player");
            }
            else
            {
                player.tag = "Player";
            }

            if (isPrevious)
            {
                var position = GetEndPositionOfMap(fileName);

                player.transform.position = position;
            }

            CurrentMapName = fileName;
        }

        IsLoadedMap = true;
        tilemaps = GetComponentsInChildren<Tilemap>();
        Log.Print("Success to Map load");

        yield return null;
    }
}
