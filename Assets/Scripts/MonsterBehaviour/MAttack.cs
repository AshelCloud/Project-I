using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


//TODO: 패턴을 delegate로 받아서 랜덤으로 실행
//패턴은 각 몬스터 스크립트에서 넘겨주기로
//
public class MAttack : MBehaviour
{
    private float AttackRange { get; set; }
    private Action Action { get; set; }

    public MAttack(Monster monster, float range, params Action[] actions) :
        base(monster)
    {
        AttackRange = range * (MObject.Renderer.flipX ? -1 : 1);

        for(int i = 0; i < actions.Length; i ++)
        {
            Action += actions[i];
        }
    }

    public override void Update()
    {
        if(Action == null) { return; }

        Action();
    }

    public override void OnGizmo()
    {
        Gizmos.color = Color.red;

        float range = AttackRange * (MObject.Renderer.flipX ? -1 : 1);
        Gizmos.DrawLine(new Vector3(MObject.transform.position.x, MObject.transform.position.y - 0.5f, MObject.transform.position.z), new Vector3(MObject.transform.position.x + range, MObject.transform.position.y - 0.5f, MObject.transform.position.z));

    }
}
