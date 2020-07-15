using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 스크립트의 모든 변수
public partial class Monster : MonoBehaviour
{
    protected bool idle = true;
    public bool Idle { get; }

    public const string m_Idle = "Idle";

    private int hash_Idle;


    protected Animator anim;

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
}
