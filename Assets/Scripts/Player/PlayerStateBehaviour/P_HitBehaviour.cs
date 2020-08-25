using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HitBehaviour : StateMachineBehaviour
{
    private Player player = null;

    private float bounceLength = 5;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter HitState");

        player = animator.gameObject.GetComponent<Player>();

        //플레이어가 피격시 공중으로 튕겨져 나감
        if (player.direction.x > 0)
        {
            var bounceForce = new Vector2(-bounceLength, 10);
            player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }

        else
        {
            var bounceForce = new Vector2(bounceLength, 10);
            player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.spriteRenderer.color = new Color(255, 0, 0);

        if (stateInfo.normalizedTime >= 0.99f)
        {
            animator.SetBool("IsHit", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit HitState");

        player.spriteRenderer.color = new Color(255, 255, 255);
    }
}
