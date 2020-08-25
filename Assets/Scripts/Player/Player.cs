using System.IO;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour, IDamageable
{
    public Animator animator { get { return gameObject.GetComponent<Animator>(); } }
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

    private MapLoader mapLoader = null;

    private void Awake()
    {
        LoadToJsonData(ID);
        SetData();

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

        healthInterface.fillAmount = hp / 100;
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

    public void ChangeDirection()
    {
        if (Input.GetAxis("Horizontal") >= 0)
        {
            direction = new Vector2(Mathf.Abs(direction.x), direction.y);                                                       //플레이어 방향전환

        }

        //우측이동
        else
        {
            direction = new Vector2(-Mathf.Abs(direction.x), direction.y);                                                              //플레이어 방향전환
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