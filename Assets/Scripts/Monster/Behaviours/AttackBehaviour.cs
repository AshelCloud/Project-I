using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations;

public class AttackBehaviour : StateMachineBehaviour
{
    private Monster monster;

    private float startAttackTime;
    private float attackDelay;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        monster.IsAttacking = true;
        monster.Attack = false;

        attackDelay = monster.attackDelay;
        startAttackTime = Time.time;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.IsAttacking = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.IsAttacking = false;
    }
}
