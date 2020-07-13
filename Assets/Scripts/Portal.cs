using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string MapName { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !string.IsNullOrEmpty(MapName) )
        {
            GameManager.Instance.LoadMap(MapName);
        }
        else
        {
            //TODO: 로그 자세히 쓰기
            Log.PrintError("Portal Error");
        }
    }
}
