using System;
using UnityEngine;

public class MAttack : MBehaviour
{
    private Monster Monster { get; set; }
    private float AttackRange { get; set; }
    public MAttack(Monster monster, float range, params Action[] actions)
    {
        Monster = monster;
        AttackRange = range;

        foreach(var action in actions)
        {
            Update += action;
        }
        OnGizmos = AttackGizmos;
    }

    public void AttackGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = new Vector3(Monster.transform.position.x, Monster.transform.position.y - 0.5f, Monster.transform.position.z);
        Vector3 to = new Vector3(Monster.transform.position.x + (Monster.Renderer.flipX ? -AttackRange : AttackRange), Monster.transform.position.y - 0.5f, Monster.transform.position.z);

        Gizmos.DrawLine(from, to);
    }
}
