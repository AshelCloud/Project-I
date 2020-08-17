using UnityEngine;

//플레이어 점프 상태
public class JumpState : IPlayerState
{
    private Player player;
    private Vector2 direction;
    private bool doubleJump = false;

    private bool leftMove = false;
    private bool rightMove = false;

    float timer = 0.0f;
    float delay = 0.05f;
    public void OnEnter(Player player)
    {
        Log.Print("Enter JumpState");

        this.player = player;
        direction = this.player.transform.localScale;

        //벽에 매달린 상태의 점프
        if (player.isCling && !player.isGrounded())
        {
            Log.Print("Player Cling jump");
            this.player.rb.AddForce(Vector2.up * player.JumpForce * 1.25f, ForceMode2D.Impulse);
            player.isCling = false;
        }

        //공중에 있을 때
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
    public void Update()
    {
        //로직 지연 처리 하여 버그 방지
        timer += Time.deltaTime;
        if (timer > delay)
        {
            //공중 상태
            if (!player.isGrounded() && !player.isHit)
            {
                PlayJumpAnim();

                //벽에 매달리기
                if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
                {
                    player.SetState(new ClingState());
                }

                //더블 점프
                if (Input.GetKeyDown(KeyCode.D) && !doubleJump)
                {
                    Log.Print("Player do double jump");

                    //더블 점프 전 y축 속도 0 설정, 벡터 합력으로 인한 슈퍼점프 방지
                    player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);
                    player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                    doubleJump = true;
                }

                //좌측이동
                if (Input.GetKey(KeyCode.RightArrow) && !leftMove)
                {
                    rightMove = true;
                    ChangeDirection();
                    //플레이어 좌측 이동
                    player.rb.AddForce(new Vector2(player.Speed, 0.0f), ForceMode2D.Force);

                    if (player.rb.velocity.x >= 10f)
                    {
                        player.rb.velocity = new Vector2(10f, player.rb.velocity.y);
                    }
                }

                //우측이동
                else if (Input.GetKey(KeyCode.LeftArrow) && !rightMove)
                {
                    leftMove = true;
                    ChangeDirection();
                    //플레이어 우측 이동
                    player.rb.AddForce(new Vector2(-player.Speed, 0.0f), ForceMode2D.Force);

                    if(player.rb.velocity.x <= -10f)
                    {
                        player.rb.velocity = new Vector2(-10f, player.rb.velocity.y);
                    }
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
                doubleJump = false;

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

    public void OnExit()
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
