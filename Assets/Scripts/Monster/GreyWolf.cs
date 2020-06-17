using UnityEngine;

public class GreyWolf : Monster
{
    protected override void Initialize()
    {
        BehaviourStack.SetPrioritys();
        BehaviourStack.SetAnimationNames("Ready", "Run", "Run", "Bite", "Hit", "Dead");
    }

    protected override void SetBehaviors()
    {
        Behaviours.Add("Patrol", new MPatrol(this, "Run", Data.Speed, 2f));
        Behaviours.Add("Chase", new MChase(this, "Run", Data.Speed, Data.DetectionRange, () => 
        { 
            if(BehaviourStack.Peek() == MonsterBehaviour.Chase) { Anim.speed = 1.5f; }
            else { Anim.speed = 1f; }
        }));
        Behaviours.Add("Hit", new MHit(this, "Hit"));
        Behaviours.Add("Attack", new MAttack(this, "Bite", Data.AttackRange, GetComponentInChildren<MAttackCollider>()));
        Behaviours.Add("Dead", new MDie(this, "Dead"));
    }

    protected override void SetID()
    {
        ID = 1;
    }
}