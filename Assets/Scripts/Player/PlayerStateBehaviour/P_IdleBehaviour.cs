using UnityEngine;

public class P_IdleBehaviour : StateMachineBehaviour
{
    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter IdleState");
        player = animator.gameObject.GetComponent<Player>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("IsRun", true);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsAttack", true);
        }

        else if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.DownArrow))
        {
            player.isJumpDown = true;
            animator.SetBool("IsJump", true);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("IsJump", true);
        }

        else if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("IsRoll", true);
        }


    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit IdleState");
    }
}