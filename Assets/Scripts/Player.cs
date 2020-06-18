﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.Timeline;



public class Player : MonoBehaviour
{
    [System.Serializable]
    private struct PlayerData
    {
        public string Name;
        public string Variablename;
        public float Offensepower;
        public float Defense;
        public float HP;
        public float Speed;
        public string Objectname;
        public string Animatorname;
        public string Prefabname;
    }


    private PlayerData playerData;
    private int ID = 1;

    public float offensePower { get; set; }
    private float defense;
    private float hp;

    //이동 속도
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    //점프력
    [SerializeField]
    private float jumpForce;
    public float JumpForce { get { return jumpForce; } }

    //구르기 거리
    [SerializeField]
    private float rollLength;
    public float RollLength { get { return rollLength; } }

    public Animator Anim { get { return gameObject.GetComponent<Animator>(); } }
    public Rigidbody2D rb { get { return gameObject.GetComponent<Rigidbody2D>(); } }

    public bool isGrounded { get; set; } = false;
    private float groundDistance;


    //공격 판정을 위한 GameObject
    private GameObject sword;
    public GameObject Sword { get { return sword; } }

    public Monster hitTarget { get; set; }

    //플레이어 점프 방향 구분
    public bool rightMove { get; set; } = false;
    public bool leftMove { get; set; } = false;

    public bool playerHit { get; set; } = false;

    private IPlayerState _currentState;

    private void Awake()
    {
        LoadToJsonData(ID);
        UpdateData();

        sword = GameObject.Find("Sword");
        //최초 게임 실행 시 대기 상태로 설정
        SetState(new IdleState());


        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Update()
    {
        //현재 상태에 따른 행동 실행
        _currentState.Update();

        //추후 필히 삭제
        //피격 디버그
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            HitByMonster(0);
        }
    }

    public void SetState(IPlayerState nextState)
    {
        if (_currentState != null)
        {
            //기존 상태 존재할 시 OnExit()호출
            _currentState.OnExit();
        }

        //다음 상태 전이
        _currentState = nextState;

        //다음 상태 진입
        _currentState.OnEnter(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && playerHit != true)
        {
            isGrounded = false;
            SetState(new JumpState());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            hitTarget = collider.gameObject.GetComponent<Monster>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            hitTarget = null;
        }
    }

    private void LoadToJsonData(int ID)
    {
        //테이블 ID는 1부터 시작
        //ID가 기본값이면 에러로그 출력
        if (ID == 0)
        {
            Debug.LogError("데이터 로드 실패! ID를 설정해주세요");
            return;
        }

        //Json 파싱
        var json = JsonManager.LoadJson<Serialization<string, PlayerData>>(Application.dataPath + "/Resources/PlayerJson/", "Characters_Table").ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        playerData = json[ID.ToString()];
    }

    private void UpdateData()
    {
        offensePower = playerData.Offensepower;
        defense = playerData.Defense;
        hp = playerData.HP;
        speed = playerData.Speed;
    }

    public void HitByMonster(float damage)
    {
        //플레이어가 맞은 상태가 아닐 때만
        if (!playerHit)
        {
            hp -= damage;
            if (hp <= 0)
            {
                //죽음 상태 부분구현
                SetState(new DeadState());
            }

            else
            {
                SetState(new HitState());
                
            }
        }
        
        //무적 상태 테스트
        else
        {
            //yield return new WaitForSeconds(1f);
            playerHit = false;
        }
    }
}