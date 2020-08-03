using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽에 매달린 상태
public class ClingState : IPlayerState
{
    private Player player;

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

        if (!player.isGrounded() && player.CheckWall())
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                player.SetState(new JumpState());
            }
        }

        else
        {
            player.SetState(new IdleState());
        }
    }

    void IPlayerState.OnExit()
    {
        Log.Print("Exit ClingState");
        player.isCling = false;
    }
}
