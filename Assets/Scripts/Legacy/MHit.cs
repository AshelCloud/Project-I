﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy
{
    public class MHit : MBehaviour
    {
        public Monster Monster { get; private set; }

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
            var curAnimatorStateInfo = Monster.Anim.GetCurrentAnimatorStateInfo(0);

            if (curAnimatorStateInfo.IsName(AnimationName))
            {
                Monster.Renderer.color = Color.red;
            }

            if (curAnimatorStateInfo.normalizedTime >= 1f && curAnimatorStateInfo.IsName(AnimationName))
            {
                Monster.Renderer.color = Color.white;
                Monster.BehaviourStack.Pop();
            }
        }
    }
}