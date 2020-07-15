using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 로직
public partial class Monster : MonoBehaviour
{
    protected virtual void GetHashIDs()
    {
        hash_Idle = Animator.StringToHash(m_Idle);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();

        GetHashIDs();
    }

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        LinkingAnimator();
    }

    public virtual void LinkingAnimator()
    {
        Anim.SetBool(hash_Idle, Idle);
    }
}
