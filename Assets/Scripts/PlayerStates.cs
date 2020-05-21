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

        _player.Anim.Play("idle");

        //이동
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _player.SetState(new RunState());
        }

        //공격
        if (Input.GetKeyDown(KeyCode.A))
        {
            _player.SetState(new AttackState());
        }

        //점프
        if (Input.GetKeyDown(KeyCode.D))
        {
            _player.SetState(new JumpState());
        }

        //구르기
        if (Input.GetKeyDown(KeyCode.F))
        {
            _player.SetState(new RollState());
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
    private Vector2 direction;
    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
        direction = _player.transform.localScale;
    }

    void IPlayerState.Update()
    {
        Debug.Log("RunState");
        _player.Anim.Play("Run");

        //좌측이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _player.transform.Translate(Vector2.right * (-_player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            direction.x = -Mathf.Abs(direction.x);                                                              //플레이어 방향전환
            _player.transform.localScale = direction;
        }

        //우측이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _player.transform.Translate(Vector2.right * _player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            direction.x = Mathf.Abs(direction.x);                                                               //플레이어 방향전환
            _player.transform.localScale = direction;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _player.SetState(new AttackState());
        }

        //점프
        if (Input.GetKeyDown(KeyCode.D))
        {
            _player.SetState(new JumpState());
        }

        //구르기
        if (Input.GetKeyDown(KeyCode.F))
        {
            _player.SetState(new RollState());
        }

        if (!Input.anyKey)
        {
            _player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
        }
    }

    void IPlayerState.OnExit()
    {
    }
}

//플레이어 공격 상태
public class AttackState : IPlayerState
{
    private Player _player;

    //공격 애니메이션 1 ~ 4
    private int attackCount = 1;

    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    { 
        _player.Anim.Play("Attack" + attackCount.ToString());

        float currentAnim = _player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;


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
            _player.SetState(new IdleState());              //아무 입력이 없으면 대기 상태로 전이
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
    private Player _player;
    void IPlayerState.OnEnter(Player player)
    {
        _player = player;

        //플레이어를 점프시킴
        _player.Rigidbody.AddForce(Vector2.up * _player.JumpForce, ForceMode2D.Impulse);
        _player.isGrounded = false;
    }

    //공격 상태에 따른 행동들
    void IPlayerState.Update()
    {
        Debug.Log("JumpState");
        _player.Anim.Play("Jump");

        //플레이어의 공중 이동
        if (!_player.isGrounded)
        {
            _player.Anim.Play("Jump", -1, 0.5f);

            //좌측이동
            if (Input.GetKey(KeyCode.LeftArrow) && !_player.isGrounded)
            {
                _player.transform.Translate(Vector2.right * (-_player.MoveSpeed) * Time.deltaTime, Space.World);    //플레이어 좌측 이동
            }

            //우측이동
            if (Input.GetKey(KeyCode.RightArrow) && !_player.isGrounded)
            {
                _player.transform.Translate(Vector2.right * _player.MoveSpeed * Time.deltaTime, Space.World);       //플레이어 우측 이동
            }
        }

        //땅에 착지 했을 시에 취하는 입력들
        else
        {

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                _player.SetState(new RunState());
            }

            //구르기
            if (Input.GetKey(KeyCode.F))
            {
                _player.SetState(new RollState());
            }

            if (!Input.anyKey)
            {
                _player.SetState(new IdleState());          //아무 입력이 없으면 대기 상태로 전이
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
    private Player _player;
    private Vector2 direction;

    void IPlayerState.OnEnter(Player player)
    {
        _player = player;
        direction = _player.transform.localScale;
    }

    void IPlayerState.Update()
    {
        _player.Anim.Play("Roll");

        if (_player.transform.localScale.x == 1f)
        {
            _player.transform.Translate(Vector2.right * 2.0f * Time.deltaTime, Space.World);
        }

        else
        {
            _player.transform.Translate(-Vector2.right * 2.0f * Time.deltaTime, Space.World);
        }

        if (_player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            _player.SetState(new IdleState());
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