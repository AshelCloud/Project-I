using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽에 매달린 상태
public class ClingState : IPlayerState
{
    private Player player;

    float timer = 0.0f;
    float delay = 0.5f;

    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter ClingState");
        this.player = player;
        player.isCling = true;
    }

    void IPlayerState.Update()
    {
        player.anim.Play("Wall_cling");

        this.player.rb.velocity = Vector2.zero;

        timer += Time.deltaTime;

        if (!player.isGrounded() && player.CheckWall())
        {
            if (timer > delay)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    player.SetState(new JumpState());
                }

                else if (Input.GetKeyDown(KeyCode.S))
                {
                    player.isCling = false;
                    player.SetState(new IdleState());
                }
            }
        }
        else
        {
            player.isCling = false;
            player.SetState(new IdleState());
        }
    }

    void IPlayerState.OnExit()
    {
        Log.Print("Exit ClingState");
    }
}
