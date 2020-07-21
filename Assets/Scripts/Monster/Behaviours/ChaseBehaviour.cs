using System;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    public float speedCorrection = 1f;

    private float speed;
    private Monster myMonster;

    private Rigidbody2D _Rigidbody2D;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myMonster = animator.GetComponent<Monster>();
        speed = myMonster.Speed * speedCorrection;
        _Rigidbody2D = myMonster._Rigidbody;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dir = (myMonster.Target.position - myMonster.transform.position).normalized.x;
        dir = Mathf.Clamp01(dir) == 0f ? -1f : 1f;

        //TODO: 개선
        if(dir < 0f && myMonster.transform.lossyScale.x > 0f)
        {
            myMonster.SetTurn();
        }
        else if(dir > 0f && myMonster.transform.lossyScale.x < 0f)
        {
            myMonster.SetTurn();
        }

        _Rigidbody2D.velocity = new Vector2(dir * speed * Time.deltaTime, _Rigidbody2D.velocity.y);
    }
}