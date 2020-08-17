using UnityEngine;

//플레이어 구르기 상태
public class RollState : IPlayerState
{
    private Player player;
    private Vector2 direction;

    private float multiplyForceValue = 0f;

    public void OnEnter(Player player)
    {
        Log.Print("Enter RollState");
        this.player = player;
        direction = player.transform.localScale;
        this.player.anim.Play("Roll");

        multiplyForceValue = player.RollForce;

        //무적 활성화
        this.player.isInvincible = true;

        DoRoll();
    }

    public void Update()
    {
        if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !Input.anyKeyDown)
        {
            player.SetState(new IdleState());
        }

        else if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            player.SetState(new RunState());
        }
    }

    public void OnExit()
    {
        Log.Print("Exit RollState");

        //무적 해제
        player.isInvincible = false;
    }

    private void StandRoll()
    {
        if((player.rb.velocity.x >= 0f && player.rb.velocity.x <= 5f) ||
           (player.rb.velocity.x <= 0f && player.rb.velocity.x >= -5f))
        {
            multiplyForceValue *= 1.75f;
        }

        else
        {
            multiplyForceValue = player.RollForce;
        }
    }

    private void DoRoll()
    {
        StandRoll();

        if (direction.x > 0)
        {
            player.rb.AddForce(new Vector2(multiplyForceValue, 0.0f), ForceMode2D.Impulse);
            direction.x = Mathf.Abs(direction.x);
            player.transform.localScale = direction;
        }

        else
        {
            player.rb.AddForce(new Vector2(-multiplyForceValue, 0.0f), ForceMode2D.Impulse);
            direction.x = -Mathf.Abs(direction.x);
            player.transform.localScale = direction;
        }
    }
}

