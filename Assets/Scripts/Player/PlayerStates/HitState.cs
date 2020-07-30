using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    float timer = 0.0f;
    float delay = 0.5f;

    private float bounceLength = 5;

    void IPlayerState.OnEnter(Player player)
    {
        Log.Print("Enter HitState");
        this.player = player;

        //플레이어 방향
        direction = player.transform.localScale;

        this.player.isHit = true;

        //플레이어가 피격시 공중으로 튕겨져 나감
        if (direction.x > 0)
        {
            var bounceForce = new Vector2(-bounceLength, 10);
            this.player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }
        else
        {
            var bounceForce = new Vector2(bounceLength, 10);
            this.player.rb.AddForce(bounceForce, ForceMode2D.Impulse);
        }


        Log.Print("Current player HP: " + player.HP);
    }
    void IPlayerState.Update()
    {
        timer += Time.deltaTime;

        player.spriteRenderer.color = new Color(255, 0, 0);

        player.anim.Play("Hit");

        if (timer > delay)
        {
            Log.Print("Hit anim end");
            player.SetState(new IdleState());
        }
    }

    void IPlayerState.OnExit()
    {
        player.isHit = false;
        player.spriteRenderer.color = new Color(255, 255, 255);
        Log.Print("Exit HitState");
    }
}