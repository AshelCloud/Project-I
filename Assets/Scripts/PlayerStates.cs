﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

/*
 **********************************************
    
    <차후 해결 사항들>
    코루틴, 델리게이트 등을 사용하여 수정할 예정
    중복 되는 코드 함수화
    if문으로 입력처리가 아닌 다른 과정으로 처리

 **********************************************
 */


//플레이어 상태 인터페이스
public interface IPlayerState
{
    void OnEnter(Player player);    //상태 진입시 최초 할 행동
    void Update();                  //상태에 맞는 행동실행
    void OnExit();                  //상태가 끝날 때 할 행동
}

//플레이어 대기 상태
public class IdleState : IPlayerState
{
    private Player player;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;

        Log.Print("대기");
    }

    void IPlayerState.Update()
    {
        player.anim.Play("Idle");

        if (!player.isGrounded())
        {
            player.SetState(new JumpState());
        }

        else
        {
            //이동
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                player.SetState(new RunState());
            }

            //공격
            else if (Input.GetKeyDown(KeyCode.A))
            {
                player.SetState(new AttackState());
            }

            //점프
            else if (!Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))
            {
                player.SetState(new JumpState());
            }

            //하향 점프
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))
            {
                player.isJumpOff = true;
                player.SetState(new JumpState());
            }

            //구르기
            else if (Input.GetKeyDown(KeyCode.F))
            {
                player.SetState(new RollState());
            }
        }
    }

    void IPlayerState.OnExit()
    {
    }

}

//플레이어 이동 상태
public class RunState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        direction = player.transform.localScale;

        Log.Print("달리기");
    }

    void IPlayerState.Update()
    {
        player.anim.Play("Run");

        //좌측이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(Vector2.right * (-player.Speed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
            player.transform.localScale = direction;
        }

        //우측이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(Vector2.right * player.Speed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
            player.transform.localScale = direction;
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
    }

    //    private void bool OnSlope()
    //    {
    //        if (!player.isGrounded)
    //        {
    //            return false;
    //        }

    //        RayCastHit hit;

    //        if (Physics2D)
    //            }
    //     }
}

//플레이어 공격 상태
public class AttackState : IPlayerState
{
    private Player player;
    private float currentAnimTime;

    //공격 애니메이션 1 ~ 4
    private int currentAnim;

    //현재 재생되는 애니메이션을 비교해 공격 판정을 애니메이션당 한번만 실행하기 위함
    private int attackAnim;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        currentAnim = 1;
        attackAnim = 1;

        Log.Print("공격상태");
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        currentAnimTime = player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //1~4까지의 공격 애니메이션 실행
        player.anim.Play("Attack" + currentAnim.ToString());

        //공격 판정 검사 및 실행
        if ((currentAnimTime >= 0.6f && currentAnimTime < 0.7f) &&          //검을 휘두르는 스프라이트에 맞춰 몬스터 타격
           attackAnim == currentAnim)                                       //현재 애니메이션 비교
        {
            //다음 애니메이션에 대한 공격 판정을 위해 증가
            attackAnim++;
            Log.Print("플레이어 공격 단계: " + currentAnim + "단계 공격");
            //몬스터 타격
            SwordHitMonster();
        }

        //현재 애니메이션이 끝나면 다음 단계 애니메이션으로 전이
        else
        {
            if (currentAnimTime >= 0.99f &&
                player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1"))
            {
                //다음 애니메이션 시작을 위해 갱신
                currentAnim = 2;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack2"))
            {
                currentAnim = 3;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack3"))
            {
                currentAnim = 4;
            }

            else if (currentAnimTime >= 0.99f &&
                     player.anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack4"))
            {
                currentAnim = 1;
                attackAnim = 1;
            }

            else
            { }
        }
        //애니메이션 종료 후 아무 입력이 없으면 대기 상태로 전이
        if (!Input.GetKey(KeyCode.A) && currentAnimTime >= 0.99f)
        {
            player.SetState(new IdleState());
        }

    }

    void IPlayerState.OnExit()
    {
        //공격 애니메이션 최초 단계로 초기화
        currentAnim = 1;
        attackAnim = 1;

        //몬스터 해제
        player.hitTarget = null;
    }

    private void SwordHitMonster()
    {
        if (player.hitTarget)
        {
            player.hitTarget.Hit(player.offensePower); //테스트용으로 한방
            Log.Print("몬스터 타격!");
            Log.Print("몬스터 체력: " + player.hitTarget.HP);
        }

        else
        {
        }
    }
}

//플레이어 점프 상태
public class JumpState : IPlayerState
{
    private Player player;
    private Vector2 direction;
    static private uint doubleJump = 0;
    
    float timer = 0.0f;
    float delay = 0.05f;
    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        direction = this.player.transform.localScale;

        Log.Print("Enter JumpState");
        if(!this.player.isGrounded())
        {
            return;
        }
        //벽에 매달린 상태의 점프
        if (player.isCling)
        {
            doubleJump = 1;
            Log.Print("Player Doing cling jump");
            Log.Print("Jump Count: " + doubleJump);
            this.player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
            player.isCling = false;
        }


        else
        {
            //일반적인 점프
            if (!this.player.isJumpOff)
            {
                this.player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                doubleJump++;
            }

            //하향 점프
            else
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
            }
        }
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        //0.05초뒤 로직 처리
        //버그 방지
        timer += Time.deltaTime;
        if (timer > delay)
        {
            //플레이어의 공중 이동
            if (!player.isGrounded())
            {
                PlayJumpAnim();

                if (Input.GetKeyDown(KeyCode.D) && doubleJump < 2)
                {
                    Log.Print("Jump Count: " + doubleJump);
                    player.rb.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
                    doubleJump++;
                }

                if (Input.GetKeyDown(KeyCode.S) && player.CheckWall())
                {
                    player.SetState(new ClingState());
                }

                //좌측이동
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    //플레이어 좌측 이동
                    player.transform.Translate(Vector2.right.normalized * player.VerticalMove * Time.deltaTime, Space.World);
                    direction.x = Mathf.Abs(direction.x);                                                              //플레이어 방향전환
                    player.transform.localScale = direction;
                }

                //우측이동
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    //플레이어 우측 이동
                    player.transform.Translate(Vector2.left.normalized * player.VerticalMove * Time.deltaTime, Space.World);
                    direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
                    player.transform.localScale = direction;
                }

                //제자리 점프
                else
                {
                }
            }

            //땅에 착지 했을 시에 취하는 입력들
            else
            {
                doubleJump = 0;
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    player.SetState(new RunState());
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    player.SetState(new AttackState());
                }

                //구르기
                else if (Input.GetKey(KeyCode.F))
                {
                    player.SetState(new RollState());
                }

                //아무 입력이 없으면 대기 상태로 전이
                else if (!Input.anyKey)
                {
                    player.SetState(new IdleState());
                }

                //버그 방지
                else
                {
                    player.SetState(new IdleState());
                }
            }
        }
    }

    void IPlayerState.OnExit()
    {
        Log.Print("End JumpState");
        player.isJumpOff = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
    }

    private void PlayJumpAnim()
    {
        if (player.rb.velocity.y >= 8f)
        {
            player.anim.Play("Jump", 0, 0.2f);
        }

        else if (player.rb.velocity.y < 8f && player.rb.velocity.y > 0f)
        {
            player.anim.Play("Jump", 0, 0.4f);
        }

        //마이너스가 나올 수록 점점 빨라짐
        else if (player.rb.velocity.y < 0f)
        {
            player.anim.Play("Jump", 0, 0.6f);
        }

        else if (player.isJumpOff)
        {

            player.isJumpOff = false;
            return;
        }

        else
        {
            return;
        }
    }

}

//플레이어 구르기 상태
public class RollState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        direction = player.transform.localScale;
        this.player.anim.Play("Roll");
        this.player.playerRoll = true;
        Log.Print("구르기");
    }

    void IPlayerState.Update()
    {

        if (direction.x > 0)
        {
            player.transform.Translate(Vector2.right * player.RollLength * Time.deltaTime, Space.World);
            direction.x = Mathf.Abs(direction.x);
            player.transform.localScale = direction;

        }

        else
        {
            player.transform.Translate(Vector2.right * -player.RollLength * Time.deltaTime, Space.World);
            direction.x = -Mathf.Abs(direction.x);
            player.transform.localScale = direction;
        }

        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !Input.anyKeyDown)
        {
            player.SetState(new IdleState());
        }

        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            player.SetState(new RunState());
        }
    }

    void IPlayerState.OnExit()
    {
        player.playerRoll = false;
    }

}

public class ClingState : IPlayerState
{
    private Player player;
    private Vector3 direction;


    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        direction = this.player.transform.localScale;
        player.isCling = true;
    }

    void IPlayerState.Update()
    {
        player.anim.Play("Wall_cling");

        this.player.rb.velocity = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.SetState(new JumpState());
        }
    }

    void IPlayerState.OnExit()
    { 
    }


}

public class HitState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    //튕겨져 나가는 거리
    private const float bounceLength = 8;

    //튕겨져 나가는 힘
    private const float bounceForce = 12;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        //플레이어 방향
        direction = player.transform.localScale;

        this.player.anim.Play("Hit");

        //플레이어가 피격시 공중으로 튕겨져 나감
        this.player.rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        //플레이어가 공중에 있음을 확인

        this.player.playerHit = true;

        Log.Print("플레이어 타격!");
        Log.Print("현재 플레이어 체력: " + player.HP);
    }
    void IPlayerState.Update()
    {
        player.spriteRenderer.color = new Color(255, 0, 0);
        if (direction.x > 0)
        {
            player.transform.Translate(Vector2.left * bounceLength * Time.deltaTime, Space.World);
        }
        else
        {
            player.transform.Translate(Vector2.right * bounceLength * Time.deltaTime, Space.World);
        }

        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            player.playerHit = false;
            player.SetState(new IdleState());
        }
    }

    void IPlayerState.OnExit()
    {
        player.spriteRenderer.color = new Color(255, 255, 255);
    }
}

//현재 미구현 사항
public class DeadState : IPlayerState
{
    private Player player;
    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
        player.anim.Play("Die");
        Log.Print("Player Dead.");
    }

    void IPlayerState.Update()
    {
        return;
    }

    void IPlayerState.OnExit()
    {

    }
}

//****************************차후 구현해야할 기능들*************************

//플레이어 상호작용
//if(Input.GetKeyDown(KeyCode.UpArrow))

//벽 타기
//if(Input.GetKeyDown(KeyCode.S))

//일시정지
//if(Input.GetKeyDown(KeyCode.Escape))

//지도
//if(Input.GetKeyDown(KeyCode.Tab))