using Boo.Lang;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//!Edit
public partial class Player : MonoBehaviour, IDamageable
{
    public PlatformEffector2D Platform { get; private set; } = null;

    private Inventory inventory = null;

    private ShopKeeper shopKeeper = null;
    public Monster.Monster HitTarget { get; set; }

    public bool IsJumpDown { get; set; } = false;
    public bool IsInvincible { get; set; } = false;

    [Header("지형 체크 설정")]
    [SerializeField]
    private Vector2 groundCheckBox = new Vector2(0.8f, 0.05f);
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    [SerializeField]
    private float groundCheckOffset = 0;
    [SerializeField]
    private float wallCheckOffset = 0;

    private Weapon weapon;
    
    private void Awake()
    {
        LoadToJsonData(ID);
        InitData();

        GetHashIDs();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Start()
    {
        weapon = GetComponentInChildren<Weapon>(true);
        
        Anim.SetFloat("HP", HP);

        inventory = FindObjectOfType<Inventory>();

        MenuOpened = false;
    }

    public void WeaponEnable(bool enable)
    {
        weapon.myCollider.enabled = enable;
        weapon.gameObject.SetActive(enable);
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

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            MenuOpened = !MenuOpened;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        Air = RB.velocity.y;

        if (Grounded)
        {
            //Anim.SetBool("IsGrounded", true);
            P_JumpBehaviour.doubleJump = false;
        }

        else
        {

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
        if (!IsInvincible)
        {
            Hit = true;
            HP -= value;
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
            if (Platform != null)
            {
                //다시 플랫폼으로 올라갈 수 있도록 오프셋 초기화
                Platform.rotationalOffset = 0;
                Platform.GetComponent<TilemapCollider2D>().enabled = true;
            }


            Grounded = true;
        }

        //플랫폼 체크
        else if (platformCheck != null)
        {
            Platform = Physics2D.BoxCast(startPos, groundCheckBox, 0f, Vector2.down, groundCheckDistance, platformLayer).collider.GetComponent<PlatformEffector2D>();

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
            Direction = new Vector2(Mathf.Abs(Direction.x), Direction.y);                                                       //플레이어 방향전환

        }

        //우측이동
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Direction = new Vector2(-Mathf.Abs(Direction.x), Direction.y);                                                              //플레이어 방향전환
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