﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MDie : MBehaviour
{
    public Monster Monster { get; private set; }

    public string AnimationName { get; private set; }

    public MDie(Monster monster, string animationName, params Action[] actions)
    {
        Monster = monster;
        AnimationName = animationName;

        foreach(var action in actions)
        {
            Update += action;
        }

        Update += DieUpdate;
    }

    private void DieUpdate()
    {
        if(Monster.HP > 0) { return; }

         Monster.Anim.Play(AnimationName);

         //TODO: 애니메이션 끝난뒤 오브젝트 삭제
    }
}