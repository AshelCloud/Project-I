using System;
using UnityEngine;

public class MAttack : MBehaviour
{
    public Monster Monster { get; private set; }
    public string AnimationName { get; private set; }
    public float AttackRange { get; set; }
    Collider2D Collider { get; set; }
    public float AttackCount { get; set; } = 1f;

    public MAttack(Monster monster, string animationName, float range = 0f, Collider2D collider = null, params Action[] actions)
    {
        Monster = monster;
        AnimationName = animationName;
        AttackRange = range;
        Collider = collider;

        Start += AttackStart;
        Update += AttackUpdate;
        foreach(var action in actions)
        {
            Update += action;
        }
        OnGizmos += AttackGizmos;
    }

    private void AttackStart()
    {
        EnableColliders(false);
    }

    private void AttackUpdate()
    {
        Vector2 end = new Vector2(Monster.transform.position.x + AttackRange, Monster.transform.position.y);

        var results = Physics2D.LinecastAll(Monster.transform.position, end);

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
            if (Monster.BehaviourStack.Peek() == Monster.MonsterBehaviour.Attack)
            {
                Monster.BehaviourStack.Pop();
            }
            return;
        }
        else if (Monster.BehaviourStack.Peek() != Monster.MonsterBehaviour.Attack)
        {
            Monster.BehaviourStack.Push(Monster.MonsterBehaviour.Attack);
        }

        Monster.Anim.Play(AnimationName);

        //플레이어 피격테스트
        //수정요망
        //player.GetComponent<Player>().HitByMonster(0);

        //!< 폐기코드
        //if(Monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime / AttackCount > 0.8f)
        //{
        //    EnableColliders(true);
        //    AttackCount ++;
        //    Debug.Log("NormalizedTime: " + Monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //}
        //else
        //{
        //    EnableColliders(false);
        //}
    }

    private void EnableColliders(bool enable)
    {
        Collider.enabled = enable;
    }

    private void AttackGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 from = new Vector3(Monster.transform.position.x, Monster.transform.position.y - 0.5f, Monster.transform.position.z);
        Vector3 to = new Vector3(Monster.transform.position.x + (Monster.Renderer.flipX ? -AttackRange : AttackRange), Monster.transform.position.y - 0.5f, Monster.transform.position.z);

        Gizmos.DrawLine(from, to);
    }
}
