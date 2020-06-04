using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MChase : MBehaviour
{
    public Monster Monster { get; private set; }
    public string AnimationName { get; private set; }
    public float Speed { get; set; }
    public float ChaseRange { get; set; }

    public MChase(Monster monster, string animationName, float speed = 0f, float range = 0f, params Action[] actions)
    {
        Monster = monster;
        AnimationName = animationName;
        Speed = speed;
        ChaseRange = range;

        foreach(var action in actions)
        {
            Update += action;
        }

        Update += ChaseUpdate;
        OnGizmos += ChaseGizmos;
    }

    private void ChaseUpdate()
    {
        if(Monster.CurrentBehaviour != Monster.MonsterBehaviour.Run && Monster.CurrentBehaviour != Monster.MonsterBehaviour.Chase) 
        {
            return;
        }

        Vector2 start = new Vector2(Monster.transform.position.x - ChaseRange, Monster.transform.position.y);
        Vector2 end = new Vector2(Monster.transform.position.x + ChaseRange, Monster.transform.position.y);

        var results = Physics2D.LinecastAll(start, end);

        GameObject player = null;

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
            Monster.CurrentBehaviour = Monster.MonsterBehaviour.Run;
            return;
        }

        Monster.CurrentBehaviour = Monster.MonsterBehaviour.Chase;

        Monster.Anim.Play(AnimationName);

        Vector3 direction = player.transform.position - Monster.transform.position;

        float spd = Speed;
        if (direction.normalized.x < 0)
        {
            spd = -Speed;
            Monster.Renderer.flipX = true;
        }
        else
        {
            Monster.Renderer.flipX = false;
        }

        Monster.RB.velocity = new Vector2(spd * Time.deltaTime, Monster.RB.velocity.y);
    }

    private void ChaseGizmos()
    {
        Vector2 from = new Vector2(Monster.transform.position.x - ChaseRange, Monster.transform.position.y);
        Vector2 to = new Vector2(Monster.transform.position.x + ChaseRange, Monster.transform.position.y);

        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(from, to);
    }
}
