using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyWolf : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetBehaviors()
    {
        Behaviours.Add(new MPatrol());
    }

    protected override void SetID()
    {
        ID = 1;
    }
}
