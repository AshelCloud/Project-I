using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private struct PlayerData
    {
        string name;
        string variableName;
        float offensePower;
        float defense;
        float hp;
        float speed;
        string objectName;
        string animatorName;
        string prefabName;
    }

    private PlayerData playerData;
    private int ID;

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

    public bool isGrounded { get; set; }

    //공격 판정을 위한 GameObject
    public GameObject Sword { get { return GameObject.Find("Sword"); } }
    public Monster hitTarget;

    public bool rightMove { get; set; }
    public bool leftMove { get; set; }

    private IPlayerState _currentState;

    

    private void Awake()
    {
        ID = 1;

        //최초 게임 실행 시 대기 상태로 설정
        SetState(new IdleState());                  
        rightMove = false;
        leftMove = false;

        //검이 공격 상태일 때만 활성화
        Sword.SetActive(false);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void FixedUpdate()
    {
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
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            hitTarget = collider.gameObject.GetComponent<Monster>();
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
        var json = JsonManager.LoadJson<Serialization<string, PlayerData>>(Application.dataPath + "/Resources/PlayerJsons/", "Characters_Table").ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        playerData = json[ID.ToString()];
    }
}