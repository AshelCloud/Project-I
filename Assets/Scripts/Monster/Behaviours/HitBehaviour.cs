using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehaviour : StateMachineBehaviour
{
    private Monster myMonster;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myMonster = animator.GetComponent<Monster>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myMonster.Damaged = false;
    }
}
