﻿using UnityEngine;

public class P_AttackBehaviour : StateMachineBehaviour
{
    private Player player = null;

    private GameObject sword = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter AttackState");

        player = animator.gameObject.GetComponent<Player>();

        sword = player.transform.GetChild(0).gameObject;

        sword.SetActive(true);

        if(stateInfo.normalizedTime >= 0.6f && stateInfo.normalizedTime < 0.7f)
        {
            SwordHitMonster();
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsAttack", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit AttackState");

        sword.SetActive(false);
        player.hitTarget = null;
    }

    private void SwordHitMonster()
    {
        if (player.hitTarget)
        {
            player.hitTarget.GetDamaged(player.OffensePower);
            Log.Print("Monster HP: " + player.hitTarget.HP);
        }

        else
        {
        }
    }
}