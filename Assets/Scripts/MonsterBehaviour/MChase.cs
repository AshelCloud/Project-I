using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class MChase : MBehaviour
{
    private float Speed { get; set; }
    private float ChaseRange { get; set; }

    public MChase(Monster monster, float speed, float Range):
        base(monster)
    {
        Speed = speed * (MObject.Renderer.flipX ? -1 : 1);
        ChaseRange = Range * (MObject.Renderer.flipX ? -1 : 1);
    }

    public override void Update()
    {
        if(MObject.CurrentBehaviour == Monster.MonsterBehaviour.Attack) { return; }

        GameObject player = null;

        Vector2 range = new Vector2(MObject.transform.position.x + ChaseRange, MObject.transform.position.y);
        var results = Physics2D.LinecastAll(new Vector2(MObject.transform.position.x - ChaseRange, MObject.transform.position.y),  range);

        foreach(var result in results)
        {
            if(result.transform.CompareTag("Player"))
            {
                player = result.transform.gameObject;
                break;
            }
        }

        if(player == null) 
        {
            MObject.Anim.speed = 1f;
            MObject.CurrentBehaviour = Monster.MonsterBehaviour.Run;
            return; 
        }

        MObject.CurrentBehaviour = Monster.MonsterBehaviour.Chase;

        MObject.Anim.Play("Run");
        MObject.Anim.speed = 1.5f;

        Vector3 direction = player.transform.position - MObject.transform.position;

        Debug.Log(direction.normalized.x);

        float spd = Speed;
        if(direction.normalized.x < 0)
        {
            spd = -Speed;
            MObject.Renderer.flipX = true;
        }
        else
        {
            MObject.Renderer.flipX = false;
        }

        MObject.RB.velocity = new Vector2(spd * Time.deltaTime, MObject.RB.velocity.y);
    }

    public override void OnGizmo()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(MObject.transform.position.x - ChaseRange, MObject.transform.position.y, MObject.transform.position.z), new Vector3(MObject.transform.position.x + ChaseRange, MObject.transform.position.y, MObject.transform.position.z));
    }
}
