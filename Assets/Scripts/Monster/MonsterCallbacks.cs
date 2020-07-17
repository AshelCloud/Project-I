using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 몬스터 스크립트의 콜백함수
public partial class Monster : MonoBehaviour
{
    public virtual void SetAttack()
    {
        Attack = true;
    }

    public virtual void SetIdle(bool value)
    {
        idle = value;
    }

    public virtual void GetDamaged(float value)
    {
        //TODO: 데미지 입는거 구현 (애니메이션 포함)
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
}
