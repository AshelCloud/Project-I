using UnityEngine;

//!Edit
public class P_IdleBehaviour : StateMachineBehaviour
{
    private Player player = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter IdleState");
        //player = animator.gameObject.GetComponent<Player>();
        player = animator.GetComponent<Player>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            //animator.SetBool("IsRun", true);
            player.Run = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            //animator.SetBool("IsAttack", true);
            player.Attack = true;
        }

        else if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.DownArrow))
        {
            player.isJumpDown = true;
            //animator.SetBool("IsJump", true);
            player.Jump = true;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            //animator.SetBool("IsJump", true);
            player.Jump = true;
        }

        else if (Input.GetKeyDown(KeyCode.F))
        {
            //animator.SetBool("IsRoll", true);
            player.Roll = true;
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit IdleState");
    }
}