using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string TargetMap { get; set; }
    public string LinkingPortalName { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null )
        {
            GameManager.Instance.ActiveLoadMap(TargetMap, LinkingPortalName);
        }
    }
}
