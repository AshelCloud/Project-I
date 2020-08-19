using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollBehaviour : StateMachineBehaviour
{

    private Player player = null;

    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter RollBehaviour");
        player = animator.gameObject.GetComponent<Player>();

        player.isInvincible = true;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.direction.x > 0)
        {
            player.rb.AddForce(new Vector2(2f, 0.0f), ForceMode2D.Impulse);
        }

        else
        {
            player.rb.AddForce(new Vector2(2f, 0.0f), ForceMode2D.Impulse);
        }

        player.ChangeDirection();

        animator.SetBool("IsRoll", false);
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit RollBehaviour");
    }
}
