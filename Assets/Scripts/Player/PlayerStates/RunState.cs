using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 이동 상태
public class RunState : IPlayerState
{
    private Player player;
    private Vector2 direction;
    private Vector2 movement;

    private bool isOnSlope;
    Vector2 slopeNormalPerp;
    float slopeDownAngle;
    float slopeDownAngleOld;

    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter RunState");
        this.player = player;
        direction = player.transform.localScale;
    }

    void IPlayerState.Update()
    {
        SlopeCheck();
        player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.Speed, player.rb.velocity.y);

        //좌측이동
        if (Input.GetAxis("Horizontal") < 0)
        {
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
        }

        //우측이동
        else
        {
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
        }
        player.transform.localScale = direction;

        player.anim.Play("Run");

        if (player.isGrounded() && !isOnSlope)
        {
            player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.Speed, 0.0f);
        }

        else if (player.isGrounded() && isOnSlope)
        {
            player.rb.velocity = new Vector2(-Input.GetAxis("Horizontal") * player.Speed * slopeNormalPerp.x,
                                             -Input.GetAxis("Horizontal") * player.Speed * slopeNormalPerp.y);
        }

        else
        {
            player.SetState(new JumpState());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            player.SetState(new AttackState());
        }

        //점프
        else if (Input.GetKeyDown(KeyCode.D))
        {
            player.SetState(new JumpState());
        }

        //구르기
        else if (Input.GetKeyDown(KeyCode.F))
        {
            player.SetState(new RollState());
        }

        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
        Log.Print("Enter ExitState");
    }

    public void SlopeCheck()
    {
        Vector2 checkPos = player.transform.position + new Vector3(0.0f, player.cc2D.size.y / 4);

        //SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        float slopeCheckDistance = 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, LayerMask.GetMask("Floor"));

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {

                isOnSlope = true;
            }

            //else
            //{
            //    isOnSlope = false;
            //    Log.Print("On Slope state: " + isOnSlope);
            //}

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);

            Debug.DrawRay(hit.point, hit.normal, Color.blue);
        }
    }
}