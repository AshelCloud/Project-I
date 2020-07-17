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
}
