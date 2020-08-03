using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
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

    private void Awake()
    {
        mapDatas = new Dictionary<string, MapData>();
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

        mapDatas = JsonManager.LoadJson<Serialization<string, MapData>>(json).ToDictionary();

        if(mapDatas == null)
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
        foreach (var data in mapDatas)
        {
            foreach (var tile in data.Value.Tiles)
            {
                var tilemap = UpdateTilemapDataWithCreate(tile.BaseTilemap);

                tilemap.SetTile(tile.LocalPlace, Resources.Load<Tile>(TileAssetFilePath + tile.Name));
                //tilemap.SetTile(tile.LocalPlace, localAssetBundle.LoadAsset<Tile>(tile.Name));
                //tilemap.SetTile(tile.LocalPlace, ResourcesContainer.Load<Tile>(tile.Name));
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
                //go.transform.localScale = prefab.Scale;
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
                position.x -= 3f;

                player.transform.position = position;
            }

            CurrentMapName = fileName;

            //TODO: Debug 코드 고치기
            //if (!string.IsNullOrEmpty(data.Value.NextMapName))
            //{
            //    GameManager.Instance.NextMapNameOfCurrentMap = data.Value.NextMapName;

            //    Log.Print("Set To NextMap: " + data.Value.NextMapName);
            //}

            //if (!string.IsNullOrEmpty(data.Value.PreviousMapName))
            //{
            //    GameManager.Instance.PreviousMapNameOfCurrentMap = data.Value.PreviousMapName;

            //    Log.Print("Set To PreviousMap: " + data.Value.PreviousMapName);
            //}
        }

        IsLoadedMap = true;
        tilemaps = GetComponentsInChildren<Tilemap>();
        Log.Print("Success to Map load");

        yield return null;
    }

    //Tilemap 정보갱신
    //JsonToTilemap함수에 필요
    //테스트용
    //본 프로젝트로 옮겨서 사용
    public Tilemap UpdateTilemapDataWithCreate(TilemapData mapData)
    {
        var maps = transform.GetComponentsInChildren<Tilemap>();

        foreach (var m in maps)
        {
            //자식중에 같은게 있으면 리턴
            if (m.name == mapData.Name)
            {
                return m;
            }
        }

        Tilemap map;

        //없으면 새로만들어서 정보업데이트후 리턴
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        //TilemapRenderer를 추가하면 Tilemap도 추가됨
        var renderer = go.AddComponent<TilemapRenderer>();
        TilemapRendererData rendererData = mapData.TilemapRenderer;

        renderer.sortOrder = rendererData.SortOrder;
        renderer.mode = rendererData.Mode;
        renderer.detectChunkCullingBounds = rendererData.DetectChunkCullingBounds;
        renderer.sortingOrder = rendererData.OrderinLayer;
        renderer.maskInteraction = rendererData.SpriteMaskInteraction;

        if (mapData.TilemapCollider.IsNotNull)
        {
            var collider = go.AddComponent<TilemapCollider2D>();
            var colliderData = mapData.TilemapCollider;

            collider.isTrigger = colliderData.IsTrigger;
            collider.usedByEffector = colliderData.UsedByEffector;
            collider.usedByComposite = colliderData.UsedByComposite;
            collider.offset = colliderData.Offset;
        }

        if (mapData.RigidBody2D.IsNotNull)
        {
            var rigidbody2D = go.AddComponent<Rigidbody2D>();
            var rigidbodyData = mapData.RigidBody2D;

            rigidbody2D.bodyType = rigidbodyData.BodyType;
            rigidbody2D.simulated = rigidbodyData.Simulated;
            rigidbody2D.useAutoMass = rigidbodyData.UseAutoMass;
            rigidbody2D.mass = rigidbodyData.Mass;
            rigidbody2D.drag = rigidbodyData.LinearDrag;
            rigidbody2D.angularDrag = rigidbodyData.AngularDrag;
            rigidbody2D.gravityScale = rigidbodyData.GravityScale;
            rigidbody2D.collisionDetectionMode = rigidbodyData.CollisionDetection;
            rigidbody2D.sleepMode = rigidbodyData.SleepingMode;
            rigidbody2D.interpolation = rigidbodyData.Interpolate;
            rigidbody2D.constraints = rigidbodyData.Constraints;
        }

        if (mapData.CompositeCollider.IsNotNull)
        {
            var collider = go.AddComponent<CompositeCollider2D>();
            var colliderData = mapData.CompositeCollider;

            collider.isTrigger = colliderData.IsTrigger;
            collider.usedByEffector = colliderData.UsedByEffector;
            collider.offset = colliderData.Offset;
            collider.geometryType = colliderData.GeometryType;
            collider.generationType = colliderData.GenerationType;
            collider.vertexDistance = colliderData.VertexDistance;
            collider.edgeRadius = colliderData.EdgeRadius;
        }

        if(mapData.PlatformEffector.IsNotNull)
        {
            var effector = go.AddComponent<PlatformEffector2D>();
            var effectorData = mapData.PlatformEffector;

            effector.useColliderMask = effectorData.UseColliderMask;
            effector.colliderMask = effectorData.ColliderMask;
            effector.rotationalOffset = effectorData.RotationalOffset;
            effector.useOneWay = effectorData.UseOneWay;
            effector.useOneWayGrouping = effectorData.UseOneWayGroup;
            effector.surfaceArc = effectorData.SurfaceArc;
            effector.useSideFriction = effectorData.UseSideFriction;
            effector.useSideBounce = effectorData.UseSideBounce;
            effector.sideArc = effectorData.SideArc;
        }

        if(mapData.BoxCollider.IsNotNull)
        {
            var collider = go.AddComponent<BoxCollider2D>();
            var colliderData = mapData.BoxCollider;

            collider.sharedMaterial = colliderData.Material;
            collider.isTrigger = colliderData.IsTrigger;
            collider.usedByEffector = colliderData.UsedByEffector;
            collider.usedByComposite = colliderData.UsedByComposite;
            collider.autoTiling = colliderData.AutoTiling;
            collider.offset = colliderData.Offest;
            collider.size = colliderData.Size;
            collider.edgeRadius = colliderData.EdgeRadius;
        }

        map = go.GetComponent<Tilemap>();

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        if(string.IsNullOrEmpty(mapData.Name))
        {
            Log.PrintWarning("MapData Name is NULL or Empty");
        }
        else
        {
            map.transform.name = mapData.Name;
        }
        if(string.IsNullOrEmpty(mapData.Tag))
        {
            Log.PrintWarning("MapData Tag is NULL or Empty");
        }
        else
        {
            map.tag = mapData.Tag;
        }
        if(LayerMask.NameToLayer(mapData.Tag) == -1)
        {
            map.gameObject.layer = 0;
            Log.PrintWarning("No Layer with same name as the MapData Tag");
        }
        else
        {
            map.gameObject.layer = LayerMask.NameToLayer(mapData.Tag);
        }
        //map.gameObject.layer = LayerMask.NameToLayer(mapData.Tag) == -1 ? 0 : LayerMask.NameToLayer(mapData.Tag);
        map.transform.position = mapData.Position;
        map.transform.rotation = mapData.Rotation;
        map.transform.localScale = mapData.Scale;
        map.animationFrameRate = mapData.AnimationFrameRate;
        map.color = mapData.Color;
        map.tileAnchor = mapData.TileAnchor;
        map.orientation = mapData.Orientation;

        return map;
    }

    private Vector3 GetEndPositionOfMap(string mapName)
    {
        foreach (var data in mapDatas)
        {
            int index = 0;

            foreach(var nextMapName in data.Value.NextMapName)
            {
                if(nextMapName == CurrentMapName)
                {
                    return data.Value.PlayerEndPosition[index];
                }
                index ++;
            }
        }
        return Vector3.zero;
    }
}
