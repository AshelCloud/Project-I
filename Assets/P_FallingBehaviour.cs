﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FallingBehaviour : StateMachineBehaviour
{
    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter FallingState");

        player = animator.gameObject.GetComponent<Player>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        P_JumpBehaviour.DoubleJump(player, animator);


        if (animator.GetBool("IsGrounded"))
        {

            Debug.Log(P_JumpBehaviour.doubleJump);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit FallingState");

        P_JumpBehaviour.doubleJump = false;

    }
}
