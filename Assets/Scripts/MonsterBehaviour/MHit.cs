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
        if (Monster.BehaviourStack.Peek() == Monster.MonsterBehaviour.Dead) { return; }

        if (Monster.BehaviourStack.Peek() != Monster.MonsterBehaviour.Hit) { return; }
        if(0 >= Monster.HP) { return; }

        Monster.Anim.Play(AnimationName);

        if(Monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            Monster.BehaviourStack.Pop();
        }
    }
}
