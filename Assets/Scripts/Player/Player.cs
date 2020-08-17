using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
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

    //기본값 = 1
    private const int ID = 1;

    [SerializeField]
    private float offensePower = 0;
    public float OffensePower { get { return offensePower; } }

    private float defense = 0f;

    static private float hp = 0f;
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
    private float rollForce = 0f;
    public float RollForce { get { return rollForce; } }

    public Animator anim { get { return gameObject.GetComponent<Animator>(); } }
    public Rigidbody2D rb { get { return gameObject.GetComponent<Rigidbody2D>(); } }
    public SpriteRenderer spriteRenderer { get { return GetComponent<SpriteRenderer>(); } }
    public CapsuleCollider2D cc2D {get { return gameObject.GetComponent<CapsuleCollider2D>(); } }

    public Vector2 direction { get { return transform.localScale; } set { transform.localScale = value; } }

    private GameObject sword = null;
    public GameObject Sword { get { return sword; } }

    private PlatformEffector2D platform = null;
    public PlatformEffector2D Platform { get { return platform; } }

    private Image healthInterface = null;

    private Button restartButton = null;

    public Monster.Monster hitTarget { get; set; }

    public bool isJumpDown { get; set; } = false;
    public bool isInvincible { get; set; } = false;
    public bool isCling { get; set; } = false;
    public bool isHit { get; set; } = false;

    private IPlayerState _currentState;

    private MapLoader mapLoader = null;

    private void Awake()
    {
        LoadToJsonData(ID);
        SetData();

        //최초 게임 실행 시 대기 상태로 설정
        //SetState(new IdleState());

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Start()
    {
        healthInterface = GameObject.Find("HealthGauge").GetComponent<Image>();

        sword = GameObject.FindGameObjectWithTag("Weapon");

        sword.SetActive(false);

        mapLoader = GameObject.Find("Grid").GetComponent<MapLoader>();
        //restartButton = GameObject.Find("Restart").GetComponent<Button>();
    }

    private void Update()
    {
        //현재 상태에 따른 행동 실행
        //_currentState.Update();

        healthInterface.fillAmount = hp / 100;

        if(Input.GetKeyDown(KeyCode.Mouse2))
        {
            hp = playerData.HP;
            mapLoader.LoadMap(mapLoader.CurrentMapName, false);
            SetState(new IdleState());
        }
    }

    private void FixedUpdate()
    {
        //if (_currentState == new RunState())
        //{
        //    //속도 제한
        //    if (rb.velocity.x > Speed)
        //    {
        //        rb.velocity = new Vector2(Speed, rb.velocity.y);
        //    }


        //    else if (rb.velocity.x < -Speed)
        //    {
        //        rb.velocity = new Vector2(-Speed, rb.velocity.y);
        //    }
        //}

        //else
        //{
        //    if (rb.velocity.y < -16f)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, -16f);
        //    }

        //    else if (rb.velocity.y > 16f)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, 16f);
        //    }
        //}
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
        if (collision.collider.CompareTag("trap"))
        {
            GetDamaged(1000f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            hitTarget = collider.gameObject.GetComponent<Monster.Monster>();
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

    private void SetData()
    {
        offensePower = playerData.Offensepower;
        defense = playerData.Defense;
        if (hp <= 0)
        {
            hp = playerData.HP;
        }
        //speed = playerData.Speed;
    }

    public void GetDamaged(float value)
    {
        Log.Print("Player hit");
        //플레이어가 무적 상태가 아닐 때만
        if (!isInvincible)
        {
            hp -= value;
            if (hp <= 0)
            {
                //***죽음 상태 미완성***
                SetState(new DeadState());
            }

            else
            {
                SetState(new HitState());

            }
        }

        //무적 상태
        else
        {
            return;
        }
    }

    public bool CheckWall()
    {
        var floorLayer = LayerMask.GetMask("Floor");

        var checkPos = new Vector3(transform.position.x, transform.position.y + 1.5f);

        Vector3 rayDirection;

        //우측 확인
        if (transform.localScale.x > 0)
        {
            rayDirection = transform.right;
        }

        //좌측 확인
        else
        {
            rayDirection = -transform.right;
        }

        if (Physics2D.Raycast(checkPos, rayDirection, 0.6f, floorLayer))
        {
            Log.Print("CheckWall True");
            return true;
        }

        else
        {
            Log.Print("CheckWall False");
            return false;
        }
    }

    public bool isGrounded()
    {
        var floorLayer = LayerMask.GetMask("Floor");
        var platformLayer = LayerMask.GetMask("Platform");

        var startPos = new Vector3(transform.position.x, transform.position.y + 0.4f);
        var boxSize = new Vector2(0.8f, 0.05f);
        //바닥 체크   
        if (Physics2D.BoxCast(startPos, boxSize, 0f, Vector2.down, 0.2f, floorLayer).collider != null)
        {
            //하강 점프 완료 후
            if (Platform != null)
            {
                //다시 플랫폼으로 올라갈 수 있도록 오프셋 초기화
                Platform.rotationalOffset = 0;
                return true;
            }

            else
            {
                return true;
            }
        }

        //플랫폼 체크
        else if (Physics2D.BoxCast(startPos, boxSize, 0f, Vector2.down, 0.2f, platformLayer).collider != null)
        {
            platform = Physics2D.BoxCast(startPos, boxSize, 0f, Vector2.down, 0.2f, platformLayer).collider.GetComponent<PlatformEffector2D>();
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        var checkPos = new Vector3(transform.position.x, transform.position.y + 0.4f);
        var boxSize = new Vector3(0.7f, 0.05f, 0f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkPos, boxSize);
    }
}