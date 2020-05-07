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
    private Player _player;

    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
    }

    void IPlayerState.Update()
    {
        Debug.Log("IdleState");

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))    //idle -> run
        {
            _player.SetState(new RunState());
        }

        if (Input.GetKeyDown(KeyCode.A))                                                    //idle -> attack
        {
            _player.SetState(new AttackState());
        }
    }

    void IPlayerState.OnExit()
    { 
    }

}

//플레이어 이동 상태
public class RunState : IPlayerState
{
    private Player _player;
    private Vector3 direction;
    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
        _player.Anim.SetBool("isRunning", true);        //idle -> run 애니메이션 전이
        direction = _player.transform.localScale;
    }

    void IPlayerState.Update()
    {

        Debug.Log("MoveState");

        //좌측이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _player.transform.Translate(Vector3.right * (-_player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
            _player.transform.localScale = direction;
        }

        //우측이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _player.transform.Translate(Vector3.right * _player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
            _player.transform.localScale = direction;
        }

        if (!Input.anyKey)
        {
            _player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
        _player.Anim.SetBool("isRunning", false);       //walk -> idle
    }

}


//플레이어 공격 상태
/*
**********************************************************
    플레이어 공격 상태는 공격 애니메이션이 여러 개인 관계로
    각 공격 애니메이션 마다 전이가 이루어져야 했다
    그래서 Animator에서 bool변수를 많이 사용했다
**********************************************************
*/
public class AttackState : IPlayerState
{
    private Player _player;
    private Vector3 direction;
    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
        _player.Anim.SetBool("startAttack", true);          //idle -> attack 애니메이션 전이, 공격 상태 시작
        direction = _player.transform.localScale;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        Debug.Log("AttackState");                           //현재는 아무것도 없다, 차후 기능 추가

        if (!Input.anyKey)
        {
            _player.SetState(new IdleState());              //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
        _player.Anim.SetBool("isAttacking", false);         //attack -> idle
    }

}

//****************************차후 구현해야할 기능들*************************

//플레이어 상호작용
//if(Input.GetKeyDown(KeyCode.UpArrow))

//하향 점프
//if(Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))

//점프
//if(Input.GetKeyDown(KeyCode.D))

//공격
//if(Input.GetKeyDown(KeyCode.A))

//벽 타기
//if(Input.GetKeyDown(KeyCode.S))

//구르기
//if(Input.GetKeyDown(KeyCode.F))

//일시정지
//if(Input.GetKeyDown(KeyCode.Escape))

//지도
//if(Input.GetKeyDown(KeyCode.Tab))