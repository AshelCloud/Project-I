using UnityEngine;

public class P_RollBehaviour : StateMachineBehaviour
{
    private Player player = null;

    [SerializeField]
    private float multipleForce;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player enter RollBehaviour");

        player = animator.GetComponent<Player>();

        //플레이어 무적 설정
        player.IsInvincible = true;

        player.RB.velocity = new Vector2(0f, player.RB.velocity.y);

        player.Stamina -= player.StaminaRedution;

        if (player.Direction.x > 0)
        {
            player.RB.AddForce(new Vector2(player.RollForce * multipleForce, 0.0f), ForceMode2D.Impulse);
        } 

        else
        {
            player.RB.AddForce(new Vector2(-player.RollForce * multipleForce, 0.0f), ForceMode2D.Impulse);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Log.Print("Player exit RollBehaviour");
        player.Roll = false;
        //플레이어 무적 해제
        player.IsInvincible = false;
    }
}
