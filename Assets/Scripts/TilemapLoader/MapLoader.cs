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

    Tilemap[] tilemaps;

    private void Awake()
    {
        mapDatas = new Dictionary<string, MapData>();
        tilemaps = GetComponentsInChildren<Tilemap>();
    }

    private void Start()
    {
        StartCoroutine(JsonToTilemap("TestWolfForest"));
    }

    public IEnumerator JsonToTilemap(string fileName)
    {
        mapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(Application.dataPath + JsonFilePath, fileName).ToDictionary();

        //현재 있는 맵들 다 삭제하고 진행
        foreach (var tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                Destroy(tilemap.gameObject);
            }
        }
        tilemaps = null;

        //Destroy가 느리기때문에 다 삭제될때까지 기다림
        yield return new WaitUntil(() => transform.childCount == 0);

        //데이터 로드
        //데이터테이블 변경시 같이 변경해야함
        foreach (var data in mapDatas)
        {
            foreach (var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, Resources.Load<Tile>(TileAssetFilePath + tile.Name));
                tilemap.SetTransformMatrix(tile.LocalPlace, tile.Matrix);
            }
            foreach (var prefab in data.Value.Prefabs)
            {
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                go.transform.localScale = prefab.Scale;
            }

            //플레이어 생성코드
            //임시
            //필요시 삭제
            var playerReosurce = Resources.Load<GameObject>(PrefabFilePath + "Player");
            var player = Instantiate(playerReosurce, data.Value.PlayerStartPosition, Quaternion.identity);
        }

        yield return null;
    }

    //Tilemap 정보갱신
    //JsonToTilemap함수에 필요
    public Tilemap UpdateTilemapDataWithCreate(TilemapData tilemapData)
    {
        var maps = transform.GetComponentsInChildren<Tilemap>();

        foreach (var m in maps)
        {
            //자식중에 같은게 있으면 리턴
            if (m.name == tilemapData.Name)
            {
                return m;
            }
        }

        Tilemap map;

        //없으면 새로만들어서 정보업데이트후 리턴
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        //TilemapRenderer를 추가하면 Tilemap도 추가됨
        go.AddComponent<TilemapRenderer>();
        if (tilemapData.IsHaveCollider)
        {
            go.AddComponent<TilemapCollider2D>();
        }
        map = go.GetComponent<Tilemap>();

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        map.transform.name = tilemapData.Name;
        map.transform.position = tilemapData.Position;
        map.transform.rotation = tilemapData.Rotation;
        map.transform.localScale = tilemapData.Scale;
        map.animationFrameRate = tilemapData.AnimationFrameRate;
        map.color = tilemapData.Color;
        map.tileAnchor = tilemapData.TileAnchor;
        map.GetComponent<TilemapRenderer>().sortingOrder = tilemapData.OrderInLayer;

        return map;
    }
}
