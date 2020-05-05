using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;                       //플레이어 이동 속도(에디터 수정 가능)
    public float MoveSpeed { get { return _moveSpeed; } }

    private Animator anim;
    public Animator Anim { get { return anim; } }

    private IPlayerState _currentState;

    private void Awake()
    {
        SetState(new IdleState());                  //최초 게임 실행 시 대기 상태로 설정

        anim = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        _currentState.Update();                     //현재 상태에 따른 행동 실행
    }

    public void SetState(IPlayerState nextState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();                 //기존 상태 존재할 시 OnExit()호출
        }

        _currentState = nextState;                  //다음 상태 전이
        _currentState.OnEnter(this);                //다음 상태 진입
    }

}