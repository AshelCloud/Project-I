using System.Collections.Generic;
using UnityEngine;

/*
                                데이터
                                추후에 데이터 테이블 참고해서 수정
 */

[System.Serializable]
public class TileData
{
    public string Name;
    public Vector3 WorldPlace;
    public Vector3Int LocalPlace;
    public Matrix4x4 Matrix;
    public TilemapData BaseTilemap;
}

[System.Serializable]
public class PrefabData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public TilemapData BaseTileMap;
}

[System.Serializable]
public class TilemapData
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public float AnimationFrameRate;
    public Color Color;
    public Vector3 TileAnchor;
    public int OrderInLayer;
}

[System.Serializable]
public class MapData
{
    public List<TileData> Tiles;
    public List<PrefabData> Prefabs;
    public Vector3 PlayerStartPosition;
    public Vector3 PlayerEndPosition;
}