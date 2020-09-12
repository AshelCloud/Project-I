using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundary : MonoBehaviour
{
    [Header("값이 커질 수록 카메라 범위가 좁아짐")]

    [SerializeField]
    private float adjustOffset = 0f;

    private Tilemap tilemap;

    private void LateUpdate()
    {
        //수정 사항
        tilemap = GameObject.Find("Platform").GetComponent<Tilemap>();

        SetBoundary();
    }

    private void SetBoundary()
    {
        BoxCollider2D boundary = GetComponent<BoxCollider2D>();

        //범위 중앙 설정
        boundary.offset = tilemap.localBounds.center;

        //범위 크기 설정
        boundary.size = tilemap.localBounds.size;
        boundary.size -= new Vector2(adjustOffset, 0);
    }
}
