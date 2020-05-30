using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GreyWolf : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetBehaviors()
    {
        Behaviours.Add(new MPatrol(this, Data.Speed));
        Behaviours.Add(new MChase(this, Data.Speed * 1.5f, Data.DetectionRange));
        Behaviours.Add(new MAttack(this, Data.AttackRange, Attack));
    }

    protected override void SetID()
    {
        ID = 1;
    }

    public void Attack()
    {
        GameObject player = null;

        Vector2 range = new Vector2(transform.position.x + Data.AttackRange, transform.position.y);
        var results = Physics2D.LinecastAll(transform.position, range);

        foreach (var result in results)
        {
            if (result.transform.CompareTag("Player"))
            {
                player = result.transform.gameObject;
                break;
            }
        }

        if (player == null)
        {
            Anim.speed = 1f;
            CurrentBehaviour = MonsterBehaviour.Run;
            return;
        }

        CurrentBehaviour = MonsterBehaviour.Attack;

        Anim.speed = 1f;
        Anim.Play("Bite");

        print("?");


    }
}
