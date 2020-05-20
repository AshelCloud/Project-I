using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    private Dictionary<string, MapData> mapDatas;

    //Application.dataPath와 연동시켜서 사용
    const string JsonFilePath = "/Resources/MapJsons/";
    
    
    const string TileAssetFilePath = "TileAssets/";
    const string PrefabFilePath = "Prefabs/";

    private int TilemapLoadIndex { get; set; }

    private void Awake()
    {
        mapDatas = new Dictionary<string, MapData>();
        TilemapLoadIndex = 0;
    }

    private void Start()
    {
        JsonToTilemap("Test_1");
    }

    public void JsonToTilemap(string fileName)
    {
        mapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(Application.dataPath + JsonFilePath, fileName, true).ToDictionary();

        //데이터 로드
        //데이터테이블 변경시 같이 변경해야함
        foreach (var data in mapDatas)
        {
            foreach (var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, Resources.Load<Tile>(TileAssetFilePath + tile.Name));
            }
            foreach (var prefab in data.Value.Prefabs)
            {
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                go.transform.localScale = prefab.Scale;
            }

            //TODO: 플레이어 생성
            //var playerReosurce = Resources.Load<GameObject>(PrefabFilePath + "Player");
            //var player = Instantiate(playerReosurce, data.Value.PlayerStartPosition, Quaternion.identity);
        }

    }

    //Tilemap 정보갱신
    //JsonToTilemap함수에 필요
    public Tilemap UpdateTilemapDataWithCreate(TilemapData tilemapData)
    {
        var maps = transform.GetComponentsInChildren<Tilemap>();

        Tilemap map;

        //Tilemap 존재시 정보만 업데이트
        //혹은 Tilemap 생성
        if (TilemapLoadIndex >= maps.Length)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            map = go.AddComponent<Tilemap>();
        }
        else
        {
            map = maps[TilemapLoadIndex];
        }

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        map.transform.name = tilemapData.Name;
        map.transform.position = tilemapData.Position;
        map.transform.rotation = tilemapData.Rotation;
        map.transform.localScale = tilemapData.Scale;
        map.animationFrameRate = tilemapData.AnimationFrameRate;
        map.color = tilemapData.Color;
        map.tileAnchor = tilemapData.TileAnchor;

        return map;
    }
}
