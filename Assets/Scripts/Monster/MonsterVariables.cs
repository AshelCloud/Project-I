using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//몬스터 스크립트의 모든 변수
public partial class Monster : MonoBehaviour
{
    #region InspectorVariables
    public bool onAwakeIdle = true;
    public bool debug = true;
    public int ID = 0;
    public float patrolTransitionTime = 3f;
    #endregion

    #region Patrol
    protected double patrolTime;
    #endregion


    #region Animator Parameters
    public const string m_Idle = "Idle";

    private int hash_Idle;
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
                if(value)
                {
                    attack = value;
                    //TODO: 어택 아이디 구현
                }
            }
        }
    }

    public bool Idle { get { return idle; } }
    public MonsterDataTable DataTable { get { return dataTable; } }
    public Transform Target { get { return target; } }

    #endregion
}
