using System.Collections.Generic;
using UnityEngine;

//몬스터 스크립트의 모든 변수
public partial class Monster : MonoBehaviour
{
    #region InspectorVariables
    public bool onAwakeIdle = true;
    public bool debug = true;
    public int ID = 0;
    public float patrolTransitionTime = 3f;
    public float attackDelay;
    public float detectionRangeCorrection;
    public float attackRangeCorrection;
    #endregion

    #region Patrol
    protected float patrolTime;
    #endregion


    #region Animator Parameters
    public const string m_Idle = "Idle";
    public const string m_Chase = "Chase";
    public const string m_Attack = "Attack";
    public const string m_AttackID = "AttackID";
    public const string m_Damaged = "Damaged";
    public const string m_Dead = "Dead";

    private int hash_Idle;
    private int hash_Attack;
    private int hash_AttackID;
    private int hash_Chase;
    private int hash_Damaged;
    private int hash_Dead;
    #endregion

    #region Components
    protected Animator anim;
    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer _renderer;
    #endregion

    //플레이어를 의미
    protected Transform target;
    protected RaycastHit2D targetRay;


    #region Animator Variables
    protected bool idle = true;

    protected int attackID;

    protected bool 
            chase,
            attack,
            isAttacking,
            damaged, 
            dead;
    #endregion


    #region DataTable
    protected MonsterDataTable dataTable;

    protected string
            objectName,
            animatorName;

    protected float
            offensePower,
            defense,
            hp,
            speed;

    protected float 
            detectionRange,
            attackRange;

    protected int
            dropBundleID;
    #endregion

    #region Attack & Damage
    protected List<AttackTrigger> AttackTriggers;

    protected float attackTime;
    #endregion

    #region Properties
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }

            return anim;
        }
    }

    public Rigidbody2D _Rigidbody
    {
        get
        {
            if(_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody2D>();
            }

            return _rigidbody;
        }
    }

    public SpriteRenderer _Renderer
    {
        get
        {
            if(_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }

            return _renderer;
        }
    }

    public bool Attack
    {
        get { return attack; }
        set
        {
            if(value == false) { attack = value; } 

            if(dead) { return; }

            if(isAttacking == false)
            {
                if(value && Time.time - attackTime >= attackDelay)
                {
                    attack = value;
                    attackTime = Time.time;
                    //TODO: 어택 아이디 구현
                }
            }
        }
    }

    public bool Idle { get { return idle; } }

    public bool Chase { get { return chase; } }

    public MonsterDataTable DataTable { get { return dataTable; } }
    public Transform Target { get { return target; } }

    public int AttackID { get { return attackID; } }

    public bool IsAttacking 
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public bool Damaged { set { damaged = value; } get { return damaged; } }

    public float DetectionRagne
    {
        get { return detectionRange + detectionRangeCorrection; }
    }

    public float AttackRange
    {
        get { return attackRange + attackRangeCorrection; }
    }

    public float OffentPower { get { return offensePower; } }

    public float Speed { get { return speed; } }

    public float HP { get { return hp; } }
    #endregion
}
