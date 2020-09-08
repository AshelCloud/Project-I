using UnityEngine;
using UnityEngine.UI;

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

    private Animator animator = null;

    public PlatformEffector2D platform { get; private set; } = null;

    private Inventory inventory = null;

    private ShopKeeper shopKeeper = null;
    public Monster.Monster hitTarget { get; set; }

    public bool isJumpDown { get; set; } = false;
    public bool isInvincible { get; set; } = false;

    public bool isGrounded { get; private set; } = false;

    [Header("지형 체크 설정")]
    [SerializeField]
    private Vector2 groundCheckBox = new Vector2(0.8f, 0.05f);
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    [SerializeField]
    private float groundCheckOffset = 0;
    [SerializeField]
    private float wallCheckOffset = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        LoadToJsonData(ID);
        SetData();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Start()
    {
        animator.SetFloat("HP", hp);

        inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
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
    }

    private void FixedUpdate()
    {
        CheckGround();

        animator.SetFloat("inAir", rb.velocity.y);

        if (isGrounded)
        {
            animator.SetBool("IsGrounded", true);
            P_JumpBehaviour.doubleJump = false;
        }

        else
        {
            animator.SetBool("IsGrounded", false);
        }
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

    public void GetDamaged(float value)
    {
        Log.Print("Player hit");
        //플레이어가 무적 상태가 아닐 때만
        if (!isInvincible)
        {
            animator.SetFloat("HP", hp);
            hp -= value;
            if (hp <= 0)
            {
                return;
            }

            else
            {
                animator.SetBool("IsHit", true);

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


            }


            isGrounded = true;
        }

        //플랫폼 체크
        else if (platformCheck != null)
        {
            platform = Physics2D.BoxCast(startPos, groundCheckBox, 0f, Vector2.down, groundCheckDistance, platformLayer).collider.GetComponent<PlatformEffector2D>();

            isGrounded = true;
        }

        else
        {
            isGrounded = false;
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