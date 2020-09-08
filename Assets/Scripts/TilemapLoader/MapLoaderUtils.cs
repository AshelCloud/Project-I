using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class MapLoader : MonoBehaviour
{
    private void DestroyAllTilemaps()
    {
        if(tilemaps == null) { return; }

        foreach (var tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                Destroy(tilemap.gameObject);
            }
        }
        tilemaps = null;
    }

    #region Legacy
    //private Vector3 GetEndPositionOfMap(string mapName)
    //{
    //    foreach (var data in MapDatas)
    //    {
    //        int index = 0;

    //        foreach (var nextMapName in data.Value.NextMapName)
    //        {
    //            if (nextMapName == CurrentMapName)
    //            {
    //                return data.Value.PlayerEndPosition[index];
    //            }
    //            index++;
    //        }
    //    }
    //    return Vector3.zero;
    //}
    #endregion

    //Tilemap 정보갱신
    private Tilemap UpdateTilemapDataWithCreate(TilemapData mapData)
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

        //없으면 새로만들어서 정보업데이트후 리턴
        GameObject go = new GameObject();
        go.transform.SetParent(transform);

        //데이터들 오브젝트와 연결
        TilemapRendererLinking(mapData.TilemapRenderer, go);
        TilemapColliderLinking(mapData.TilemapCollider, go);
        Rigidbody2DLinking(mapData.RigidBody2D, go);
        CompositeColliderLinking(mapData.CompositeCollider, go);
        PlatformEffectorLinking(mapData.PlatformEffector, go);
        BoxCollider2DLinking(mapData.BoxCollider, go);

        Tilemap map = go.GetComponent<Tilemap>();

        //정보 업데이트
        //데이터테이블 변경시 같이 변경해야함
        if (string.IsNullOrEmpty(mapData.Name))
        {
            Log.PrintWarning("MapData Name is NULL or Empty");
        }
        else
        {
            map.transform.name = mapData.Name;
        }
        if (string.IsNullOrEmpty(mapData.Tag))
        {
            Log.PrintWarning("MapData Tag is NULL or Empty");
        }
        else
        {
            map.tag = mapData.Tag;
        }
        if (LayerMask.NameToLayer(mapData.Tag) == -1)
        {
            map.gameObject.layer = 0;
        }
        else
        {
            map.gameObject.layer = LayerMask.NameToLayer(mapData.Tag);
        }
        map.transform.position = mapData.Position;
        map.transform.rotation = mapData.Rotation;
        map.transform.localScale = mapData.Scale;
        map.animationFrameRate = mapData.AnimationFrameRate;
        map.color = mapData.Color;
        map.tileAnchor = mapData.TileAnchor;
        map.orientation = mapData.Orientation;

        return map;
    }

    private void TilemapRendererLinking(TilemapRendererData data, GameObject go)
    {
        if(data.IsNotNull == false)
        {
            return;
        }

        var renderer = go.GetComponent<TilemapRenderer>();
        if (renderer == null)
        {
            renderer = go.AddComponent<TilemapRenderer>();
        }
        TilemapRendererData rendererData = data;

        renderer.sortOrder = rendererData.SortOrder;
        renderer.mode = rendererData.Mode;
        renderer.detectChunkCullingBounds = rendererData.DetectChunkCullingBounds;
        renderer.sortingOrder = rendererData.OrderinLayer;
        renderer.maskInteraction = rendererData.SpriteMaskInteraction;
    }

    private void TilemapColliderLinking(TilemapCollider2DData data, GameObject go)
    {
        if(data.IsNotNull == false)
        {
            return;
        }

        var collider = go.GetComponent<TilemapCollider2D>();
        if (collider == null)
        {
            collider = go.AddComponent<TilemapCollider2D>();
        }
        var colliderData = data;

        collider.isTrigger = colliderData.IsTrigger;
        collider.usedByEffector = colliderData.UsedByEffector;
        collider.usedByComposite = colliderData.UsedByComposite;
        collider.offset = colliderData.Offset;
    }

    private void Rigidbody2DLinking(Rigidbody2DData data, GameObject go)
    {
        if (data.IsNotNull == false)
        {
            return;
        }

        var rigidbody2D = go.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            rigidbody2D = go.AddComponent<Rigidbody2D>();
        }

        var rigidbodyData = data;

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

    private void CompositeColliderLinking(CompositeCollider2DData data, GameObject go)
    {
        if(data.IsNotNull == false)
        {
            return;
        }

        var collider = go.GetComponent<CompositeCollider2D>();
        if (collider == null)
        {
            collider = go.AddComponent<CompositeCollider2D>();
        }
        var colliderData = data;

        collider.isTrigger = colliderData.IsTrigger;
        collider.usedByEffector = colliderData.UsedByEffector;
        collider.offset = colliderData.Offset;
        collider.geometryType = colliderData.GeometryType;
        collider.generationType = colliderData.GenerationType;
        collider.vertexDistance = colliderData.VertexDistance;
        collider.edgeRadius = colliderData.EdgeRadius;
    }

    private void PlatformEffectorLinking(PlatformEffectorData data, GameObject go)
    {
        if(data.IsNotNull == false)
        {
            return;
        }

        var effector = go.GetComponent<PlatformEffector2D>();
        if (effector == null)
        {
            effector = go.AddComponent<PlatformEffector2D>();
        }
        var effectorData = data;

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

    private void BoxCollider2DLinking(BoxCollider2DData data, GameObject go)
    {
        if(data.IsNotNull == false)
        {
            return;
        }

        var collider = go.GetComponent<BoxCollider2D>();
        if(collider == null)
        {
            collider = go.AddComponent<BoxCollider2D>();
        }
        var colliderData = data;

        collider.sharedMaterial = colliderData.Material;
        collider.isTrigger = colliderData.IsTrigger;
        collider.usedByEffector = colliderData.UsedByEffector;
        collider.usedByComposite = colliderData.UsedByComposite;
        collider.autoTiling = colliderData.AutoTiling;
        collider.offset = colliderData.Offest;
        collider.size = colliderData.Size;
        collider.edgeRadius = colliderData.EdgeRadius;
    }
}
