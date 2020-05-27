using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MPatrol : MBehaviour
{
    private float Speed { get; set; }
    private float MoveTime { get; set; }
    private float StartTime { get; set; }
    private bool FlipX { get; set; }

    public MPatrol(Monster monster, float speed) :
    base(monster)
    {
        SetSpeed(speed);
        MoveTime = 2f;
        StartTime = 0f;
        FlipX = false;
    }

    public override void Start()
    {
        StartTime = Time.time;
    }

    public override void Update()
    {
        if(MObject.CurrentBehaviour != Monster.MonsterBehaviour.Run) { return; }

        MObject.CurrentBehaviour = Monster.MonsterBehaviour.Run;

        if(Time.time - StartTime >= MoveTime)
        {
            StartTime = Time.time;
            FlipX = !FlipX;
            Speed = -Speed;
            MObject.Renderer.flipX = FlipX;
        }

        MObject.RB.velocity = new Vector2(Speed * Time.deltaTime, MObject.RB.velocity.y);

        MObject.Anim.Play("Run");
    }

    private void SetSpeed(float speed)
    {
        Speed = speed;
    }
}