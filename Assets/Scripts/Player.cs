﻿using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    //이동 속도
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    //점프력
    [SerializeField]
    private float jumpForce;
    public float JumpForce { get { return jumpForce; } }

    //구르기 거리
    [SerializeField]
    private float rollLength;
    public float RollLength { get { return rollLength; } }

    public Animator Anim { get { return gameObject.GetComponent<Animator>(); } }
    public Rigidbody2D Rigidbody { get { return gameObject.GetComponent<Rigidbody2D>(); } }

    private bool grounded = false;

    public bool isGrounded { get { return grounded; } set { grounded = value; } }

    //공격 판정을 위한 GameObject
    private GameObject sword;
    public GameObject Sword { get { return sword; } }
    public Monster hitTarget;

    public bool rightMove;
    public bool leftMove;

    private IPlayerState _currentState;

    private void Awake()
    {
        SetState(new IdleState());                  //최초 게임 실행 시 대기 상태로 설정
        rightMove = false;
        leftMove = false;

        sword = GameObject.Find("Sword");

        //검이 공격 상태일 때만 활성화
        sword.SetActive(false);
    }

    private void Update()
    {
        //현재 상태에 따른 행동 실행
        _currentState.Update();                     

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Monster")
        {
            hitTarget = collider.gameObject.GetComponent<Monster>();
            Debug.Log("몬스터가 맞았다!");
        }
    }
}