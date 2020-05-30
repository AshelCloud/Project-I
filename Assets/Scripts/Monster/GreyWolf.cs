using UnityEngine;

public class GreyWolf : Monster
{
    protected override void SetBehaviors()
    {
        Behaviours.Add("Patrol", new MPatrol(this, Data.Speed, 2f));
        Behaviours.Add("Chase", new MChase(this, Data.Speed, Data.DetectionRange));
        Behaviours.Add("Attack", new MAttack(this, Data.AttackRange, Attack));
    }
    protected override void SetID()
    {
        ID = 1;
    }
    public void Attack()
    {
        Vector2 end = new Vector2(transform.position.x + Data.AttackRange, transform.position.y);

        var results = Physics2D.LinecastAll(transform.position, end);

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
            CurrentBehaviour = MonsterBehaviour.Run;
            return;
        }

        CurrentBehaviour = MonsterBehaviour.Attack;

        Anim.Play("Bite");
        Anim.speed = 1f;
    }
}