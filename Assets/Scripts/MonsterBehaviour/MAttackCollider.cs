﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAttackCollider : MonoBehaviour
{
    private Monster Root { get; set; }

    public Collider2D attackCollider;

    private void Awake()
    {
        Root = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<Player>().HitByMonster(Root.OffentPower);
        }
    }
}
