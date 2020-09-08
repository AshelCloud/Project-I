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
        if (Initialized)
        {
            return;
        }

        tilemaps = GetComponentsInChildren<Tilemap>();

        Initialized = true;

        Log.Print("MapLoader Initialize");
    }

    public void LoadMap(string fileName, string linkingPortalName)
    {
        Loaded = false;

        StartCoroutine(JsonToTilemap(fileName, linkingPortalName));
    }

    public IEnumerator JsonToTilemap(string fileName, string linkingPortalName)
    {
        Log.Print("MapLoader Load Start");

        TextAsset json = LocalAssetBundle.LoadAsset<TextAsset>(fileName);

        MapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(json).ToDictionary();
        if (MapDatas == null)
        {
            Log.PrintError("Failed to load Json MapData");
            yield return null;
        }

        Log.Print("Success Load MapData AssetBundle");

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
                Log.Print("PrefabsName: " + prefab.Name);
                var tilemap = UpdateTilemapDataWithCreate(prefab.BaseTileMap);

                var go = Instantiate(ResourcesContainer.Load(PrefabFilePath + prefab.Name), prefab.Position, prefab.Rotation, tilemap.transform);
                go.name = prefab.Name;
                #region Legacy Portal System
                //if (prefab.Tag.Contains("Portal"))
                //{
                //    GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + "Portal"), prefab.Position, prefab.Rotation, tilemap.transform);
                //    go.name = prefab.Name;

                //    BoxCollider2DLinking(prefab.BoxCollider, go);

                //    int mapIndex = int.Parse(go.name);
                //    var portal = go.GetComponent<Portal>();

                //    if(mapIndex <= 0)
                //    {
                //        portal.MapName = data.Value.PreviousMapName;
                //        portal.IsPrevious = true;
                //    }
                //    else
                //    {
                //        portal.MapName = data.Value.NextMapName[mapIndex - 1];
                //        portal.IsPrevious = false;
                //    }
                //}
                #endregion
            }

            foreach (var portalData in data.Value.Portals)
            {
                var tilemap = UpdateTilemapDataWithCreate(portalData.BaseTileMap);

                GameObject go = Instantiate(Resources.Load<GameObject>(PrefabFilePath + "Portal"), portalData.Position, portalData.Rotation, tilemap.transform);
                go.name = portalData.Name;

                BoxCollider2DLinking(portalData.BoxCollider, go);
                Portal portal = go.GetComponent<Portal>();

                portal.LinkingPortalName = portalData.LinkingPortalName;
                portal.TargetMap = portalData.TargetMap;
            }

            //플레이어 위치 재조정
            Player curPlayer = FindObjectOfType(typeof(Player)) as Player;
            if (curPlayer == null)
            {
                Player resource = ResourcesContainer.Load<Player>(PrefabFilePath + "Player");
                curPlayer = Instantiate(resource, Vector3.zero, Quaternion.identity);
            }
            Log.Print("LinkingPortalName: " + linkingPortalName);
            GameObject targetPortal = GameObject.Find(linkingPortalName);

            if (targetPortal == null)
            {
                curPlayer.transform.position = Vector3.zero;
            }
            else
            {
                curPlayer.transform.position = targetPortal.transform.position + (targetPortal.transform.right * 3f);
            }

            yield return null;

            RaycastHit2D[] hits = Physics2D.RaycastAll(curPlayer.transform.position, Vector2.down);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    //TODO: 플레이어의 몸 절반이 뭔지 몰라서 그냥 0.5f 넣었음
                    Vector2 pos = new Vector2(hit.point.x, hit.point.y + 0.5f);
                    curPlayer.transform.position = pos;
                }

            }

            #region Legacy PlayerPosition Replace
            //Log.Print("Player position replaced");

            //Player curPlayer = FindObjectOfType(typeof(Player)) as Player;
            //if(curPlayer == null)
            //{
            //    Player resource = ResourcesContainer.Load<Player>(PrefabFilePath + "Player");
            //    curPlayer = Instantiate(resource, data.Value.PlayerStartPosition, Quaternion.identity);
            //}

            //if (isPrevious)
            //{
            //    var position = GetEndPositionOfMap(fileName);

            //    curPlayer.transform.position = position;
            //}
            //else
            //{
            //    curPlayer.transform.position = data.Value.PlayerStartPosition;
            //}
            #endregion
            CurrentMapName = fileName;
        }

        Loaded = true;
        tilemaps = GetComponentsInChildren<Tilemap>();
        Log.Print("Success to Map load");

        yield return null;
    }
}
