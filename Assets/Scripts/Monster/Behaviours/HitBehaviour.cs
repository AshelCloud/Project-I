using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class HitBehaviour : StateMachineBehaviour
    {
        private Monster myMonster;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            myMonster = animator.GetComponent<Monster>();

            myMonster._Renderer.color = Color.red;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            myMonster.Damaged = false;

            myMonster._Renderer.color = Color.white;
        }
    }
}