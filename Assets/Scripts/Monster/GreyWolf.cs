using UnityEngine;

public class GreyWolf : Monster
{
    protected override void SetBehaviors()
    {
        Behaviours.Add("Patrol", new MPatrol(this, "Run", Data.Speed, 2f));
        Behaviours.Add("Chase", new MChase(this, "Run", Data.Speed, Data.DetectionRange, () => 
        { 
            if(CurrentBehaviour == MonsterBehaviour.Chase) { print("1.5f"); Anim.speed = 1.5f; }
        }));
        Behaviours.Add("Attack", new MAttack(this, "Bite", Data.AttackRange, Attack));
        Behaviours.Add("Dead", new MDie(this, "Dead"));
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
            return;
        }

        CurrentBehaviour = MonsterBehaviour.Attack;

        Anim.Play("Bite");
    }
}