using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string MapName { get; set; }

    public bool IsPrevious { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.GetComponent<Player>() != null )
        {
            if(string.IsNullOrEmpty(MapName))
            {
                //TODO: 로그 자세히 쓰기
                Log.PrintError("Portal Error");
            }
            else
            {
                GameManager.Instance.LoadMap(MapName, IsPrevious);
            }
        }
    }
}
