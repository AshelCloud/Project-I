using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    private Player player;

    private bool doubleJump = false;

    private bool leftMove = false;
    private bool rightMove = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter JumpBehaviour");

        player = animator.gameObject.GetComponent<Player>();

        if(player.isGrounded() && player.isJumpDown)
        {
            player.Platform.rotationalOffset = 180;
        }

        else if(player.isGrounded())
        {
            player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }

        else
        {
            return;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player.isGrounded())
        {
            if (Input.GetKeyDown(KeyCode.D) && !doubleJump)
            {
                Log.Print("Player do double jump");

                doubleJump = true;

                animator.Play("Jump");

                //더블 점프 전 y축 속도 0 설정, 벡터 합력으로 인한 슈퍼점프 방지
                player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);
                player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
            }

            if(Input.GetKeyDown(KeyCode.S) && player.CheckWall())
            {
                animator.SetBool("IsCling", true);
            }


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
        }

        else
        {
            animator.SetBool("IsJump", false);
            doubleJump = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit JumpBehaviour");

        rightMove = false;
        leftMove = false;

        player.isJumpDown = false;
    }
}
