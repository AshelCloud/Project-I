using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!Edit
public partial class Player : MonoBehaviour
{
    #region Animator Prameters
    public const string m_Run = "IsRun";
    public const string m_Jump = "IsJump";
    public const string m_Grounded = "IsGrounded";
    public const string m_Roll = "IsRoll";
    public const string m_Attack = "IsAttack";
    public const string m_Hit = "IsHit";
    public const string m_Cling = "IsCling";
    public const string m_Air = "inAir";

    private int hash_Run;
    private int hash_Jump;
    private int hash_Grounded;
    private int hash_Roll;
    private int hash_Attack;
    private int hash_Hit;
    private int hash_Cling;
    private int hash_Air;
    #endregion

    private Animator anim;


    public Animator Anim
    {
        get
        {
            if(anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }

    public bool Run 
    { get; set; }
    public bool Jump { get; set; }
    public bool Grounded { get; set; }

    private bool _roll;
    public bool Roll 
    { 
        get
        {
            return _roll;
        }

        set
        {
            if (Grounded)
            {
                _roll = value;
            }
        }
    }

    private bool _attack;
    public bool Attack 
    {
        get { return _attack; }
        set
        {
            if(Dead) { return; }

            _attack = value;
        }
    }
    public bool Hit { get; set; }
    public bool Cling { get; set; }
    public float Air { get; set; }
    public bool Dead { get; set; }

    private float _hp = 100;

    public float HP 
    {
        get
        {
            return _hp;
        }

        private set
        {
            if (_hp <= 0)
            {
                //TODO: Dead 활성화
            }
            else
            {
                _hp = value;
                Log.Print("Player HP: " + _hp);
            }
        }
    }
    public float MaxHP { get; private set; }

    private bool _menuOpened;
    public bool MenuOpened
    {
        get
        {
            return _menuOpened;
        }
        set
        {
            _menuOpened = value;
            MenuManager.Instance.MenuCanvas.enabled = _menuOpened;
        }
    }

    public Rigidbody2D RB
    {
        get
        {
            return gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public Vector2 Direction
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

    private bool _isShopping;
    public bool IsShopping
    {
        private set
        {
            _isShopping = value;
        }
        get
        {
            return _isShopping;
        }
    }
}
