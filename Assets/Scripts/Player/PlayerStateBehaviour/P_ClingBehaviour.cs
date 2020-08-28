using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ClingBehaviour : StateMachineBehaviour
{

    private Player player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter ClingState");

        player = animator.gameObject.GetComponent<Player>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.rb.velocity = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.rb.AddForce(Vector2.up * player.JumpForce * 1.25f, ForceMode2D.Impulse);

            animator.SetBool("IsCling", false);
            animator.SetBool("IsJump", true);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit ClingState");
    }
}
