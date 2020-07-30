using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 점프 상태
public class JumpState : IPlayerState
{
    private Player player;
    private Vector2 direction;
    static private uint doubleJump = 0;

    private bool leftMove = false;
    private bool rightMove = false;

    float timer = 0.0f;
    float delay = 0.05f;
    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter JumpState");

        this.player = player;
        direction = this.player.transform.localScale;

        this.player.rb.velocity /= 2;

        //벽에 매달린 상태의 점프
        if (player.isCling && !player.isGrounded())
        {
            doubleJump++;
            Log.Print("Player Cling jump");
            this.player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
            player.isCling = false;
        }

        //공중에 있을 때 점프 단계 건너뛰게 한다.
        if (!this.player.isGrounded())
        {
            return;
        }

        //바닥에 있을 때
        else
        {
            //일반적인 점프
            if (!this.player.isJumpDown)
            {
                this.player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                doubleJump++;
                Log.Print("Jump Count: " + doubleJump);
            }

            //하향 점프
            else
            {
                if (player.isGrounded())
                {
                    //Platform Effecter Offset 변경되며 플랫폼 아래로 내려감
                    player.Platform.rotationalOffset = 180;
                }

                else
                {

                }
            }
        }
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        //0.05초뒤 로직 처리 하여 버그 방지
        timer += Time.deltaTime;
        if (timer > delay)
        {
            //공중 상태
            if (!player.isGrounded() && !player.isHit)
            {
                PlayJumpAnim();

                //더블 점프
                if (Input.GetKeyDown(KeyCode.D) && doubleJump < 2)
                {
                    player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                    Log.Print("Jump Count: " + doubleJump);
                    doubleJump++;
                }

                //벽에 매달리기
                if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
                {
                    doubleJump = 0;
                    player.SetState(new ClingState());
                }

                //좌측이동
                if (Input.GetKey(KeyCode.RightArrow) && !leftMove)
                {
                    rightMove = true;
                    ChangeDirection();
                    //플레이어 좌측 이동
                    player.rb.AddForce(new Vector2(player.Speed / 2, 0.0f), ForceMode2D.Force);
                }

                //우측이동
                else if (Input.GetKey(KeyCode.LeftArrow) && !rightMove)
                {
                    leftMove = true;
                    ChangeDirection();
                    //플레이어 우측 이동
                    player.rb.AddForce(new Vector2(-player.Speed / 2, 0.0f), ForceMode2D.Force);
                }

                //제자리 점프
                else
                {
                    ChangeDirection();
                }
            }

            //땅에 착지
            else
            {
                //더블 점프 횟수 초기화
                doubleJump = 0;

                //이동
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    player.SetState(new RunState());
                }

                //공격
                else if (Input.GetKey(KeyCode.A))
                {
                    player.SetState(new AttackState());
                }

                //구르기
                else if (Input.GetKey(KeyCode.F))
                {
                    player.SetState(new RollState());
                }

                //아무 행동도 취하지 않을시 대기
                else
                {
                    player.SetState(new IdleState());
                }
            }
        }
    }

    void IPlayerState.OnExit()
    {
        rightMove = false;
        leftMove = false;

        Log.Print("End JumpState");
        player.isJumpDown = false;
    }

    //점프 애니메이션 재생
    private void PlayJumpAnim()
    {
        //점프 시작
        if (player.rb.velocity.y >= 8f)
        {
            player.anim.Play("Jump", 0, 0.2f);
        }

        //공중
        else if (player.rb.velocity.y < 8f && player.rb.velocity.y > 0f)
        {
            player.anim.Play("Jump", 0, 0.4f);
        }

        //하강
        else if (player.rb.velocity.y < 0f)
        {
            player.anim.Play("Jump", 0, 0.6f);
        }

        else
        {
            return;
        }
    }

    private void ChangeDirection()
    {
        //좌측이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환

        }

        //우측이동
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
        }

        player.transform.localScale = direction;
    }
}
