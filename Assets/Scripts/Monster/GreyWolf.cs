using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GreyWolf : Monster
{
    private float StartTime { get; set; }
    private float MoveTime { get; set; }

    protected override void Start()
    {
        base.Start();

        CurrentBehaviour = MonsterBehaviour.Run;
    }

    protected override void SetBehaviors()
    {
        Behaviours.Add("Patrol", new MPatrol(this, Data.Speed, 2f));
        Behaviours.Add("Chase", new MChase(this, Data.Speed, Data.DetectionRange));
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