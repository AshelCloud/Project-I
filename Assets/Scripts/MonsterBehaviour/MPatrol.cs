using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MPatrol : MBehaviour
{
    public Monster Monster { get; set; }
    public float StartTime { get; set; }
    public float MoveTime { get; set; }
    public float Speed { get; set; }

    public MPatrol(Monster monster, float speed = 0f, float moveTime = 1f)
    {
        Monster = monster;
        MoveTime = moveTime;
        Speed = speed;

        Start = PatrolStart;
        Update = PatrolUpdate;
    }

    private void PatrolStart()
    {
        StartTime = Time.time;
    }

    private void PatrolUpdate()
    {
        if (Monster.CurrentBehaviour != Monster.MonsterBehaviour.Run) { return; }

        Monster.CurrentBehaviour = Monster.MonsterBehaviour.Run;

        if (Time.time - StartTime >= MoveTime)
        {
            StartTime = Time.time;
            Monster.Renderer.flipX = !Monster.Renderer.flipX;
        }

        int direction = Monster.Renderer.flipX ? -1 : 1;

        Monster.RB.velocity = new Vector2(Speed * direction * Time.deltaTime, Monster.RB.velocity.y);

        Monster.Anim.Play("Run");
    }
}