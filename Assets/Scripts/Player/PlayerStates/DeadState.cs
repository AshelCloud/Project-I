//현재 미구현 사항
public class DeadState : IPlayerState
{
    private Player player;
    public void OnEnter(Player player)
    {
        Log.Print("Enter DeadState");
        this.player = player;
        player.anim.Play("Die");
        Log.Print("Player Dead.");
    }

    public void Update()
    {
        return;
    }

    public void OnExit()
    {
        Log.Print("Exit DeadState");
    }
}
