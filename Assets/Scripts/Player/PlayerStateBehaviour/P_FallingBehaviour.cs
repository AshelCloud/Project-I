using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FallingBehaviour : StateMachineBehaviour
{
    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter FallingState");

        player = animator.GetComponent<Player>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.ChangeDirection();

        P_JumpBehaviour.DoubleJumping(player, animator);

        if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
        {
            animator.SetBool("IsCling", true);
        }


        player.CheckGround();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit FallingState");
    }
}
