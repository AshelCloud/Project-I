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
        Behaviours.Add(new MPatrol(this, Data.Speed));
        Behaviours.Add(new MChase(this, Data.Speed, Data.DetectionRange));
        Behaviours.Add(new MAttack(this, 10f));
    }

    protected override void SetID()
    {
        ID = 1;
    }
}
