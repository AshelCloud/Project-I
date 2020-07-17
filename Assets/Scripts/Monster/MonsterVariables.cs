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
    #endregion

    #region Patrol
    protected float patrolTime;
    #endregion


    #region Animator Parameters
    public const string m_Idle = "Idle";
    public const string m_Attack = "Attack";

    private int hash_Idle;
    private int hash_Attack;
    #endregion

    #region Components
    protected Animator anim;
    protected Rigidbody2D _rigidbody;
    #endregion

    //플레이어를 의미
    protected Transform target;

    #region Animator Variables
    protected bool idle = true;

    protected bool 
            attack,
            isAttacking,
            damaged, 
            death;
    #endregion


    #region DataTable
    protected MonsterDataTable dataTable;

    protected string
            objectName,
            animatorName;

    protected float
            offensePower,
            defense,
            HP,
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

    public bool Attack
    {
        get { return attack; }
        set
        {
            if(value == false) { attack = value; } 

            if(death) { return; }

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
    public MonsterDataTable DataTable { get { return dataTable; } }
    public Transform Target { get { return target; } }

    public bool IsAttacking 
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public float DetectionRagne
    {
        get { return detectionRange + detectionRangeCorrection; }
    }

    public float OffentPower { get { return offensePower; } }
    #endregion
}
