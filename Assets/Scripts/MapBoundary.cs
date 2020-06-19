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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var position = collision.transform.position.x - Bounds.min.x;

            if(position >= Bounds.max.x)
            {
                GameManager.Instance.LoadNextMap();
            }
            else
            {
                GameManager.Instance.LoadPreviousMap();
            }
        }
    }
}
