using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class MChase : MBehaviour
{
    private Monster Monster { get; set; }
    private float Speed { get; set; }
    private float ChaseRange { get; set; }

    public MChase(Monster monster, float speed = 0f, float range = 0f)
    {
        Monster = monster;
        Speed = speed;
        ChaseRange = range;

        Update = ChaseUpdate;
        OnGizmos = ChaseGizmos;
    }
    private void ChaseUpdate()
    {
        if (Monster.CurrentBehaviour == Monster.MonsterBehaviour.Attack) { return; }

        GameObject player = null;

        Vector2 start = new Vector2(Monster.transform.position.x - ChaseRange, Monster.transform.position.y);
        Vector2 end = new Vector2(Monster.transform.position.x + ChaseRange, Monster.transform.position.y);

        var results = Physics2D.LinecastAll(start, end);

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
            Monster.Anim.speed = 1f;
            Monster.CurrentBehaviour = Monster.MonsterBehaviour.Run;
            return;
        }

        Monster.CurrentBehaviour = Monster.MonsterBehaviour.Chase;

        Monster.Anim.Play("Run");
        Monster.Anim.speed = 1.5f;

        Vector3 direction = player.transform.position - Monster.transform.position;

        Debug.Log(direction.normalized.x);

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
        Debug.Log("Run!");

        Vector2 from = new Vector2(Monster.transform.position.x - ChaseRange, Monster.transform.position.y);
        Vector2 to = new Vector2(Monster.transform.position.x + ChaseRange, Monster.transform.position.y);

        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(from, to);
    }
}
