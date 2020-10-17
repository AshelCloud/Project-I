using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//!Edit
public partial class Player : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb 
    { 
        get 
        { 
            return gameObject.GetComponent<Rigidbody2D>(); 
        } 
    }

    public Vector2 direction 
    { 
        get 
        { 
            return transform.localScale; 
        } 

        set 
        { 
            transform.localScale = value; 
        } 
    }


    public PlatformEffector2D platform { get; private set; } = null;

    private Inventory inventory = null;

    private ShopKeeper shopKeeper = null;
    public Monster.Monster hitTarget { get; set; }

    public bool isJumpDown { get; set; } = false;
    public bool isInvincible { get; set; } = false;

    //public bool isGrounded { get; private set; } = false;

    public bool menuOpened { get; set; } = false;

    [Header("지형 체크 설정")]
    [SerializeField]
    private Vector2 groundCheckBox = new Vector2(0.8f, 0.05f);
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    [SerializeField]
    private float groundCheckOffset = 0;
    [SerializeField]
    private float wallCheckOffset = 0;

    InputControl inputControl;

    private void Awake()
    {
        LoadToJsonData(ID);
        InitData();

        GetHashIDs();

        inputControl = new InputControl(this);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Start()
    {
        Anim.SetFloat("HP", HP);

        inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        inputControl.UpdateInput();
        InterationEvent();

        if (shopKeeper)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Log.Print("Shopping Now");
                Item perchased = shopKeeper._ShopCanvas.Purchase();

                inventory.PerchasingItem(perchased);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !menuOpened)
        {
            menuOpened = true;
            var menu = Instantiate(ResourcesContainer.Load<GameObject>("Prefabs/UI/PauseMenu"), GameObject.Find("UICanvas").transform);
            menu.transform.SetParent(GameObject.Find("UICanvas").transform, false);
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        //Anim.SetFloat("inAir", rb.velocity.y);
        Air = rb.velocity.y;

        if (Grounded)
        {
            //Anim.SetBool("IsGrounded", true);
            P_JumpBehaviour.doubleJump = false;
        }

        else
        {
            //Anim.SetBool("IsGrounded", false);
            Grounded = false;
        }
    }

    private void LateUpdate()
    {
        LinkingAnimator();
        LinkingStatistic();
    }

    private void LinkingStatistic()
    {
        StatusUIManager.Instance.HPAmount = HP / MaxHP;
        //TODO: 스태미너 추가
    }

    private void LinkingAnimator()
    {
        Anim.SetBool(hash_Run, Run);
        Anim.SetBool(hash_Jump, Jump);
        Anim.SetBool(hash_Grounded, Grounded);
        Anim.SetBool(hash_Roll, Roll);
        Anim.SetBool(hash_Attack, Attack);
        Anim.SetBool(hash_Hit, Hit);
        Anim.SetBool(hash_Cling, Cling);
        Anim.SetFloat(hash_Air, Air);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("trap"))
        {
            GetDamaged(1000f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        StartCoroutine(NPCInteraction(collision));
    }

    private void GetHashIDs()
    {
        Log.Print("Player: Get Hash IDs");
        hash_Run = Animator.StringToHash(m_Run);
        hash_Jump = Animator.StringToHash(m_Jump);
        hash_Grounded = Animator.StringToHash(m_Grounded);
        hash_Roll = Animator.StringToHash(m_Roll);
        hash_Attack = Animator.StringToHash(m_Attack);
        hash_Hit = Animator.StringToHash(m_Hit);
        hash_Cling = Animator.StringToHash(m_Cling);
        hash_Air = Animator.StringToHash(m_Air);
    }

    public void GetDamaged(float value)
    {
        Log.Print("Player hit");
        //플레이어가 무적 상태가 아닐 때만
        if (!isInvincible)
        {
            HP -= value;
            Anim.SetFloat("HP", HP);

            if (HP <= 0)
            {
                return;
            }

            else
            {
                //Anim.SetBool("IsHit", true);
                Hit = true;
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

        var floorLayer = LayerMask.GetMask("Floor");

        var checkPos = new Vector3(transform.position.x + (wallCheckOffset * rayDirection.x), transform.position.y);



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

    public void CheckGround()
    {
        var floorLayer = LayerMask.GetMask("Floor");
        var platformLayer = LayerMask.GetMask("Platform");

        var startPos = new Vector3(transform.position.x, transform.position.y + groundCheckOffset);

        var groundCheck = Physics2D.BoxCast(startPos, groundCheckBox, 0f, Vector2.down, groundCheckDistance, floorLayer).collider;
        var platformCheck = Physics2D.BoxCast(startPos, groundCheckBox, 0f, Vector2.down, groundCheckDistance, platformLayer).collider;


        //바닥 체크   
        if (groundCheck != null)
        {
            //하강 점프 완료 후
            if (platform != null)
            {
                //다시 플랫폼으로 올라갈 수 있도록 오프셋 초기화
                platform.rotationalOffset = 0;
                platform.GetComponent<TilemapCollider2D>().enabled = true;
            }


            Grounded = true;
        }

        //플랫폼 체크
        else if (platformCheck != null)
        {
            platform = Physics2D.BoxCast(startPos, groundCheckBox, 0f, Vector2.down, groundCheckDistance, platformLayer).collider.GetComponent<PlatformEffector2D>();

            Grounded = true;
        }

        else
        {
            Grounded = false;
        }
    }

    public void ChangeDirection()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = new Vector2(Mathf.Abs(direction.x), direction.y);                                                       //플레이어 방향전환

        }

        //우측이동
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = new Vector2(-Mathf.Abs(direction.x), direction.y);                                                              //플레이어 방향전환
        }

        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        var checkPos = new Vector3(transform.position.x, transform.position.y + groundCheckOffset);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkPos, groundCheckBox);

        checkPos = new Vector3(transform.position.x + wallCheckOffset, transform.position.y);
        var goalPos = new Vector3(checkPos.x + 0.6f, checkPos.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(checkPos, goalPos);
    }
}