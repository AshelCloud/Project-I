using System;
using UnityEngine;

public class MAttack : MBehaviour
{
    public Monster Monster { get; private set; }
    public string AnimationName { get; private set; }
    public float AttackRange { get; set; }
    public MAttack(Monster monster, string animationName, float range = 0f, params Action[] actions)
    {
        Monster = monster;
        AnimationName = animationName;
        AttackRange = range;

        foreach(var action in actions)
        {
            Update += action;
        }
        OnGizmos += AttackGizmos;
    }

    private void AttackGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = new Vector3(Monster.transform.position.x, Monster.transform.position.y - 0.5f, Monster.transform.position.z);
        Vector3 to = new Vector3(Monster.transform.position.x + (Monster.Renderer.flipX ? -AttackRange : AttackRange), Monster.transform.position.y - 0.5f, Monster.transform.position.z);

        Gizmos.DrawLine(from, to);
    }
}
