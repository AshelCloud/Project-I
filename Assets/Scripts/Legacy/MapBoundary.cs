using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    private Bounds Bounds { get; set; }

    private void Awake()
    {
        Bounds = GetComponent<BoxCollider2D>().bounds;
    }
}
