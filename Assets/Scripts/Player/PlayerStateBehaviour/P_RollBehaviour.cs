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
        player.isInvincible = true;

        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);

        if (player.direction.x > 0)
        {
            player.rb.AddForce(new Vector2(player.RollForce * multipleForce, 0.0f), ForceMode2D.Impulse);
        } 

        else
        {
            player.rb.AddForce(new Vector2(-player.RollForce * multipleForce, 0.0f), ForceMode2D.Impulse);
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 0.99f || !player.Grounded)
        {
            player.Roll = false;
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //플레이어 무적 해제
        player.isInvincible = false;
        Log.Print("Player exit RollBehaviour");
    }
}
