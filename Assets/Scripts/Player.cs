using UnityEngine;
using System.IO;
using UnityEditor;

public class Player : MonoBehaviour
{
    [System.Serializable]
    private class PlayerData
    {
        public string Name = null;
        public string Variablename = null;
        public float Offensepower = 0f;
        public float Defense = 0f;
        public float HP = 0f;
        public float Speed = 0f;
        public string Objectname = null;
        public string Animatorname = null;
        public string Prefabname = null;
    }


    private PlayerData playerData;
    private int ID = 1;

    public float offensePower { get; set; }
    private float defense = 0f;
    private float hp = 0f;
    public float HP { get { return hp; } }

    //이동 속도
    [SerializeField]
    private float speed = 0f;
    public float Speed { get { return speed; } }

    //점프력
    [SerializeField]
    private float jumpForce = 0f;
    public float JumpForce { get { return jumpForce; } }

    //구르기 거리
    [SerializeField]
    private float rollLength = 0f;
    public float RollLength { get { return rollLength; } }

    //공중 이동 능력
    [SerializeField]
    private float verticalMove = 0f;
    public float VerticalMove { get { return verticalMove; } }

    public Animator anim { get { return gameObject.GetComponent<Animator>(); } }
    public Rigidbody2D rb { get { return gameObject.GetComponent<Rigidbody2D>(); } }

    //공격 판정을 위한 GameObject
    private GameObject sword;
    public GameObject Sword { get { return sword; } }

    public Monster hitTarget { get; set; }

    public bool isJumpOff { get; set; } = false;
    public bool playerHit { get; set; } = false;
    public bool playerRoll { get; set; } = false;

    //public bool isGrounded { get; set; } = false;
    public bool isTouchWall { get; set; } = false;
    public bool isCling { get; set; } = false;
    private IPlayerState _currentState;

    public SpriteRenderer spriteRenderer { get { return GetComponent<SpriteRenderer>(); } }

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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
    //    {
    //        isGrounded = true;
    //    }

    //    else if (collision.gameObject.tag == "trap")
    //    {
    //        SetState(new DeadState());
    //    }

    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Floor" && playerHit != true && playerRoll != true)
    //    {
    //        isGrounded = false;
    //        SetState(new JumpState());
    //    }
    //}

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
        AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
        
        if (localAssetBundle == null)
        {
            Log.PrintError("Failed to load AssetBundle!");
        }

        if (ID == 0)
        {
            Log.PrintError("Failed to Player Data, ID is null or 0");
            return;
        }

        TextAsset json = localAssetBundle.LoadAsset<TextAsset>("Characters_Table");

        //Json 파싱
        var playerDatas = JsonManager.LoadJson<Serialization<string, PlayerData>>(json).ToDictionary();

        //ID 값으로 해당되는 Data 저장
        //ID는 각 몬스터 스크립트에서 할당
        playerData = playerDatas[ID.ToString()];
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

    public bool CheckWall()
    {
        var floorLayer = LayerMask.GetMask("Floor");

        var checkPos = new Vector3(transform.position.x, transform.position.y + 1.5f);

        Vector3 rayDirection;

        if (transform.localScale.x > 0)
        {
            rayDirection = transform.right;
        }

        else
        {
            rayDirection = -transform.right;
        }

        if (Physics2D.Raycast(checkPos, rayDirection, 0.9f, floorLayer))
        {
            Log.Print("CheckWall");
            return true;
        }

        else
        {
            Log.Print("Nothing");
            return false;
        }
    }

    public bool isGrounded()
    {
        var floorLayer = LayerMask.GetMask("Floor");
        var startPos = new Vector3(transform.position.x, transform.position.y + 0.4f);

        if (Physics2D.Raycast(startPos, -transform.up, 0.3f, floorLayer).distance > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        var checkPos = new Vector3(transform.position.x, transform.position.y + 1.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(checkPos, checkPos + new Vector3(0.9f, 0));

        checkPos = new Vector3(transform.position.x, transform.position.y + 0.4f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(checkPos, checkPos + new Vector3(0, -0.3f));
    }
}