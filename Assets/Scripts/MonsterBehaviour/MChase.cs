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
        GameObject player = null;

        ChaseRange = ChaseRange * (MObject.Renderer.flipX ? -1 : 1);

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

        if(player == null) { return; }

        MObject.CurrentBehaviour = Monster.MonsterBehaviour.Chase;

        float spd = Speed;
        if(MObject.Renderer.flipX)
        {
            spd = -Speed;
        }

        MObject.RB.velocity = new Vector2(spd * Time.deltaTime, MObject.RB.velocity.y);
    }

    public override void OnGizmo()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(MObject.transform.position.x - ChaseRange, MObject.transform.position.y, MObject.transform.position.z), new Vector3(MObject.transform.position.x + ChaseRange, MObject.transform.position.y, MObject.transform.position.z));
    }
}
