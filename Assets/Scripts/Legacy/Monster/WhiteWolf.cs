using Legacy;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Legacy
{
public class WhiteWolf : Monster
{
    private Player _player;
    private Player Player
    {
        get
        {
            if(_player == null)
            {
                _player = FindObjectOfType(typeof(Player)) as Player;
            }
            
            return _player;
        }
    }


    private Collider2D BiteCollider { get; set; }
    private Collider2D ClawCollider { get; set; }
    private int AttackCount { get; set; } = 0;

    public float attackRate = 0.7f;
    private float AttackTime { get; set; }

    private bool IsAttacking { get; set; } = false;

    protected override void SetID()
    {
        ID = 2;
    }

    protected override void SetBehaviourStackSettings()
    {
        BehaviourStack.SetPrioritys();

        //넣지않은 애니메이션: Claw
        BehaviourStack.SetAnimationNames("Ready", "Run", "Run", "Bite", "Hit", "Dead");
    }


    protected override void ChaseUpdateBehaviour()
    {
        if( BehaviourStack.Push(MonsterBehaviour.Chase) == false)
        {
            return;
        }

        Vector3 direction = Player.transform.position - transform.position;
        var curSclae = transform.localScale;

        float spd = Speed;
        if (direction.normalized.x < 0)
        {
            spd = -Speed;
            if (curSclae.x > 0f)
            {
                transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);
            }
        }
        else
        {
            if (curSclae.x < 0f)
            {
                transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);
            }
        }

        //추적은 1.5배 빠르게 이동
        RB.velocity = new Vector2(spd * 1.5f * Time.deltaTime, RB.velocity.y);
    }

    protected override void AttackStartBehaviour()
    {
        foreach(var col in GetComponentsInChildren<MAttackCollider>())
        {
            if(col.attackName.Contains("Bite"))
            {
                BiteCollider = col.attackCollider;
            }
            else if(col.attackName.Contains("Claw"))
            {
                ClawCollider = col.attackCollider;
            }
        }

        BiteCollider.enabled = false;
        ClawCollider.enabled = false;

        AttackTime = 0;
    }

    protected override void AttackUpdateBehaviour()
    {
        Vector2 start = new Vector2(transform.position.x, transform.position.y - 1f);
        Vector2 end = new Vector2(transform.position.x + AttackRange * transform.lossyScale.x, transform.position.y - 1f);

        var p = FindPlayer(start, end);
        if (p == null)
        {
            if (BehaviourStack.Peek() == MonsterBehaviour.Attack)
            {
                BehaviourStack.Pop();
            }

            BiteCollider.enabled = false;
            ClawCollider.enabled = false;
            return;
        }
        if (BehaviourStack.Push(MonsterBehaviour.Attack) == false)
        {
            BiteCollider.enabled = false;
            ClawCollider.enabled = false;
            return;
        }

        RB.velocity = Vector2.zero;

        var curAnimatorStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        float curAnimatorNormalizedTime = curAnimatorStateInfo.normalizedTime - (int)curAnimatorStateInfo.normalizedTime;

        Collider2D CurrentAttackCollider;
        if (curAnimatorNormalizedTime > 0.8f && IsAttacking == false)
        {
            if(AttackCount % 4 == 0)
            {
                BehaviourStack.SetAnimationName(MonsterBehaviour.Attack, "Claw");
                CurrentAttackCollider = ClawCollider;
            }
            else
            {
                BehaviourStack.SetAnimationName(MonsterBehaviour.Attack, "Bite");
                CurrentAttackCollider = BiteCollider;
            }
            AttackCount++;

            CurrentAttackCollider.enabled = true;

            IsAttacking = true;
        }
        else
        {
            if (IsAttacking == true)
            {
                AttackTime += Time.deltaTime;

                if (AttackTime > attackRate)
                {
                    IsAttacking = false;
                    AttackTime = 0;
                }
                else
                {
                    Anim.Play(BehaviourStack.AnimationNames[MonsterBehaviour.Idle]);
                }
            }

            BiteCollider.enabled = false;
            ClawCollider.enabled = false;
        }
    }

    protected override void HitUpdateBehaviour()
    {
        var curAnimatorStateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimatorStateInfo.IsName(BehaviourStack.AnimationNames[MonsterBehaviour.Hit]))
        {
            Renderer.color = Color.red;
        }

        if (curAnimatorStateInfo.normalizedTime >= 1f && curAnimatorStateInfo.IsName(BehaviourStack.AnimationNames[MonsterBehaviour.Hit]))
        {
            Renderer.color = Color.white;
            BehaviourStack.Pop();
        }
    }
    protected override void DeadUpdateBehaviour()
    {
        if (HP > 0) { return; }

        BehaviourStack.Push(MonsterBehaviour.Dead);

        var curAnimatorStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        //TODO: 애니메이션 끝난뒤 오브젝트 삭제
        if (curAnimatorStateInfo.normalizedTime > 1f && curAnimatorStateInfo.IsName(BehaviourStack.AnimationNames[MonsterBehaviour.Dead]))
        {
            //TODO: FadeOut으로 교체
            DestroyObject();
        }
    }

    protected override void AttackOnGizmosBehaviour()
    {
        Gizmos.color = Color.red;

        Vector3 from = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        Vector3 to = new Vector3(transform.position.x + AttackRange * transform.lossyScale.x, transform.position.y - 1f, transform.position.z);

        Gizmos.DrawLine(from, to);
    }
}
}