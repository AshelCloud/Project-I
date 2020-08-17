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
            if (Input.anyKey)
            {
                foreach (var dic in InputControl.Instance.KeyDictionary)
                {
                    if(Input.GetKeyDown(dic.Key))
                    {
                        dic.Value();
                    }
                }
            }
        }
    }

    public void OnExit()
    {
        Log.Print("Exit IdleState");
    }

}