using UnityEngine;

//플레이어 대기 상태
public class IdleState : IPlayerState
{
    private Player player;

    public void OnEnter(Player player)
    {
        Log.Print("Enter IdleState");
        this.player = player;
    }

    public void Update()
    {
        player.anim.Play("Idle");

        if (!player.isGrounded())
        {
            player.SetState(new JumpState());
        }

        else
        {
            //이동
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                player.SetState(new RunState());
            }

            //공격
            else if (Input.GetKeyDown(KeyCode.A))
            {
                player.SetState(new AttackState());
            }

            //점프
            else if (!Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))
            {
                player.SetState(new JumpState());
            }

            //플랫폼 하강 점프
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))
            {
                player.isJumpDown = true;
                player.SetState(new JumpState());
            }

            //구르기
            else if (Input.GetKeyDown(KeyCode.F))
            {
                player.SetState(new RollState());
            }
        }
    }

    public void OnExit()
    {
        Log.Print("Exit IdleState");
    }

}