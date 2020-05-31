using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MHit : MBehaviour
{
    public Monster Monster { get; private set; }
    public string AnimationName { get; private set; }

    public MHit(Monster monster, string animationName, params Action[] actions)
    {
        Monster = monster;
        AnimationName = animationName;

        foreach (var action in actions)
        {
            Update += action;
        }

        Update += HitUpdate;
    }

    private void HitUpdate()
    {
    }
}
