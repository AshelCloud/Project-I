﻿using System;
namespace Legacy
{
    public class MDie : MBehaviour
    {
        public Monster Monster { get; private set; }

        public MDie(Monster monster, string animationName, params Action[] actions)
        {
            Monster = monster;
            AnimationName = animationName;

            foreach (var action in actions)
            {
                Update += action;
            }

            Update += DieUpdate;
        }

        private void DieUpdate()
        {
            if (Monster.HP > 0) { return; }

            Monster.BehaviourStack.Push(MonsterBehaviour.Dead);

            //TODO: 애니메이션 끝난뒤 오브젝트 삭제
            if (Monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && Monster.Anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                //TODO: FadeOut으로 교체
                Monster.DestroyObject();
            }
        }
    }
}