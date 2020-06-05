using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void IPlayerState.Update()
    {
        Debug.Log("IdleState");

        player.Anim.Play("idle");

        //이동
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            player.SetState(new RunState());
        }

        //공격
        if (Input.GetKeyDown(KeyCode.A))
        {
            player.SetState(new AttackState());
        }

        //점프
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.SetState(new JumpState());
        }

        //구르기
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.SetState(new RollState());
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
    }

    void IPlayerState.Update()
    {
        Debug.Log("RunState");
        player.Anim.Play("Run");

        //좌측이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(Vector2.right * (-player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
            player.transform.localScale = direction;
        }

        //우측이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(Vector2.right * player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
            player.transform.localScale = direction;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            player.SetState(new AttackState());
        }

        //점프
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.SetState(new JumpState());
        }

        //구르기
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.SetState(new RollState());
        }

        if (!Input.anyKey)
        {
            player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
    }
}

//플레이어 공격 상태
public class AttackState : IPlayerState
{
    private Player player;

    //공격 애니메이션 1 ~ 4
    private int attackCount = 1;

    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    { 
        player.Anim.Play("Attack" + attackCount.ToString());

        float currentAnim = player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;


        //현재 애니메이션이 끝나면 다음 단계 애니메이션으로 전이
        if (currentAnim >= 0.99f)
        {
            attackCount++;
        }

        //마지막 애니메이션이 끝나면 최초 단계로 초기화
        else if (attackCount >= 4)
        {
            attackCount = 1;
        }

        if (!Input.GetKey(KeyCode.A) && currentAnim >= 0.99f)
        {
            player.SetState(new IdleState());              //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
        //공격 애니메이션 최초 단계로 초기화
        attackCount = 1;
    }

}

//플레이어 점프 상태
public class JumpState : IPlayerState
{
    private Player player;
    void IPlayerState.OnEnter(Player player)
    {
        this.player = player;

        //플레이어를 점프시킴
        player.Rigidbody.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        player.isGrounded = false;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        Debug.Log("JumpState");
        player.Anim.Play("Jump");

        //플레이어의 공중 이동
        if (!player.isGrounded)
        {
            player.Anim.Play("Jump", -1, 0.5f);

            //좌측이동
            if (Input.GetKey(KeyCode.LeftArrow) && !player.isGrounded)
            {
                player.transform.Translate(Vector2.right * (-player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            }

            //우측이동
            if (Input.GetKey(KeyCode.RightArrow) && !player.isGrounded)
            {
                player.transform.Translate(Vector2.right * player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            }
        }

        //땅에 착지 했을 시에 취하는 입력들
        else
        {

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                player.SetState(new RunState());
            }

            //구르기
            if (Input.GetKey(KeyCode.F))
            {
                player.SetState(new RollState());
            }

            if (!Input.anyKey)
            {
                player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
            }
        }

        }

    void IPlayerState.OnExit()
    {
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
    }

    void IPlayerState.Update()
    {
        player.Anim.Play("Roll");

        if (player.transform.localScale.x > 0)
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

        if (player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            player.SetState(new IdleState());
        }
    }

    void IPlayerState.OnExit()
    {
    }

}

//****************************차후 구현해야할 기능들*************************

//플레이어 상호작용
//if(Input.GetKeyDown(KeyCode.UpArrow))

//하향 점프
//if(Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))

//벽 타기
//if(Input.GetKeyDown(KeyCode.S))

//일시정지
//if(Input.GetKeyDown(KeyCode.Escape))

//지도
//if(Input.GetKeyDown(KeyCode.Tab))