using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


//TODO: 패턴을 delegate로 받아서 랜덤으로 실행
//패턴은 각 몬스터 스크립트에서 넘겨주기로
//
public class MAttack : MBehaviour
{
    private float AttackRange { get; set; }

    public MAttack(Monster monster, float Range) :
        base(monster)
    {
        AttackRange = Range;
    }

    public override void Update()
    {
        //추격하는상태에서만 진행
        if(MObject.CurrentBehaviour != Monster.MonsterBehaviour.Chase) { return; }


    }
}
