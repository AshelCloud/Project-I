using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GreyWolf : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetBehaviors()
    {
        Behaviours.Add(new MPatrol(this, Data.Speed * 25));
        Behaviours.Add(new MChase(this, Data.Speed * 35, 10f));
        Behaviours.Add(new MAttack(this, 10f));
    }

    protected override void SetID()
    {
        ID = 1;
    }
}
