using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int index = 1;

    private Monster myMonster;
    private Collider2D _collider;

    public Collider2D Collider
    {
        get
        {
            if(_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            return _collider;
        }
    }

    private void Awake()
    {
        myMonster = GetComponentInParent<Monster>();
    }

    private void Start()
    {
        if(Collider)
        {
            Collider.isTrigger = true;
            Collider.enabled = false;
        }
    }


    //TODO: 공격 기능 구현
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
