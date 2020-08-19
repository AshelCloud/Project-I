using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    private Player player;

    private bool doubleJump = false;

    private bool leftMove = false;
    private bool rightMove = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter JumpBehaviour");

        player = animator.gameObject.GetComponent<Player>();

        if(player.isGrounded())
        {
            player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }

        else
        {

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!player.isGrounded())
        {
            //좌측이동
            if (Input.GetKey(KeyCode.RightArrow) && !leftMove)
            {
                rightMove = true;
                player.ChangeDirection();
                //플레이어 좌측 이동
                player.rb.AddForce(new Vector2(player.Speed, 0.0f), ForceMode2D.Force);
            }

            //우측이동
            else if (Input.GetKey(KeyCode.LeftArrow) && !rightMove)
            {
                leftMove = true;
                player.ChangeDirection();
                //플레이어 우측 이동
                player.rb.AddForce(new Vector2(-player.Speed, 0.0f), ForceMode2D.Force);
            }

            //제자리 점프
            else
            {
                player.ChangeDirection();
            }

            if (Input.GetKeyDown(KeyCode.D) && !doubleJump)
            {
                Log.Print("Player do double jump");

                animator.Play("Jump");

                //더블 점프 전 y축 속도 0 설정, 벡터 합력으로 인한 슈퍼점프 방지
                player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);
                player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                doubleJump = true;
            }
        }

        else
        {
            //더블 점프 횟수 초기화
            doubleJump = false;

            animator.SetBool("IsJump", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit JumpBehaviour");

        rightMove = false;
        leftMove = false;

        player.isJumpDown = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
