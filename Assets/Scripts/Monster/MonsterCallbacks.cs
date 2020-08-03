using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 몬스터 스크립트의 콜백함수
public partial class Monster : MonoBehaviour
{
    public virtual void SetAttack()
    {
        attackID = -1;
        Attack = true;
    }

    public virtual void SetAttack(int id)
    {
        attackID = id;
        Attack = true;
    }

    public virtual int SetRandomAttackID()
    {
        return Random.Range(1, totalAttack + 1);
    }

    public virtual void SetIdle(bool value)
    {
        idle = value;
    }

    public virtual void GetDamaged(float value)
    {
        if (dead) { return; }

        hp -= value;

        if (hp > 0f)
        {
            damaged = true;
        }
        else
        {
            SetDead();
        }
    }

    public virtual void SetChase(bool value)
    {
        chase = value;
    }

    public virtual void AttackTrigger(int triggerIndex)
    {
        if(triggerIndex == -1)
        {
            foreach(AttackTrigger trigger in AttackTriggers)
            {
                trigger.Collider.enabled = true;
                trigger.gameObject.SetActive(true);
            }

            return;
        }

        if(triggerIndex == 0)
        {
            foreach(AttackTrigger trigger in AttackTriggers)
            {
                trigger.Collider.enabled = false;
                trigger.gameObject.SetActive(false);
            }

            return;
        }

        List<AttackTrigger> At_L = AttackTriggers.FindAll(item => item.index == triggerIndex);

        if(At_L != null)
        {
            foreach(AttackTrigger trigger in At_L)
            {
                trigger.Collider.enabled = true;
                trigger.gameObject.SetActive(true);
            }
        }
    }

    public virtual void SetDead()
    {
        dead = true;

        Anim.SetTrigger(hash_Dead);
    }

    public virtual void SetTurn()
    {
        Vector3 scale = transform.lossyScale;
        scale.x = -scale.x;

        transform.localScale = scale;
    }
}
