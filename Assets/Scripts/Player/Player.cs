using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb { get { return gameObject.GetComponent<Rigidbody2D>(); } }

    public Vector2 direction { get { return transform.localScale; } set { transform.localScale = value; } }

    private PlatformEffector2D platform = null;
    public PlatformEffector2D Platform { get { return platform; } }

    private Image healthInterface = null;

    private Button restartButton = null;

    public Monster.Monster hitTarget { get; set; }

    public bool isJumpDown { get; set; } = false;
    public bool isInvincible { get; set; } = false;

    public bool isGrounded { get; private set; } = false;

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
        LoadToJsonData(ID);
        SetData();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"));
    }

    private void Start()
    {
        healthInterface = GameObject.Find("HealthGauge").GetComponent<Image>();

        //restartButton = GameObject.Find("Restart").GetComponent<Button>();
    }

    private void Update()
    {
        healthInterface.fillAmount = hp / 100;
    }

    private void FixedUpdate()
    {
        CheckGround();
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

    public void GetDamaged(float value)
    {
        Animator animator = gameObject.GetComponent<Animator>();

        Log.Print("Player hit");
        //플레이어가 무적 상태가 아닐 때만
        if (!isInvincible)
        {
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
            if (Platform != null)
            {
                //다시 플랫폼으로 올라갈 수 있도록 오프셋 초기화
                Platform.rotationalOffset = 0;


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