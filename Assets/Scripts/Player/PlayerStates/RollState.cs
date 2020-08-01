using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 구르기 상태
public class RollState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter RollState");
        this.player = player;
        direction = player.transform.localScale;
        this.player.anim.Play("Roll");

        //무적 활성화
        this.player.isInvincible = true;

        if (direction.x > 0)
        {
            player.rb.AddForce(new Vector2(player.RollForce, 0.0f), ForceMode2D.Impulse);
            direction.x = Mathf.Abs(direction.x);
            player.transform.localScale = direction;
        }

        else
        {
            player.rb.AddForce(new Vector2(-player.RollForce, 0.0f), ForceMode2D.Impulse);
            direction.x = -Mathf.Abs(direction.x);
            player.transform.localScale = direction;
        }
    }

    void IPlayerState.Update()
    {



        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !Input.anyKeyDown)
        {
            player.SetState(new IdleState());
        }

        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            player.SetState(new RunState());
        }
    }

    void IPlayerState.OnExit()
    {
        Log.Print("Exit RollState");

        //무적 해제
        player.isInvincible = false;
    }

}

