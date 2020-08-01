using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//현재 미구현 사항
public class DeadState : IPlayerState
{
    private Player player;
    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter DeadState");
        this.player = player;
        player.anim.Play("Die");
        Log.Print("Player Dead.");
    }

    void IPlayerState.Update()
    {
        return;
    }

    void IPlayerState.OnExit()
    {
        Log.Print("Exit DeadState");
    }
}
