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

    public virtual void AttackTrigger(int triggerIndex)
    {
        if(triggerIndex == -1)
        {
            foreach(AttackTrigger trigger in AttackTriggers)
            {
                trigger.Collider.enabled = true;
            }

            return;
        }

        if(triggerIndex == 0)
        {
            foreach(AttackTrigger trigger in AttackTriggers)
            {
                trigger.Collider.enabled = false;
            }   

            return;
        }

        List<AttackTrigger> At_L = AttackTriggers.FindAll(item => item.index == triggerIndex);

        if(At_L != null)
        {
            foreach(AttackTrigger trigger in At_L)
            {
                trigger.Collider.enabled = true;
            }
        }
    }
}
