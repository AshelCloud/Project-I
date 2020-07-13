using UnityEngine;

public class GreyWolf : Monster
{
    [Header("Patrol Time")]
    public float moveTime = 2f;
    //Patrol 시간 체크용
    private float StartTime { get; set; }
    //몇초 마다 방향을 바꿀껀지
    private Collider2D AttackCollider { get; set; }

    protected override void SetBehaviourStackSettings()
    {
        BehaviourStack.SetPrioritys();
        BehaviourStack.SetAnimationNames("Ready", "Run", "Run", "Bite", "Hit", "Dead");
    }

    protected override void SetID()
    {
        ID = 1;
    }

    protected override void RunStartBehaviour()
    {
        StartTime = Time.time;
    }

    protected override void RunUpdateBehaviour()
    {
        if(BehaviourStack.Peek() != MonsterBehaviour.Run) { return; }

        var curSclae = transform.localScale;

        if (Time.time - StartTime >= moveTime)
        {
            StartTime = Time.time;
            transform.localScale = new Vector3(-curSclae.x, curSclae.y, curSclae.z);
        }
        int direction = curSclae.x < 0f ? -1 : 1;

        RB.velocity = new Vector2(Speed * direction * Time.deltaTime, RB.velocity.y);
    }

    protected override void ChaseUpdateBehaviour()
    {
        Vector2 start = new Vector2(transform.position.x - DetectionRange, transform.position.y);
        Vector2 end = new Vector2(transform.position.x + DetectionRange, transform.position.y);

        var player = FindPlayer(start, end);
        if (player == null)
        {
            if(BehaviourStack.Peek() == MonsterBehaviour.Chase)
            {
                BehaviourStack.Pop();
            }
            Anim.speed = 1f;

            return;
        }
        if( BehaviourStack.Push(MonsterBehaviour.Chase) == false )
        {
            Anim.speed = 1f;
            return;
        }

        Anim.speed = 1.5f;
        Vector3 direction = player.position - transform.position;
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
    protected override void ChaseOnGizmosBehaviour()
    {
        Vector2 from = new Vector2(transform.position.x - DetectionRange, transform.position.y);
        Vector2 to = new Vector2(transform.position.x + DetectionRange, transform.position.y);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(from, to);
    }
    protected override void AttackStartBehaviour()
    {
        AttackCollider = GetComponentInChildren<MAttackCollider>().attackCollider;
        AttackCollider.enabled = false;
    }
    protected override void AttackUpdateBehaviour()
    {
        Vector2 start = transform.position;
        Vector2 end = new Vector2(transform.position.x + AttackRange * transform.lossyScale.x, transform.position.y);

        var player = FindPlayer(start, end);
        if(player == null)
        {
            if(BehaviourStack.Peek() == MonsterBehaviour.Attack)
            {
                BehaviourStack.Pop();
            }
            
            AttackCollider.enabled = false;
            return;
        }
        if( BehaviourStack.Push(MonsterBehaviour.Attack) == false )
        {
            AttackCollider.enabled = false;
            return;
        }


        RB.velocity = Vector2.zero;

        var curAnimatorStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        float curAnimatorNormalizedTime = curAnimatorStateInfo.normalizedTime - (int)curAnimatorStateInfo.normalizedTime;

        if (curAnimatorNormalizedTime > 0.8f)
        {
            AttackCollider.enabled = true;
        }
        else
        {
            AttackCollider.enabled = false;
        }
    }
    protected override void AttackOnGizmosBehaviour()
    {
        Gizmos.color = Color.red;

        Vector3 from = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        Vector3 to = new Vector3(transform.position.x + AttackRange * transform.lossyScale.x, transform.position.y - 0.5f, transform.position.z);

        Gizmos.DrawLine(from, to);
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
    //TODO: 죽는건 상위 클래스에서 처리 하도록 바꾸기
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
}